
Imports System.Threading
Imports System.Net.Sockets
Imports System.Net
Imports System.IO

Public Class FrmServer
    Inherits Form

    Friend WithEvents txtInput As TextBox
    Friend WithEvents txtDisplay As TextBox
    Friend WithEvents lblPort As System.Windows.Forms.Label
    Friend WithEvents btnStart As System.Windows.Forms.Button
    Friend WithEvents numPort As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblHostIPAddr As System.Windows.Forms.Label
    Friend WithEvents btnStop As Button

    Private connection As Socket
    Private readThread As Thread

    Private socketStream As NetworkStream

    Private writer As BinaryWriter
    Private reader As BinaryReader

    Dim portNumber As Integer
    Dim hostName As String
    Dim hostIpAddr As String

    Public Sub New()
        MyBase.New()

        InitializeComponent()

        hostIpAddr = LocalIP()
        lblHostIPAddr.Text &= hostIpAddr
        readThread = New Thread(AddressOf RunServer)


    End Sub

    Public Function LocalIP() As String
        ' Obtain the first address of local machine with addressing scheme
        For Each IP As IPAddress In Dns.GetHostEntry(Dns.GetHostName()).AddressList
            If IP.AddressFamily.ToString() = "InterNetwork" Then
                Return (IP.ToString())
            End If
        Next IP
        Return (vbNullString)
    End Function

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click


        portNumber = numPort.Value
        readThread.Start()

        numPort.Enabled = False
        btnStart.Enabled = False
        'btnStart.Visible = False
        'btnStop.Enabled = True
        'btnStop.Visible = True

    End Sub

    'Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click

    '    txtDisplay.Text &= vbCrLf & "Terminating connection."

    '    Try
    '        readThread.Abort()

    '    Catch ex As Exception

    '    End Try

    '    btnStart.Enabled = True
    '    btnStart.Visible = True
    '    btnStop.Enabled = False
    '    btnStop.Visible = False
    'End Sub

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

            Dim localAddr As IPAddress = IPAddress.Parse(hostIpAddr)
            listener = New TcpListener(localAddr, portNumber)
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
        Me.lblPort = New System.Windows.Forms.Label()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.numPort = New System.Windows.Forms.NumericUpDown()
        Me.lblHostIPAddr = New System.Windows.Forms.Label()
        Me.btnStop = New System.Windows.Forms.Button()
        CType(Me.numPort, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtInput
        '
        Me.txtInput.Location = New System.Drawing.Point(12, 108)
        Me.txtInput.Name = "txtInput"
        Me.txtInput.Size = New System.Drawing.Size(366, 20)
        Me.txtInput.TabIndex = 0
        '
        'txtDisplay
        '
        Me.txtDisplay.Location = New System.Drawing.Point(12, 134)
        Me.txtDisplay.Multiline = True
        Me.txtDisplay.Name = "txtDisplay"
        Me.txtDisplay.Size = New System.Drawing.Size(366, 342)
        Me.txtDisplay.TabIndex = 1
        '
        'lblPort
        '
        Me.lblPort.AutoSize = True
        Me.lblPort.Location = New System.Drawing.Point(381, 15)
        Me.lblPort.Name = "lblPort"
        Me.lblPort.Size = New System.Drawing.Size(29, 13)
        Me.lblPort.TabIndex = 3
        Me.lblPort.Text = "Port:"
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(384, 39)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(86, 23)
        Me.btnStart.TabIndex = 4
        Me.btnStart.Text = "Start"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'numPort
        '
        Me.numPort.Location = New System.Drawing.Point(413, 13)
        Me.numPort.Maximum = New Decimal(New Integer() {6000, 0, 0, 0})
        Me.numPort.Minimum = New Decimal(New Integer() {2000, 0, 0, 0})
        Me.numPort.Name = "numPort"
        Me.numPort.Size = New System.Drawing.Size(57, 20)
        Me.numPort.TabIndex = 5
        Me.numPort.Value = New Decimal(New Integer() {2000, 0, 0, 0})
        '
        'lblHostIPAddr
        '
        Me.lblHostIPAddr.AutoSize = True
        Me.lblHostIPAddr.Location = New System.Drawing.Point(9, 15)
        Me.lblHostIPAddr.Name = "lblHostIPAddr"
        Me.lblHostIPAddr.Size = New System.Drawing.Size(99, 13)
        Me.lblHostIPAddr.TabIndex = 6
        Me.lblHostIPAddr.Text = "Your IP Address is: "
        '
        'btnStop
        '
        Me.btnStop.Enabled = False
        Me.btnStop.Location = New System.Drawing.Point(384, 39)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(86, 23)
        Me.btnStop.TabIndex = 7
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        Me.btnStop.Visible = False
        '
        'FrmServer
        '
        Me.ClientSize = New System.Drawing.Size(480, 488)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.lblHostIPAddr)
        Me.Controls.Add(Me.numPort)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.lblPort)
        Me.Controls.Add(Me.txtDisplay)
        Me.Controls.Add(Me.txtInput)
        Me.Name = "FrmServer"
        Me.Text = "Server"
        CType(Me.numPort, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub


End Class