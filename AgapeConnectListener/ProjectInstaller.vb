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





    Public Overrides Sub Commit(ByVal savedState As System.Collections.IDictionary)
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




    Private Sub ServiceInstaller1_Committed(ByVal sender As System.Object, ByVal e As System.Configuration.Install.InstallEventArgs) Handles ServiceInstaller1.Committed
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





    Private Sub ServiceInstaller1_BeforeInstall(ByVal sender As System.Object, ByVal e As System.Configuration.Install.InstallEventArgs) Handles ServiceProcessInstaller1.BeforeInstall


        Try
            Dim sc As New ServiceController(ServiceInstaller1.ServiceName)


            If sc.Status = ServiceControllerStatus.Running Then
                sc.Stop()
                sc.WaitForStatus(ServiceControllerStatus.Stopped, System.TimeSpan.FromSeconds(30))

            End If
           


        Catch ex As Exception


        End Try

    End Sub

    Private Sub ProjectInstaller_BeforeUninstall(ByVal sender As Object, ByVal e As System.Configuration.Install.InstallEventArgs) Handles Me.BeforeUninstall
        Try
            Dim sc As New ServiceController(ServiceInstaller1.ServiceName)


            If sc.Status = ServiceControllerStatus.Running Then
                sc.Stop()
                sc.WaitForStatus(ServiceControllerStatus.Stopped, System.TimeSpan.FromSeconds(30))

            End If
            Dim sio As New ServiceInstaller()
            sio.Context = Context
            sio.ServiceName = "ACDatalinks"
            sio.Uninstall(Nothing)


        Catch ex As Exception


        End Try
    End Sub
End Class
