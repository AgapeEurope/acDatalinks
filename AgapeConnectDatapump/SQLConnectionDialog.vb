Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Data.Linq
Imports Microsoft.Data.ConnectionUI

Imports System.Data.SqlClient

Public Class SQLConnectionDialog
    Private cp As SqlConnectionProperties
    Private uic As SqlConnectionUIControl

    Public Const TNT_MODE = "TNT"
    Public Const SOL_MODE = "SOL"

    Private _dbMode As String
    Public Property dbMode() As String
        Get
            Return _dbMode
        End Get
        Set(ByVal value As String)
            _dbMode = value
        End Set
    End Property




   


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        cp = New Microsoft.Data.ConnectionUI.SqlFileConnectionProperties
        cp("User Instance") = False

        uic = New Microsoft.Data.ConnectionUI.SqlConnectionUIControl
        uic.Initialize(cp)
        ' uic.Controls(0).Visible = False

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
        If Test() Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
       
        End If
        
        
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Dialog1_Load(ByVal sender As System.Object, _
        ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Padding = New Padding(5)
        Dim adv As Button = New Button
        Dim Tst As Button = New Button

        'Size the form and place the uic, Test connection button, 
        'and advanced button
        uic.LoadProperties()
        uic.Dock = DockStyle.Top
        uic.Parent = Me
        Me.ClientSize = Size.Add(uic.MinimumSize, New Size(10, _
            (adv.Height + 25)))
        Me.MinimumSize = Me.Size
        With adv
            .Text = "Advanced"
            .Dock = DockStyle.None
            .Location = New Point((uic.Width - .Width), (uic.Bottom + 10))
            .Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            AddHandler .Click, AddressOf Me.Advanced_Click
            .Parent = Me
        End With

        With Tst
            .Text = "Test Connection"
            .Width = 100
            .Dock = DockStyle.None
            .Location = _
                New Point((uic.Width - .Width) - adv.Width - 10, (uic.Bottom + 10))
            .Anchor = (AnchorStyles.Right Or AnchorStyles.Top)
            AddHandler .Click, AddressOf Me.Test_Click
            .Parent = Me
        End With
    End Sub

    Private Sub Advanced_Click(ByVal sender As Object, ByVal e As EventArgs)
        'Set up a form to display the advanced connection properties
        Dim frm As Form = New Form
        Dim pg As PropertyGrid = New PropertyGrid
        pg.SelectedObject = cp
        pg.Dock = DockStyle.Fill
        pg.Parent = frm
        frm.ShowDialog()
    End Sub

    Private Function Test() As Boolean
        Using conn As New SqlConnection()



            conn.ConnectionString = cp.ConnectionStringBuilder.ConnectionString
            Try
                conn.Open()

                Dim tables = From c In conn.GetSchema("Tables").Rows
                Dim IsTnT = False
                Dim IsSol = False
                For Each row As DataRow In tables
                    If row(2) = "DataPumpScannerSource" Then
                        IsTnT = True
                    ElseIf row(2) = "SubAcct" Then
                        IsSol = True
                    End If
                Next

                If (Not IsTnT) And dbMode = TNT_MODE Then
                    MsgBox("The connection succeeded but this Database does not appear to be a TnT Dataserver database. Please check your settings and try again.", MsgBoxStyle.Critical)
                ElseIf (Not IsSol) And dbMode = SOL_MODE Then
                    MsgBox("The connection succeeded but this Database does not appear to be a TnT Dataserver database. Please check your settings and try again.", MsgBoxStyle.Critical)
                Else

                    Return True
                End If



            Catch ex As Exception
                MsgBox("Test Connection Failed.", MsgBoxStyle.Critical)
            Finally
                Try
                    conn.Close()
                Catch ex As Exception

                End Try
            End Try
            Return False
        End Using
    End Function


    Private Sub Test_Click(ByVal sender As Object, ByVal e As EventArgs)
        'Test the connection
        If Test() Then
            MsgBox("Test Connection Succeeded.", MsgBoxStyle.Exclamation)
        End If

    End Sub
End Class

