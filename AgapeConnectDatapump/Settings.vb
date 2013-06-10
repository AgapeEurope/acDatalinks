Public Class Settings

    Private _ACDatalinkVersion As String
    Public Property ACDatalinkVersion() As String
        Get
            Return _ACDatalinkVersion
        End Get
        Set(ByVal value As String)
            _ACDatalinkVersion = value
        End Set
    End Property
    Private _TntVersion As String
    Public Property TntVersion() As String
        Get
            Return _TntVersion
        End Get
        Set(ByVal value As String)
            _TntVersion = value
        End Set
    End Property
    Private _TntPath As String
    Public Property TntPath() As String
        Get
            Return _TntPath
        End Get
        Set(ByVal value As String)
            _TntPath = value
        End Set
    End Property
    Private _PollDelayInSeconds As Integer
    Public Property PollDelayInSeconds() As Integer
        Get
            Return _PollDelayInSeconds
        End Get
        Set(ByVal value As Integer)
            _PollDelayInSeconds = value
        End Set
    End Property



    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


   
    
   
    Private Sub btnBrowse_Click(sender As System.Object, e As System.EventArgs) Handles btnBrowse.Click
        OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)

        Dim rtn = OpenFileDialog1.ShowDialog()
        If rtn = Windows.Forms.DialogResult.OK Then
            tbTntPath.Text = OpenFileDialog1.FileName.Replace("TntMPD.DataServer.exe", "")
            tbTntVersion.Text = GetTntVersion(tbTntPath.Text)
        End If
    End Sub
    Public Function GetTntVersion(ByVal tntPath As String) As String
        Try
            Dim tntVersion As String = ""
            If tntPath <> "" Then
                If System.IO.File.Exists(tntPath & "TntMPD.DataServer.exe") Then
                    Dim tntVersionInfo = FileVersionInfo.GetVersionInfo(tntPath & "TntMPD.DataServer.exe")
                    If Not tntVersionInfo Is Nothing Then
                        tntVersion = tntVersionInfo.FileVersion
                    End If
                End If
            End If
            Return tntVersion
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Sub btnACDatalinkDownload_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles btnACDatalinkDownload.LinkClicked
        Process.Start("https://help.agapeconnect.me/ACDatalinks/Download/")
    End Sub

    Private Sub btnTntDownload_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles btnTntDownload.LinkClicked
        Process.Start("http://www.tntware.com/tntmpd.dataserver/downloads/")

    End Sub
End Class