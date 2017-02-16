Imports System.Windows.Forms
Imports System.Threading
Imports System.Net.Sockets
Imports System.Net
Imports System.IO

Public Class FrmServer
    Inherits Form

    Friend WithEvents txtInput As TextBox
    Friend WithEvents txtDisplay As TextBox

    Private connection As Socket
    Private readThread As Thread

    Private socketStream As NetworkStream

    Private writer As BinaryWriter
    Private reader As BinaryReader

    Public Sub New()
        MyBase.New()

        InitializeComponent()

        readThread = New Thread(AddressOf RunServer)
        readThread.Start()
    End Sub

    Private Sub frmServer_Closing(
                                   ByVal sender As System.Object,
                                   ByVal e As System.ComponentModel.CancelEventArgs) _
                                    Handles MyBase.Closing

        System.Environment.Exit(System.Environment.ExitCode)
    End Sub

    Private Sub txtInput_KeyDown(ByVal sender As System.Object,
                                  ByVal e As System.Windows.Forms.KeyEventArgs) _
                                    Handles txtInput.KeyDown

        Try

            If (e.KeyCode = Keys.Enter AndAlso
            Not connection Is Nothing) Then

                writer.Write("SERVER>>> " & txtInput.Text)
                txtDisplay.Text &= vbCrLf & "SERVER>>> " &
                    txtInput.Text

                If txtInput.Text = "TERMINATE" Then
                    connection.Close()
                End If
                txtInput.Clear()
            End If

        Catch ex As SocketException
            txtDisplay.Text &= vbCrLf & "Error writing object"
        End Try
    End Sub

    Public Sub RunServer()

        Dim listener As TcpListener
        Dim counter As Integer = 1

        Try

            Dim localAddr As IPAddress = IPAddress.Parse("10.50.129.252")
            listener = New TcpListener(localAddr, 5000)
            listener.Start()

            While True
                txtDisplay.Text = "Waiting for connection" & vbCrLf
                connection = listener.AcceptSocket()

                socketStream = New NetworkStream(connection)

                writer = New BinaryWriter(socketStream)
                reader = New BinaryReader(socketStream)

                txtDisplay.Text &= "Connection" & counter &
                    " received." & vbCrLf

                writer.Write("SERVER>>> Connection Successful")

                txtInput.ReadOnly = False
                Dim theReply As String = ""

                Try

                    Do
                        theReply = reader.ReadString()

                        txtDisplay.Text &= vbCrLf & theReply

                    Loop While (theReply <> "Client>>> TERMINATE" _
                        AndAlso connection.Connected)

                Catch inputOutputExecption As IOException
                    MessageBox.Show("Client application closing")
                Finally

                    txtDisplay.Text &= vbCrLf & "User terminated connection."

                    txtInput.ReadOnly = True

                    writer.Close()
                    reader.Close()
                    socketStream.Close()
                    connection.Close()

                    counter += 1


                End Try
            End While

        Catch inputOutputException As IOException
            MessageBox.Show("Server Application Closing")
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
        Me.txtInput.Size = New System.Drawing.Size(756, 31)
        Me.txtInput.TabIndex = 0
        '
        'txtDisplay
        '
        Me.txtDisplay.Location = New System.Drawing.Point(12, 49)
        Me.txtDisplay.Multiline = True
        Me.txtDisplay.Name = "txtDisplay"
        Me.txtDisplay.Size = New System.Drawing.Size(756, 499)
        Me.txtDisplay.TabIndex = 1
        '
        'FrmServer
        '
        Me.ClientSize = New System.Drawing.Size(780, 560)
        Me.Controls.Add(Me.txtDisplay)
        Me.Controls.Add(Me.txtInput)
        Me.Name = "FrmServer"
        Me.Text = "Server"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
End Class