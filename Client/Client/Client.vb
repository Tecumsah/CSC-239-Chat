Imports System.Windows.Forms
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
    'Friend WithEvents txtInput As TextBox
    'Friend WithEvents txtDisplay As TextBox
    Private readThread As Thread

    Public Sub New()
        MyBase.New()

        InitializeComponent()

        readThread = New Thread(AddressOf RunClient)
        readThread.Start()
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
            client.Connect("10.19.5.139", 5000)

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
        Me.SuspendLayout()
        '
        'txtInput
        '
        Me.txtInput.Location = New System.Drawing.Point(12, 12)
        Me.txtInput.Name = "txtInput"
        Me.txtInput.Size = New System.Drawing.Size(682, 31)
        Me.txtInput.TabIndex = 0
        '
        'txtDisplay
        '
        Me.txtDisplay.Location = New System.Drawing.Point(12, 49)
        Me.txtDisplay.Multiline = True
        Me.txtDisplay.Name = "txtDisplay"
        Me.txtDisplay.Size = New System.Drawing.Size(682, 457)
        Me.txtDisplay.TabIndex = 1
        '
        'frmClient
        '
        Me.ClientSize = New System.Drawing.Size(706, 518)
        Me.Controls.Add(Me.txtDisplay)
        Me.Controls.Add(Me.txtInput)
        Me.Name = "frmClient"
        Me.Text = "Client"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
End Class
