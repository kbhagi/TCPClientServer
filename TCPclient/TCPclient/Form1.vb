Imports System.Net
Imports System.IO
Imports System.Net.Sockets
Public Class Form1
    Dim client As Sockets.TcpClient
    Dim sWriter As StreamWriter
    Dim NIckFrefix As Integer = New Random().Next(1111, 9999)
    Sub xLoad() Handles Me.Load
        Me.Text &= " " & NIckFrefix
    End Sub
    Delegate Sub _xUpdate(ByVal str As String)
    Sub xUpdate(ByVal str As String)
        If InvokeRequired Then
            Invoke(New _xUpdate(AddressOf xUpdate), str)
        Else
            TextBox3.AppendText(str & vbNewLine)
        End If
    End Sub

    Sub read(ByVal ar As IAsyncResult)
        Try
            xUpdate(New StreamReader(client.GetStream).ReadLine)
            client.GetStream.BeginRead(New Byte() {0}, 0, 0, AddressOf read, Nothing)

        Catch ex As Exception
            xUpdate("You have disconnecting from server")
            Exit Sub
        End Try
    End Sub
    Private Sub send(ByVal str As String)
        Try
            sWriter = New StreamWriter(client.GetStream)
            sWriter.WriteLine(str)
            sWriter.Flush()
        Catch ex As Exception
            xUpdate("You're not server")
        End Try
    End Sub
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        If Button1.Text = "Connect" Then
            Try
                client = New Sockets.TcpClient(TextBox1.Text, CInt(TextBox2.Text))
                client.GetStream.BeginRead(New Byte() {0}, 0, 0, New AsyncCallback(AddressOf read), Nothing)
                Button1.Text = "Disconnect"
            Catch ex As Exception
                xUpdate("Can't connect to the server!")
            End Try
        Else
            client.Client.Close()
            client = Nothing
            Button1.Text = "Connect"
        End If
    End Sub
    Private Sub TextBox4_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox4.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            send(NIckFrefix & " says : " & TextBox4.Text)
            TextBox4.Clear()
        End If
    End Sub
    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        send(NIckFrefix & " says : " & TextBox4.Text)
        TextBox4.Clear()
    End Sub
End Class
