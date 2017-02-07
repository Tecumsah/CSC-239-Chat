Public Class Open_WIndows
    Private Sub btnClient_Click(sender As Object, e As EventArgs) Handles btnClient.Click
        Dim clientForm As frmClient
        clientForm = New frmClient()
        clientForm.Show()
        clientForm = Nothing
    End Sub

    Private Sub btnServer_Click(sender As Object, e As EventArgs) Handles btnServer.Click
        Dim serverForm As FrmServer
        serverForm = New FrmServer()
        serverForm.Show()
        serverForm = Nothing
    End Sub
End Class