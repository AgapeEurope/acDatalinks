Public Class ServiceAccount

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If ddlType.SelectedIndex = 0 Then
            If ValidateActiveDirectoryLogin(tbDomain.Text, tbUsername.Text, tbPassword.Text) Then
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            Else

                MsgBox("Login Failed")
            End If
        Else
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If


    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Abort
        Me.Close()
    End Sub


    Private Function ValidateActiveDirectoryLogin(ByVal Domain As String, ByVal Username As String, ByVal Password As String) As Boolean
        Dim Success As Boolean = False
        Dim Entry As New System.DirectoryServices.DirectoryEntry("LDAP://" & Domain, Username, Password)
        Dim Searcher As New System.DirectoryServices.DirectorySearcher(Entry)
        Searcher.SearchScope = DirectoryServices.SearchScope.OneLevel
        Try
            Dim Results As System.DirectoryServices.SearchResult = Searcher.FindOne
            Success = Not (Results Is Nothing)
        Catch
            Success = False
        End Try
        Return Success
    End Function
    
    Private Sub ComboBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ddlType.SelectedIndexChanged
        If ddlType.SelectedIndex = 0 Then
            tbDomain.Enabled = True
            tbUsername.Enabled = True
            tbPassword.Enabled = True
        Else
            tbDomain.Enabled = False
            tbUsername.Enabled = False
            tbPassword.Enabled = False
        End If
    End Sub

    Private Sub ServiceAccount_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Me.BringToFront()
    End Sub
End Class