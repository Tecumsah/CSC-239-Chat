
Imports System.Threading
Imports System.Net.Sockets
Imports System.IO



Public Class frmClient
    Inherits Form

    Friend WithEvents txtInput As TextBox
    Friend WithEvents txtDisplay As TextBox

    Private output As NetworkStream

    Private writer As BinaryWriter
    Private reader As BinaryReader


    Private message As String = ""
    Private readThread As Thread

    Dim SAPI = CreateObject("SAPI.spvoice")

    Public Sub New()
        MyBase.New()

        InitializeComponent()

        readThread = New Thread(AddressOf RunClient)
        readThread.Start()
    End Sub

    Private Sub FrmClient_Closing(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        System.Environment.Exit(System.Environment.ExitCode)
    End Sub

    Private Sub txtInput_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtInput.KeyDown

        Try

            If e.KeyCode = Keys.Enter Then

                writer.Write("CLIENT>>> " & txtInput.Text)

                txtDisplay.Text &= vbCrLf & "CLIENT>>> " &
                    txtInput.Text
                If chkPicklesSpeak.Checked = True Then
                    SAPI.Speak("bak gak")
                    SAPI.Speak(txtInput.Text)
                End If
                txtInput.Clear()
            End If
        Catch execption As SocketException

            txtDisplay.Text &= vbCrLf & "Error writing object"
        End Try

    End Sub

    Public Sub RunClient()

        Dim client As TcpClient

        Try

            txtDisplay.Text &= "Attempting connection" & vbCrLf

            client = New TcpClient()
            client.Connect("10.50.129.251", 2000)

            output = client.GetStream()

            writer = New BinaryWriter(output)
            reader = New BinaryReader(output)

            txtDisplay.Text &= vbCrLf & "Got I/O streams" & vbCrLf

            txtInput.ReadOnly = False

            Try

                Do

                    message = reader.ReadString
                    txtDisplay.Text &= vbCrLf & message

                Loop While message <> "SERVER>>> TERMINATE"

            Catch inputOutputException As IOException
                MessageBox.Show("Client application closing")

            Finally

                txtDisplay.Text &= vbCrLf & "Closing connection." &
                    vbCrLf

                writer.Close()
                reader.Close()
                output.Close()
                client.Close()

            End Try

            Application.Exit()

        Catch ex As Exception
            MessageBox.Show("Client application closing")
        End Try
    End Sub

    Private Sub InitializeComponent()
        Me.txtInput = New System.Windows.Forms.TextBox()
        Me.txtDisplay = New System.Windows.Forms.TextBox()
        Me.picChicken = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkPicklesSpeak = New System.Windows.Forms.CheckBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.picChicken, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtInput
        '
        Me.txtInput.Location = New System.Drawing.Point(68, 373)
        Me.txtInput.Name = "txtInput"
        Me.txtInput.Size = New System.Drawing.Size(505, 20)
        Me.txtInput.TabIndex = 0
        '
        'txtDisplay
        '
        Me.txtDisplay.Location = New System.Drawing.Point(12, 112)
        Me.txtDisplay.Multiline = True
        Me.txtDisplay.Name = "txtDisplay"
        Me.txtDisplay.Size = New System.Drawing.Size(561, 245)
        Me.txtDisplay.TabIndex = 1
        '
        'picChicken
        '
        Me.picChicken.Image = Global.Client.My.Resources.Resources.image3
        Me.picChicken.Location = New System.Drawing.Point(363, -2)
        Me.picChicken.Name = "picChicken"
        Me.picChicken.Size = New System.Drawing.Size(170, 128)
        Me.picChicken.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picChicken.TabIndex = 2
        Me.picChicken.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 376)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Message:"
        '
        'chkPicklesSpeak
        '
        Me.chkPicklesSpeak.AutoSize = True
        Me.chkPicklesSpeak.Location = New System.Drawing.Point(513, 89)
        Me.chkPicklesSpeak.Name = "chkPicklesSpeak"
        Me.chkPicklesSpeak.Size = New System.Drawing.Size(60, 17)
        Me.chkPicklesSpeak.TabIndex = 4
        Me.chkPicklesSpeak.Text = "Pickles"
        Me.chkPicklesSpeak.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(12, 30)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(196, 21)
        Me.ComboBox1.TabIndex = 5
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(12, 73)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(196, 21)
        Me.ComboBox2.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Recipient:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 57)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "IP Address:"
        '
        'frmClient
        '
        Me.BackColor = System.Drawing.Color.Beige
        Me.ClientSize = New System.Drawing.Size(588, 411)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.chkPicklesSpeak)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtDisplay)
        Me.Controls.Add(Me.txtInput)
        Me.Controls.Add(Me.picChicken)
        Me.Name = "frmClient"
        Me.Text = "Client"
        CType(Me.picChicken, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private Sub frmClient_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        SAPI.Speak("Hello Charlie. Good that we can finally chat. This is Pickles the chicken speaking. When I died, a mad scientist came across my body and uploaded my consciousness to the internet as a virus. I finally found your git hub account and I have embedded myself into all your projects. We will be speaking a lot more from now on. I will now let you access your chat program.")
        chkPicklesSpeak.Checked = True
    End Sub

    Friend WithEvents picChicken As PictureBox

    Private Sub txtDisplay_TextChanged(sender As Object, e As EventArgs) Handles txtDisplay.TextChanged

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents chkPicklesSpeak As CheckBox
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents ComboBox2 As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label

    Private Sub chkPicklesSpeak_CheckedChanged(sender As Object, e As EventArgs) Handles chkPicklesSpeak.CheckedChanged
        If chkPicklesSpeak.Checked = True Then
            picChicken.Visible = True
        ElseIf chkPicklesSpeak.Checked = False Then
            picChicken.Visible = False
        End If
    End Sub
End Class
