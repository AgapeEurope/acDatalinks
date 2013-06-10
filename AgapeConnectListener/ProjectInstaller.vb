Imports System.ComponentModel
Imports System.Configuration.Install
Imports System.ServiceProcess
Imports System
Imports System.Collections.Generic

Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Configuration

Imports System.Xml

Public Class ProjectInstaller

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()
        'ServiceProcessInstaller1.Username = My.User.Name


        'Select Case Context.Parameters("ac")
        '    Case 1
        '        ServiceProcessInstaller1.Account = ServiceAccount.User
        '        ServiceProcessInstaller1.Username = My.User.Name
        '    Case 2
        '        ServiceProcessInstaller1.Account = ServiceAccount.LocalService
        '        ServiceProcessInstaller1.Username = Nothing
        '    Case 3
        '        ServiceProcessInstaller1.Account = ServiceAccount.LocalSystem
        '        ServiceProcessInstaller1.Username = Nothing
        '    Case 4
        '        ServiceProcessInstaller1.Account = ServiceAccount.NetworkService
        '        ServiceProcessInstaller1.Username = Nothing
        'End Select

        'Add initialization code after the call to InitializeComponent

    End Sub
    

    Public Overrides Sub Commit(savedState As System.Collections.IDictionary)
        MyBase.Commit(savedState)

        Try
            Dim sc As New ServiceController()
            sc.ServiceName = ServiceInstaller1.ServiceName
           
            If sc.Status = ServiceControllerStatus.Stopped Then
                sc.Start()
            End If


        Catch ex As Exception


        End Try
    End Sub




    Private Sub ServiceInstaller1_Committed(sender As System.Object, e As System.Configuration.Install.InstallEventArgs) Handles ServiceInstaller1.Committed
        Dim sc As New ServiceController()
        sc.ServiceName = ServiceInstaller1.ServiceName

        If sc.Status = ServiceControllerStatus.Stopped Then
            Try
                ' Start the service, and wait until its status is "Running".
                sc.Start()
                sc.WaitForStatus(ServiceControllerStatus.Running)

                ' TODO: log status of service here: sc.Status
            Catch ex As Exception
                ' TODO: log an error here: "Could not start service: ex.Message"

            End Try
        End If
    End Sub


   
   
    
    'Private Sub ServiceInstaller1_BeforeInstall(sender As System.Object, e As System.Configuration.Install.InstallEventArgs) Handles ServiceProcessInstaller1.BeforeInstall



    '    Dim sa As New ServiceAccount
    '    sa.ddlType.SelectedIndex = 0
    '    sa.tbDomain.Text = My.Computer.Name


    '    Dim rslt = sa.ShowDialog()

    '    If rslt = DialogResult.OK Then
    '        Select Case sa.ddlType.SelectedIndex
    '            Case 0
    '                ServiceProcessInstaller1.Account = ServiceProcess.ServiceAccount.User
    '                ServiceProcessInstaller1.Username = sa.tbDomain.Text & "\" & sa.tbUsername.Text
    '                ServiceProcessInstaller1.Password = sa.tbPassword.Text
    '            Case 1
    '                ServiceProcessInstaller1.Account = ServiceProcess.ServiceAccount.LocalService
    '            Case 2
    '                ServiceProcessInstaller1.Account = ServiceProcess.ServiceAccount.LocalSystem
    '            Case 3
    '                ServiceProcessInstaller1.Account = ServiceProcess.ServiceAccount.NetworkService

    '        End Select




    '    Else
    '        Error 18
    '    End If



    'End Sub
End Class
