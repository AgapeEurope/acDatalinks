Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel
Imports Microsoft.Data.ConnectionUI
Imports System.Data.SqlClient
Public Class SQLConnectionDialog
    Private cp As SqlFileConnectionProperties
    Private uic As SqlConnectionUIControl

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        cp = New Microsoft.Data.ConnectionUI.SqlFileConnectionProperties
        cp("User Instance") = False
        cp("Initial Catalog") = "acDatalinks"
        cp("Persist Security Info") = True
        cp("Connect Timeout") = "20"

        uic = New Microsoft.Data.ConnectionUI.SqlConnectionUIControl


        uic.Initialize(cp)
        uic.Controls(0).Visible = False

        Me.BringToFront()
    End Sub

    'Allows the user to change the title of the dialog
    Public Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
        End Set
    End Property

    'Pass the original connection string or get the 
    'resulting connection string
    Public Property ConnectionString() As String
        Get
            Return cp.ConnectionStringBuilder.ConnectionString
        End Get
        Set(ByVal value As String)
            cp.ConnectionStringBuilder.ConnectionString = value
        End Set
    End Property


    Private Sub OK_Button_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles OK_Button.Click
       

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Dialog1_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Padding = New Padding(5)
        'Dim adv As Button = New Button
        '   Dim Tst As Button = New Button

        'Size the form and place the uic, Test connection button, 
        'and advanced button
        uic.LoadProperties()
        uic.Dock = DockStyle.None
        uic.Parent = Me
        uic.Location = New Point(5, 137)
       
        Me.MinimumSize = Me.Size
        
    End Sub

    Private Sub Advanced_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdvanced.Click
        'Set up a form to display the advanced connection properties
        Dim frm As Form = New Form
        Dim pg As PropertyGrid = New PropertyGrid
        pg.SelectedObject = cp
        pg.Dock = DockStyle.Fill
        pg.Parent = frm
        frm.ShowDialog()
    End Sub

    'Private Sub Test_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTestConnection.Click
    '    'Test the connection
    '    Dim conn As New SqlConnection()

    '    conn.ConnectionString = cp.ConnectionStringBuilder.ConnectionString
    '    Try
    '        conn.Open()


    '        MsgBox("Test Connection Succeeded.", MsgBoxStyle.Exclamation)
    '    Catch ex As Exception
    '        MsgBox("Test Connection Failed.", MsgBoxStyle.Critical)
    '    Finally
    '        Try
    '            conn.Close()
    '        Catch ex As Exception

    '        End Try
    '    End Try

    'End Sub

    Private Sub SQLConnectionDialog_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown



    End Sub

    
End Class

