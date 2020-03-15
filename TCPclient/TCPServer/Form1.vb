Imports System.Net
Imports System.IO
Imports System.Net.Sockets
Public Class Client
    Dim Listning As TcpListener
    Dim Allclient As TcpClient
    Dim clientList As New List(Of ClassforClient)
    Dim pReader As StreamReader
    Dim pClient As ClassforClient
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Listning = New TcpListener(IPAddress.Any, 3818)
        Listning.Start()
        UpdateList("Server Starting", False)
        Listning.BeginAcceptTcpClient(New AsyncCallback(AddressOf AcceptClient), Listning)
    End Sub
    ' create a delegate
    Delegate Sub _cUpdate(ByVal str As String, ByVal relay As Boolean)
    Sub UpdateList(ByVal str As String, ByVal relay As Boolean)
        On Error Resume Next
        If InvokeRequired Then
            Invoke(New _cUpdate(AddressOf UpdateList), str, relay)
        Else
            TextBox1.AppendText(str & vbNewLine)
            ' if relay we will send a string
            If relay Then send(str)
        End If
    End Sub
    Sub send(ByVal str As String)
        For x As Integer = 0 To clientList.Count - 1
            Try
                clientList(x).Send(str)
            Catch ex As Exception
                clientList.RemoveAt(x)
            End Try
        Next
    End Sub
    Sub AcceptClient(ByVal ar As IAsyncResult)
        pClient = New ClassforClient(Listning.EndAcceptTcpClient(ar))
        AddHandler (pClient.getMessage), AddressOf MessageReceived
        AddHandler (pClient.clientLogout), AddressOf ClientExited
        clientList.Add(pClient)
        UpdateList("New Client Joined!", True)
        Listning.BeginAcceptTcpClient(New AsyncCallback(AddressOf AcceptClient), Listning)
    End Sub
    Sub MessageReceived(ByVal str As String)
        UpdateList(str, True)
    End Sub
    Sub ClientExited(ByVal client As ClassforClient)
        clientList.Remove(client)
        UpdateList("Client Exited!", True)
    End Sub
    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            UpdateList("Server says : " & TextBox2.Text, True)
            TextBox2.Clear()
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        UpdateList("Server says : " & TextBox2.Text, True)
        TextBox2.Clear()
    End Sub
End Class
