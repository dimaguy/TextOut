Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Module encryption
    Public Function Encrypt(ByVal text As String, ByVal key As String) As String
        Try
            Dim crp As New TripleDESCryptoServiceProvider
            Dim uEncode As New UnicodeEncoding
            Dim bytPlainText() As Byte = uEncode.GetBytes(text)
            Dim stmCipherText As New MemoryStream
            Dim slt() As Byte = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12}
            Dim pdb As New Rfc2898DeriveBytes(key, slt)
            Dim bytDerivedKey() As Byte = pdb.GetBytes(24)

            crp.Key = bytDerivedKey
            crp.IV = pdb.GetBytes(8)

            Dim csEncrypted As New CryptoStream(stmCipherText, crp.CreateEncryptor(), CryptoStreamMode.Write)

            csEncrypted.Write(bytPlainText, 0, bytPlainText.Length)
            csEncrypted.FlushFinalBlock()
            Return Convert.ToBase64String(stmCipherText.ToArray())
        Catch ex As Exception
            Throw
        End Try
    End Function
    Function Decrypt(ByVal text As String, ByVal key As String) As String

        Dim crp As TripleDESCryptoServiceProvider
        Try
            crp = New TripleDESCryptoServiceProvider
            Dim uEncode As New UnicodeEncoding
            Dim bytCipherText() As Byte = Convert.FromBase64String(text)
            Dim stmPlainText As New MemoryStream
            Dim stmCipherText As New MemoryStream(bytCipherText)
            Dim slt() As Byte = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12}
            Dim pdb As New Rfc2898DeriveBytes(key, slt)
            Dim bytDerivedKey() As Byte = pdb.GetBytes(24)
            crp.Key = bytDerivedKey
            crp.IV = pdb.GetBytes(8)

            Dim csDecrypted As New CryptoStream(stmCipherText, crp.CreateDecryptor(), CryptoStreamMode.Read)
            Dim sw As New StreamWriter(stmPlainText)
            Dim sr As New StreamReader(csDecrypted)
            sw.Write(sr.ReadToEnd)
            sw.Flush()
            csDecrypted.Clear()
            crp.Clear()
            Return uEncode.GetString(stmPlainText.ToArray())
        Catch ex As Exception
            Throw
        End Try

    End Function

    Function GenerateRandomString(ByRef len As Integer, ByRef upper As Boolean) As String
        Dim rand As New Random()
        Dim allowableChars() As Char = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()
        Dim final As String = String.Empty
        For i As Integer = 0 To len - 1
            final += allowableChars(rand.Next(allowableChars.Length - 1))
        Next

        Return IIf(upper, final.ToUpper(), final)
    End Function

End Module
