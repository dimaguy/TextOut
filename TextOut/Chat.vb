Imports System.IO
Imports System.Net
Imports System.Net.Sockets


Public Class Chat
    Public Port As Integer = 65534
    Public destinyaddress As String
    Protected Property Active As Boolean
    Dim listener As New TcpListener(IPAddress.Any, Port)
    Dim listener1 As New TcpListener(IPAddress.Any, Port + 1) 'hidden communication to transfer non-chat data
    Dim client As New TcpClient
    Dim client1 As New TcpClient
    Dim userinfoclient As Integer = 0
    Dim userinfoserver As Integer = 0
    Dim enckeyserver As String
    Dim enckeyclient As String

    Private Sub Chat_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            listener.Start()
            listener1.Start()
        Catch SocketException As Exception
            MsgBox("Error when starting server", MsgBoxStyle.Critical, "Error - TextOut")
        End Try
        enckeyclient = GenerateRandomString(5, False) 'this generates a encryption key(5 alphanumeric digits without upper letters) that will be used during all the communications made to this client
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        client = New TcpClient(destinyaddress, Port)
        Dim sw As New StreamWriter(client.GetStream())
        If userinfoclient < 1 Then
            sw.Write(My.Settings.Username + " " + "Connected" + vbNewLine)
        End If
        sw.Write(vbNewLine + My.Settings.Username + " Disconnected")
        sw.Flush()
        client.Close()

        My.Settings.Save()
        My.Settings.Reload()
        Application.Restart()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim message As String = ""

        If listener.Pending = True Then
            client = listener.AcceptTcpClient
            client1 = listener1.AcceptTcpClient
            Dim sr As New StreamReader(client.GetStream())
            Dim sr1 As New StreamReader(client1.GetStream())

            While sr.Peek > -1
                enckeyserver = Convert.ToChar(sr1.Read()).ToString
                message &= Decrypt(Convert.ToChar(sr.Read()).ToString, enckeyserver)
            End While

            RichTextBox1.AppendText(message + vbNewLine)

        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        client = New TcpClient(destinyaddress, Port)
        Dim sw As New StreamWriter(client.GetStream())
        If userinfoclient < 1 Then 'on the first message sent the client will transfer the key and will announce its presence
            If destinyaddress IsNot "127.0.0.1" Then
                client1 = New TcpClient(destinyaddress, Port + 1)
                Dim sw1 As New StreamWriter(client.GetStream())
                sw1.Write(enckeyclient)
                sw1.Flush()
            End If
            sw.Write(My.Settings.Username + " " + "Connected" + vbNewLine)
            userinfoclient = userinfoclient + 1
        End If
        sw.Write("(" + My.Settings.Username + ") " + Encrypt(TextBox1.Text, enckeyclient) + vbNewLine)
        sw.Flush()
        'RichTextBox1.AppendText("(" + My.Settings.Username + ") " + Encrypt(TextBox1.Text, enckeyclient) + vbNewLine) this code is server-side only and will be removed soon
        TextBox1.Clear()
    End Sub

    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged
        Beep()
    End Sub
End Class