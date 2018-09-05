Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not My.Settings.Username = "" Then
            TextBox3.Text = My.Settings.Username
        End If
        If Not My.Settings.Remoteip = "" Then
            TextBox2.Text = My.Settings.Remoteip
        End If
    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Chat.destinyaddress = TextBox2.Text
        My.Settings.Remoteip = TextBox2.Text
        My.Settings.Username = TextBox3.Text
        My.Settings.Save()
        My.Settings.Reload()
        Chat.Show()
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub
End Class
