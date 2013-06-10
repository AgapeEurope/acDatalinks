Imports System.Windows.Forms

Public Class NewDatalink
    Private _DataLinkName As String
    Public Property DataLinkName() As String
        Get
            Return _DataLinkName
        End Get
        Set(ByVal value As String)
            _DataLinkName = value
        End Set
    End Property

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If tbName.Text = "" Then
            MsgBox("Please enter a name.", MsgBoxStyle.OkOnly, "Alert")
            Return
        End If
        _DataLinkName = tbName.Text

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
