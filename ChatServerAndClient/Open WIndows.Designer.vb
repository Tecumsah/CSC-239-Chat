<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Open_WIndows
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnClient = New System.Windows.Forms.Button()
        Me.btnServer = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnClient
        '
        Me.btnClient.AutoSize = True
        Me.btnClient.Location = New System.Drawing.Point(191, 114)
        Me.btnClient.Name = "btnClient"
        Me.btnClient.Size = New System.Drawing.Size(77, 35)
        Me.btnClient.TabIndex = 0
        Me.btnClient.Text = "Client"
        Me.btnClient.UseVisualStyleBackColor = True
        '
        'btnServer
        '
        Me.btnServer.AutoSize = True
        Me.btnServer.Location = New System.Drawing.Point(330, 114)
        Me.btnServer.Name = "btnServer"
        Me.btnServer.Size = New System.Drawing.Size(96, 35)
        Me.btnServer.TabIndex = 1
        Me.btnServer.Text = "Server"
        Me.btnServer.UseVisualStyleBackColor = True
        '
        'Open_WIndows
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(670, 465)
        Me.Controls.Add(Me.btnServer)
        Me.Controls.Add(Me.btnClient)
        Me.Name = "Open_WIndows"
        Me.Text = "Open_WIndows"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnClient As Button
    Friend WithEvents btnServer As Button
End Class
