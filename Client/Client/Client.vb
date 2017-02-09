Imports System.Windows.Forms
Imports System.Threading
Imports System.Net.Sockets
Imports System.Net
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
    Dim portNumber As Integer
    Dim hostName As String
    Dim hostIPAddr As String

    Public Sub New()
        MyBase.New()

        InitializeComponent()

        hostName = Dns.GetHostName()
        hostIPAddr = Dns.GetHostEntry(hostName).AddressList(3).ToString()
        lblHostIpAddr.Text &= hostIPAddr


    End Sub

    Private Sub FrmClient_Closing(ByVal sender As System.Object,
                                  ByVal e As System.ComponentModel.CancelEventArgs) _
                                Handles MyBase.Closing

        System.Environment.Exit(System.Environment.ExitCode)
    End Sub

    Private Sub txtInput_KeyDown(ByVal sender As System.Object,
                                   ByVal e As System.Windows.Forms.KeyEventArgs) _
                                Handles txtInput.KeyDown

        Try

            If e.KeyCode = Keys.Enter Then

                writer.Write("CLIENT>>> " & txtInput.Text)

                txtDisplay.Text &= vbCrLf & "CLIENT>>> " &
                    txtInput.Text

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
            client.Connect(hostIPAddr, portNumber)

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
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.lblPort = New System.Windows.Forms.Label()
        Me.numPort = New System.Windows.Forms.NumericUpDown()
        Me.lblHostIpAddr = New System.Windows.Forms.Label()
        CType(Me.numPort, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtInput
        '
        Me.txtInput.Location = New System.Drawing.Point(12, 137)
        Me.txtInput.Name = "txtInput"
        Me.txtInput.Size = New System.Drawing.Size(392, 20)
        Me.txtInput.TabIndex = 0
        '
        'txtDisplay
        '
        Me.txtDisplay.Location = New System.Drawing.Point(12, 163)
        Me.txtDisplay.Multiline = True
        Me.txtDisplay.Name = "txtDisplay"
        Me.txtDisplay.Size = New System.Drawing.Size(392, 343)
        Me.txtDisplay.TabIndex = 1
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(317, 36)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(87, 23)
        Me.btnConnect.TabIndex = 2
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'lblPort
        '
        Me.lblPort.AutoSize = True
        Me.lblPort.Location = New System.Drawing.Point(314, 15)
        Me.lblPort.Name = "lblPort"
        Me.lblPort.Size = New System.Drawing.Size(29, 13)
        Me.lblPort.TabIndex = 3
        Me.lblPort.Text = "Port:"
        '
        'numPort
        '
        Me.numPort.Location = New System.Drawing.Point(349, 13)
        Me.numPort.Maximum = New Decimal(New Integer() {6000, 0, 0, 0})
        Me.numPort.Minimum = New Decimal(New Integer() {2000, 0, 0, 0})
        Me.numPort.Name = "numPort"
        Me.numPort.Size = New System.Drawing.Size(55, 20)
        Me.numPort.TabIndex = 4
        Me.numPort.Value = New Decimal(New Integer() {2000, 0, 0, 0})
        '
        'lblHostIpAddr
        '
        Me.lblHostIpAddr.AutoSize = True
        Me.lblHostIpAddr.Location = New System.Drawing.Point(9, 15)
        Me.lblHostIpAddr.Name = "lblHostIpAddr"
        Me.lblHostIpAddr.Size = New System.Drawing.Size(98, 13)
        Me.lblHostIpAddr.TabIndex = 5
        Me.lblHostIpAddr.Text = "Your Ip Address is: "
        '
        'frmClient
        '
        Me.ClientSize = New System.Drawing.Size(419, 518)
        Me.Controls.Add(Me.lblHostIpAddr)
        Me.Controls.Add(Me.numPort)
        Me.Controls.Add(Me.lblPort)
        Me.Controls.Add(Me.btnConnect)
        Me.Controls.Add(Me.txtDisplay)
        Me.Controls.Add(Me.txtInput)
        Me.Name = "frmClient"
        Me.Text = "Client"
        CType(Me.numPort, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnConnect As System.Windows.Forms.Button
    Friend WithEvents lblPort As System.Windows.Forms.Label
    Friend WithEvents numPort As System.Windows.Forms.NumericUpDown

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click

        portNumber = numPort.Value

        readThread = New Thread(AddressOf RunClient)
        readThread.Start()
    End Sub
    Friend WithEvents lblHostIpAddr As System.Windows.Forms.Label
End Class
