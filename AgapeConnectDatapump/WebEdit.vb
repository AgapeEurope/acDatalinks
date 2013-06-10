Imports System.Security.Cryptography
Imports SolomonInterface
Public Class WebEdit
    Private _urlAgapeConnect As String = ""
    Public Property urlAgapeConnect() As String
        Get
            Return _urlAgapeConnect
        End Get
        Set(ByVal value As String)
            _urlAgapeConnect = value
        End Set
    End Property

    Private _pollDelay As Integer = 300001
    Public Property pollDelay() As Integer
        Get
            Return _pollDelay
        End Get
        Set(ByVal value As Integer)
            _pollDelay = value
        End Set
    End Property
    Private _password As String
    Public Property password() As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
        End Set
    End Property
    Private Shared s_aditionalEntropy As Byte() = {9, 3, 2, 2, 3}
    Private Sub btnNext_Click(sender As System.Object, e As System.EventArgs) Handles btnNext.Click
        Dim val = CheckURL(TextBox1.Text & "/DesktopModules/AgapeConnect/StaffAdmin/DatatSync.asmx")
        If val = True Then
            _urlAgapeConnect = TextBox1.Text & "/DesktopModules/AgapeConnect/StaffAdmin/DatatSync.asmx"
            Dim encoding As New System.Text.UTF8Encoding()

            _password = AgapeEncryption.AgapeEncrypt.Encrypt(tbPassword.Text)

         

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()

        Else
            lblStatus.Text = "Could not find webservice at specified website. Please check the url and try again"
            lblStatus.ForeColor = Drawing.Color.Red
            lblStatus.Visible = True
        End If
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        lblStatus.Visible = False
        Me.Refresh()

        Dim val = CheckURL(TextBox1.Text & "/DesktopModules/AgapeConnect/StaffAdmin/DatatSync.asmx")

        If val = True Then
            lblStatus.Text = "Website found!"
            lblStatus.ForeColor = Drawing.Color.Green
            lblStatus.Visible = True
        Else
            lblStatus.Text = "Could not find webservice at specified website. Please check the url/password and try again"
            lblStatus.ForeColor = Drawing.Color.Red
            lblStatus.Visible = True

        End If
    End Sub

    Public Function CheckURL(ByVal HostAddress As String) As Boolean
        CheckURL = False

        Dim d As New dynamicAgapeConnect.DatatSync()
        d.UseDefaultCredentials = True

        d.RequestEncoding = System.Text.Encoding.UTF8

        d.Url = HostAddress


        Try
            Dim test = d.HelloWorld(tbPassword.Text)
            If test Is Nothing Then
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try
        Return True

    End Function


    Public Sub New(ByVal webURL As String, ByVal Pword As String)
        

        ' Dim x = Convert.ToBase64String(ProtectedData.Unprotect(encoding.GetBytes(Pword), s_aditionalEntropy, DataProtectionScope.LocalMachine))

        '_password = x
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.BringToFront()
        TextBox1.Text = webURL.Replace("/DesktopModules/AgapeConnect/StaffAdmin/DatatSync.asmx", "")
        Try
            tbPassword.Text = AgapeEncryption.AgapeEncrypt.Decrypt(Pword)
        Catch ex As Exception
            tbPassword.Text = ""
        End Try

    End Sub
End Class