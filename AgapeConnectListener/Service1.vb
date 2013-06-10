Imports System.Threading
Imports System.Linq
Imports SolomonInterface
Public Class ACListener
    Private m_oPollingThread As New Thread(New System.Threading.ThreadStart(AddressOf PollProcess))
    Private Active As Boolean = False
    Private mustStop As Boolean = False
    Private pollDelayInSeconds As Integer = 300
    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        EventLog1.WriteEntry("ACListener Started")
       
        mustStop = False
        Active = False
        m_oPollingThread.Start()


    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        mustStop = True
        For i As Integer = 1 To 6
            If Active = False Then
                Exit For
            End If
            System.Threading.Thread.Sleep(5000) 'wait 5 seconds
        Next
        m_oPollingThread.Abort()
        EventLog1.WriteEntry("AgapeConnect Stopped")
    End Sub

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
            Return tntVersion
        Catch ex As Exception
            Return ""
        End Try
    End Function


    Private Sub PollProcess()
        ' Loops, until killed by OnStop.
        EventLog1.WriteEntry("AgapeConnect service polling thread started.")



        Do
            Try
                Active = True

                Using d As New ACListenerDataContext()


                    Dim activeDatalinks = From c In d.Datalinks Where c.Active
                    'EventLog1.WriteEntry("Connected to: " & d.Connection.ConnectionString)
                    'EventLog1.WriteEntry("Found " & activeDatalinks.Count & " datalinks to process")
                    If d.Settings.Count > 0 Then
                        d.Settings.First.tntVersion = GetTntVersion(d.Settings.First.tntPath)
                        pollDelayInSeconds = d.Settings.First.PollDelayInSeconds
                        d.SubmitChanges()
                    End If



                    For Each dataLink In activeDatalinks

                        Try



                            EventLog1.WriteEntry("Running Datalink " & dataLink.Name)
                            If mustStop Then
                                Active = False
                                Exit For
                            End If
                            Dim Status As String = ""
                            Dim poll As New PollWebsite(dataLink.DatalinkId, d.Connection.ConnectionString, Status)
                            If Status = "OK" Then
                                Try
                                    Dim rtn = poll.Sync()
                                    
                                Catch ex As Exception
                                    dataLink.Error = True
                                    dataLink.ErrorMessage &= "Error running datalink " & dataLink.Name & ": " & ex.Message
                                    EventLog1.WriteEntry("Error running datalink " & dataLink.Name & ": " & ex.Message)
                                End Try

                            ElseIf Status <> "Off" Then
                                dataLink.Error = True
                                dataLink.ErrorMessage &= Status & "; "
                                EventLog1.WriteEntry("Error establishing datalink " & dataLink.Name & ": " & Status)
                            End If
                            poll.CleanUp()
                            d.SubmitChanges()


                        Catch ex As Exception
                            dataLink.Error = True
                            dataLink.ErrorMessage &= ex.Message & "; "
                            d.SubmitChanges()
                            EventLog1.WriteEntry("Error running datalink" & dataLink.Name & ": " & ex.Message)
                        End Try

                    Next
                    activeDatalinks = Nothing

                End Using
                Active = False
                ' Wait...
            Catch ex As Exception
                EventLog1.WriteEntry("Error retrieving active datalinks: " & ex.Message)
            End Try
            System.Threading.Thread.Sleep(pollDelayInSeconds * 1000) ' milliseconds

        Loop

    End Sub
   


    
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        If Not System.Diagnostics.EventLog.SourceExists("AgapeConnectSource") Then
            System.Diagnostics.EventLog.CreateEventSource("AgapeConnectSource",
            "ListenerLog")
        End If
        EventLog1.Source = "AgapeConnectSource"
        EventLog1.Log = "ListenerLog"
        ' Add any initialization after the InitializeComponent() call.

    End Sub

   
End Class
