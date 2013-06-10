Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Configuration
Imports System.Configuration.Install
Imports System.Xml
Imports System.Data.SqlClient

Imports System.ServiceProcess


Public Class WindowWrapper
    Implements System.Windows.Forms.IWin32Window
    Private _hwnd As IntPtr

    Public Sub New(ByVal _handle As IntPtr)
        _hwnd = _handle
    End Sub
    Public ReadOnly Property Handle As System.IntPtr Implements System.Windows.Forms.IWin32Window.Handle
        Get
            Return _hwnd
        End Get
    End Property
End Class



Public Class InstallerConfig


    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add initialization code after the call to InitializeComponent

    End Sub

    Protected Overrides Sub OnBeforeInstall(savedState As System.Collections.IDictionary)
        MyBase.OnBeforeInstall(savedState)


    End Sub


    Public Overrides Sub Install(ByVal stateSaver As  _
      System.Collections.IDictionary)
        MyBase.Install(stateSaver)

        Try
            Using acl As New SQLConnectionDialog()
                Dim d As ListenerDataContext
                Dim ProcId As Integer = 0
                For Each proc In Process.GetProcesses()
                    If proc.ProcessName.Contains("msiexec") And proc.MainWindowTitle = "ACDatalinks" Then
                        ProcId = proc.Id
                    End If

                Next


                Dim rslt As Windows.Forms.DialogResult
                Dim hwnd As IntPtr = Nothing
                If ProcId <> 0 Then
                    Dim theProc = Process.GetProcessById(ProcId)
                    hwnd = theProc.MainWindowHandle
                    rslt = acl.ShowDialog(New WindowWrapper(hwnd))
                Else

                    rslt = acl.ShowDialog()

                End If




                Dim aclCon As String = ""
                If rslt = DialogResult.OK Then
                    aclCon = acl.ConnectionString
                    '   MsgBox(aclCon)

                    d = New ListenerDataContext(aclCon)
                    If d.DatabaseExists() Then
                        'ask for new Database
                        Using de As New DatabaseExists




                            Dim del As Windows.Forms.DialogResult
                            If hwnd = Nothing Then
                                del = de.ShowDialog()
                            Else
                                del = de.ShowDialog(New WindowWrapper(hwnd))
                            End If


                            If del = DialogResult.OK Then
                                d.DeleteDatabase()
                                d.CreateDatabase()

                            Else
                                'do nothing - keep the existing database
                                'Upgrade Database
                                'Using conn As New SqlConnection()

                                '    Try
                                '        Dim q = d.Settings.First.Spare1

                                '    Catch ex As Exception


                                '        conn.ConnectionString = d.Connection.ConnectionString

                                '        Dim alterSQL As String = "ALTER TABLE Settings ADD Spare1 NVARCHAR(MAX); " & vbNewLine
                                '        alterSQL &= "ALTER TABLE Settings ADD Spare2 NVARCHAR(100); " & vbNewLine
                                '        alterSQL &= "ALTER TABLE Settings ADD Spare3 NVARCHAR(100); " & vbNewLine
                                '        alterSQL &= "ALTER TABLE Settings ADD Spare4 NVARCHAR(100); " & vbNewLine
                                '        alterSQL &= "ALTER TABLE Settings ADD Spare5 NVARCHAR(100); " & vbNewLine

                                '        Dim comm = New SqlCommand(alterSQL, conn)
                                '        Dim sqlDone As Integer = 0
                                '        Try
                                '            conn.Open()
                                '            sqlDone = comm.ExecuteNonQuery()

                                '        Catch ex2 As Exception
                                '            'Database did not get changed.
                                '            MsgBox("There was a problem updating your AgapeConnect database. The only option might be delete the existing database and start over. Restart the installer and select this option, when prompted.", MsgBoxStyle.Critical)
                                '            Error 18



                                '        Finally
                                '            conn.Close()
                                '        End Try

                                '    End Try
                                'End Using


                            End If
                        End Using
                    Else
                        'create database
                        d.CreateDatabase()


                    End If
                Else
                    Error 18

                    Return
                End If

                Dim location = System.Reflection.Assembly.GetExecutingAssembly.Location

                location = location.Replace("ConfigureListener.dll", "AgapeConnectListener.exe")
                Dim conf = ConfigurationManager.OpenExeConfiguration(location)


                Dim tntPath As String = GetTnTPath()

                Dim tntVersion As String = GetTntVersion(tntPath)


                conf.ConnectionStrings.ConnectionStrings("AgapeConnectListener.My.MySettings.acConnectionString").ConnectionString = aclCon
                conf.ConnectionStrings.ConnectionStrings("acConnectionString").ConnectionString = aclCon

                conf.Save()


                Dim location2 = System.Reflection.Assembly.GetExecutingAssembly.Location
                ' MsgBox(location2)
                location2 = location2.Replace("ConfigureListener.dll", "AgapeConnectDatapump.exe")
                Dim conf2 = ConfigurationManager.OpenExeConfiguration(location2)




                conf2.ConnectionStrings.ConnectionStrings("AgapeConnectDatapump.My.MySettings.acConnectionString").ConnectionString = aclCon
                conf2.Save()


                If d.Settings.Count > 0 Then
                    d.Settings.First.tntPath = tntPath
                    d.Settings.First.tntVersion = tntVersion
                    d.Settings.First.PollDelayInSeconds = 300
                Else
                    Dim insert As New Listener.Setting
                    insert.tntPath = tntPath
                    insert.tntVersion = tntVersion
                    insert.PollDelayInSeconds = 300
                    d.Settings.InsertOnSubmit(insert)
                End If
                d.SubmitChanges()

            End Using
        Catch ex As Exception

            ' MsgBox(ex.Message)
            Throw ex
        End Try

    End Sub

    Private Function GetTnTPath() As String
        'Very cautious and thorough route to the tnt registry entry...
        'but the registry is scary, and we have to handle the fact
        'that tnt registry values install in different locations
        'on 32bit and 64bit systems
        Try
            Dim tntPath As String = ""
            Dim Root = My.Computer.Registry.LocalMachine
            If Root.GetSubKeyNames.Contains("SOFTWARE") Then
                Dim Software = Root.OpenSubKey("SOFTWARE")
                Dim tntWare As Microsoft.Win32.RegistryKey
                If Software.GetSubKeyNames.Contains("TntWare") Then
                    tntWare = Software.OpenSubKey("TntWare")
                ElseIf Software.GetSubKeyNames.Contains("Wow6432Node") Then
                    Dim Wow = Software.OpenSubKey("Wow6432Node")
                    If Wow.GetSubKeyNames.Contains("TntWare") Then
                        tntWare = Wow.OpenSubKey("TntWare")
                    Else
                        Return ""
                    End If
                Else
                    Return ""
                End If
                If tntWare.GetSubKeyNames.Contains("TntMPD.DataServer") Then
                    Dim dataserver = tntWare.OpenSubKey("TntMPD.DataServer")
                    If dataserver.GetValueNames.Contains("InstallPath") Then
                        Return CStr(dataserver.GetValue("InstallPath"))
                    End If
                End If
            End If
        Catch ex As Exception
            Return ""
        End Try
        Return ""
    End Function
    Private Function GetTntVersion(ByVal tntPath As String) As String
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
            Return tntPath
        Catch ex As Exception
            Return ""
        End Try
    End Function


    Public Overrides Sub Commit(savedState As System.Collections.IDictionary)
        MyBase.Commit(savedState)
        'Dim location1 = System.Reflection.Assembly.GetExecutingAssembly.Location
        'Dim dir = location1.Substring(0, location1.LastIndexOf("\"))
        'Dim location2 = location1.Replace("ConfigureListener.dll", "AgapeConnectDatapump.exe")

        'System.IO.Directory.SetCurrentDirectory(dir)
        'Process.Start(location2)
    End Sub




End Class
