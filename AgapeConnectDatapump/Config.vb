Imports System.Configuration
Imports System.ServiceProcess
Imports SolomonInterface
Imports System.Threading
Public Class Config

    'Private refreshThread As New Thread(New System.Threading.ThreadStart(AddressOf RefreshForm))

    Structure NameValue
        Public DisplayName As String
        Public PropertyValue As Boolean
    End Structure

   

    Private _baseConnectionString As String = ""

    Private Sub Config_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        '   refreshThread.Abort()

    End Sub

    

    Private Sub Config_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        Using d As New ListenerDataContext



            Me.Text = "Agape Connect - Datalink Manager (Version " & System.Reflection.Assembly.GetExecutingAssembly.GetName().Version.ToString & ")"



            Me.DatalinksTableAdapter.Fill(Me.AcDatalinks.Datalinks)

            GetServiceStatus()
            GetBaseConnection()



            If d.Datalinks.Count = 0 Then
                btnAddNew_Click(Me, Nothing)


            End If
        End Using

        ' refreshThread.Start()
    End Sub



    'Public Sub RefreshForm()
    '    Do
    '        'System.Threading.Thread.Sleep(30000) ' milliseconds
    '        SetData()
    '        GetServiceStatus()
    '    Loop
    'End Sub
    Delegate Sub SetServiceTextCallback([text] As String)
    Private Sub SetServiceText(ByVal [text] As String)

        If lblServiceStatus.InvokeRequired Then
            Dim d As New SetServiceTextCallback(AddressOf SetServiceText)
            Me.Invoke(d, New Object() {[text]})
        Else
            Me.lblServiceStatus.Text = [text]
        End If
    End Sub
    Delegate Sub SetDataCallback()
    Private Sub SetData()

        If DataRepeater1.InvokeRequired Then
            Dim d As New SetDataCallback(AddressOf SetData)
            Me.Invoke(d)
        Else
            Me.DatalinksTableAdapter.Fill(Me.AcDatalinks.Datalinks)
            Me.lblServiceStatus.Text = [Text]
        End If
    End Sub






    Private Sub GetBaseConnection()
        Using d As New ListenerDataContext



            Dim Temp = d.Connection.ConnectionString.Split(";")
            _baseConnectionString = ""
            For Each row In Temp
                If Not row.Contains("Initial Catalog") Then
                    _baseConnectionString &= row & ";"
                End If

            Next
        End Using


    End Sub
    Private Sub GetServiceStatus()
        Dim Service As New ServiceController("ACDatalinks")
        Me.SetServiceText(Service.Status.ToString)
        'lblServiceStatus.Text = Service.Status.ToString
        Service.Dispose()

    End Sub

    Private Sub btnEditDonorwise_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles btnEditDonorwise.LinkClicked
        'Edit Dataserver connection
        Dim OldState As Boolean = False
        Dim dwConnString = CType(DataRepeater1.CurrentItem.Controls.Find("dwCon", True).First, Label).Text


        Using d As New ListenerDataContext


            Dim Did = CType(DataRepeater1.CurrentItem.Controls.Find("lblDatalinkId", True).First, Label).Text
            Dim q = From c In d.Datalinks Where c.DatalinkId = Did
            If q.Count > 0 Then
                Me.Enabled = False
                OldState = q.First.Active
                q.First.Active = False
                d.SubmitChanges()
                Using dsEdit As New SQLConnectionDialog()


                    dsEdit.ConnectionString = dwConnString
                    dsEdit.dbMode = SQLConnectionDialog.DW_MODE


                    dsEdit.Text = "Donorwise - SQL Connection"
                    Dim rslt = dsEdit.ShowDialog()
                    q.First.Active = OldState
                    If rslt = Windows.Forms.DialogResult.OK Then

                        q.First.dwConnectionString = dsEdit.ConnectionString

                        d.SubmitChanges()

                    Else
                        d.SubmitChanges()
                    End If

                End Using
                Me.Enabled = True
                Me.DatalinksTableAdapter.Fill(Me.AcDatalinks.Datalinks)
            End If
        End Using
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        'Edit Solomon Connection String
        Dim OldState As Boolean = False
        Dim solConnString = CType(DataRepeater1.CurrentItem.Controls.Find("solCon", True).First, Label).Text
        Using d As New ListenerDataContext
            Dim Did = CType(DataRepeater1.CurrentItem.Controls.Find("lblDatalinkId", True).First, Label).Text
            Dim q = From c In d.Datalinks Where c.DatalinkId = Did
            If q.Count > 0 Then
                Me.Enabled = False
                OldState = q.First.Active
                q.First.Active = False
                d.SubmitChanges()
                Using dsEdit As New SQLConnectionDialog()
                    dsEdit.ConnectionString = solConnString
                    dsEdit.dbMode = SQLConnectionDialog.SOL_MODE
                    dsEdit.Text = "Dynamics(Solomon) Application Database  - SQL Connection"
                    Dim rslt = dsEdit.ShowDialog()

                    If rslt = Windows.Forms.DialogResult.OK Then

                        q.First.CostCenterCount = 0
                        q.First.AccountCodeCount = 0
                        d.SubmitChanges()

                        q.First.solConnectionString = dsEdit.ConnectionString

                    End If
                    q.First.Active = OldState
                    d.SubmitChanges()


                   
                   


                    Me.Enabled = True
                    Me.DatalinksTableAdapter.Fill(Me.AcDatalinks.Datalinks)
                End Using
            End If
        End Using
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        Dim webURL = CType(DataRepeater1.CurrentItem.Controls.Find("webURL", True).First, Label).Text

        Using d As New ListenerDataContext
            Dim Did = CType(DataRepeater1.CurrentItem.Controls.Find("lblDatalinkId", True).First, Label).Text
            Dim q = From c In d.Datalinks Where c.DatalinkId = Did
            Dim OldState As Boolean = False
            If q.Count > 0 Then
                OldState = q.First.Active
                q.First.Active = False
                d.SubmitChanges()
                Me.Enabled = False
                Dim w = q.First.webURL
                Dim p = q.First.webPassword
                If w = Nothing Then
                    w = ""
                End If
                If p = Nothing Then
                    p = ""
                End If
                Using t As New WebEdit(w, p)


                    Dim rslt = t.ShowDialog()

                    If rslt = DialogResult.OK Then

                        q.First.webURL = t.urlAgapeConnect
                        q.First.webPassword = t.password

                        d.SubmitChanges()
                        ' CType(DataRepeater1.CurrentItem.Controls.Find("dsCon", True).First, Label).Text = dsEdit.ConnectionString
                        Me.DatalinksTableAdapter.Fill(Me.AcDatalinks.Datalinks)


                    End If
                End Using
                q.First.Active = OldState
                d.SubmitChanges()
                Me.Enabled = True
            End If
        End Using
    End Sub

    Private Sub btnSync_Click(sender As System.Object, e As System.EventArgs) Handles btnSync.Click
        Try
            

            ' Dim webURL = CType(DataRepeater1.CurrentItem.Controls.Find("webURL", True).First, Label).Text
            'Dim solConnString = CType(DataRepeater1.CurrentItem.Controls.Find("solCon", True).First, Label).Text
            '  Dim dsConnString = CType(DataRepeater1.CurrentItem.Controls.Find("dsCon", True).First, Label).Text


            Using d As New ListenerDataContext

                Dim Did = CType(DataRepeater1.CurrentItem.Controls.Find("lblDatalinkId", True).First, Label).Text
                Dim Status As String = "Force"

                Dim p As New PollWebsite(CInt(Did), d.Connection.ConnectionString, Status)
                If Status = "OK" Then


                    Dim resp = p.Sync()

                    Me.DatalinksTableAdapter.Fill(Me.AcDatalinks.Datalinks)


                    MsgBox(resp.Message, IIf(resp.Status = "Error", MsgBoxStyle.Critical, MsgBoxStyle.Information))
                Else
                    MsgBox("There was a problem running the datalink. More info: " & Status, MsgBoxStyle.Critical)
                    Me.DatalinksTableAdapter.Fill(Me.AcDatalinks.Datalinks)
                    Return
                End If
                p.CleanUp()
            End Using
        Catch ex As Exception
            MsgBox("There was a problem running the datalink. More info: " & ex.Message, MsgBoxStyle.Critical)

        Finally

        End Try



    End Sub

    'Private Sub btnACEdit_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles btnACEdit.LinkClicked
    '    Dim x = ConfigurationManager.ConnectionStrings
    '    For Each row As ConnectionStringSettings In x
    '        MsgBox(row.ConnectionString)
    '    Next

    '    Dim ACCon = ConfigurationManager.ConnectionStrings("AgapeConnectDatapump.My.MySettings.AgapeConnectWebUsersConnectionString").ConnectionString
    '    Dim dsEdit As New SQLConnectionDialog()
    '    dsEdit.ConnectionString = ACCon
    '    Dim rslt = dsEdit.ShowDialog()

    '    If rslt = Windows.Forms.DialogResult.OK Then
    '        ConfigurationManager.ConnectionStrings("AgapeConnectDatapump.My.MySettings.AgapeConnectWebUsersConnectionString").ConnectionString = dsEdit.ConnectionString
    '       Me.DatalinksTableAdapter.Fill(Me.AcDatalinks.Datalinks)


    '    End If
    ' End Sub

    'Private Sub DataRepeater1_DataSourceChanged(sender As System.Object, e As System.EventArgs) Handles DataRepeater1.
    '    If DataRepeater1.ItemCount > 0 Then



    '    For i As Integer = 0 To DataRepeater1.ItemCount - 1
    '        DataRepeater1.CurrentItemIndex = i
    '        Try
    '            Dim enabled As Boolean = CType(DataRepeater1.CurrentItem.Controls.Find("lblStatus", True).First, Label).Tag
    '            CType(DataRepeater1.CurrentItem.Controls.Find("lblStatus", True).First, Label).Text = IIf(enabled, "Active", "Offline")

    '        Catch ex As Exception

    '        End Try

    '    Next

    '    End If
    'End Sub
   
    Private Sub btnEnableDisable_Click(sender As System.Object, e As System.EventArgs) Handles btnEnableDisable.Click
        Dim but As Button = CType(sender, Button)
        
        Using d As New ListenerDataContext


            Dim q = From c In d.Datalinks Where c.DatalinkId = CInt(but.Tag)

            If q.Count > 0 Then
                If q.First.solConnectionString.Length > 0 And q.First.webURL.Length > 0 And q.First.webPassword.Length > 0 Then


                    q.First.Active = Not q.First.Active
                    d.SubmitChanges()
                    Me.DatalinksTableAdapter.Fill(Me.AcDatalinks.Datalinks)
                Else
                    MsgBox("This Datalink could not be activated because it has not yet been configured. Click the edit buttons to configure the links to Dataserver, Solomon(Dynamics) and your website.", MsgBoxStyle.Critical)

                End If
            End If

        End Using
    End Sub

    Private Sub btnRefresh_Click(sender As System.Object, e As System.EventArgs) Handles btnRefresh.Click

        Me.DatalinksTableAdapter.Fill(Me.AcDatalinks.Datalinks)
        GetServiceStatus()
       

    End Sub

    Private Sub btnAddNew_Click(sender As System.Object, e As System.EventArgs) Handles btnAddNew.Click
        'Get New Name
        Using nd As New NewDatalink()

            nd.DataLinkName = "New Datalink"

            Dim rslt = nd.ShowDialog()
            If rslt = Windows.Forms.DialogResult.Cancel Then
                Return
            End If

            Using d As New ListenerDataContext


                Dim insert As New Listener.Datalink
                insert.Active = False
                insert.Error = False
                insert.dsConnectionString = ""
                insert.solConnectionString = insert.dsConnectionString
                insert.AccountCodeCount = 0
                insert.LastSync = New Date(2001, 1, 1)
                insert.ErrorMessage = ""
                insert.CostCenterCount = 0

                insert.Name = nd.DataLinkName
                d.Datalinks.InsertOnSubmit(insert)
                d.SubmitChanges()
            End Using
            Me.DatalinksTableAdapter.Fill(Me.AcDatalinks.Datalinks)
        End Using
    End Sub

    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click
        Using d As New ListenerDataContext


            Dim Did = CType(DataRepeater1.CurrentItem.Controls.Find("lblDatalinkId", True).First, Label).Text
            Dim q = From c In d.Datalinks Where c.DatalinkId = Did

            d.Datalinks.DeleteAllOnSubmit(q)
            d.SubmitChanges()
        End Using
        Me.DatalinksTableAdapter.Fill(Me.AcDatalinks.Datalinks)
    End Sub

    'Private Sub btnStartStop_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles btnStartStop.LinkClicked

    '    Dim impersonationContext As System.Security.Principal.WindowsImpersonationContext

    '    Dim thisUser As New System.Security.Principal.WindowsIdentity(System.Security.Principal.WindowsIdentity.GetCurrent().Token)

    '    impersonationContext = thisUser.Impersonate()
    '    Dim Service As New ServiceController("ACListener")
    '    If btnStartStop.Text = "Start" Then
    '        If Service.Status <> ServiceControllerStatus.Running And Service.Status <> ServiceControllerStatus.StartPending Then
    '            Service.Start()
    '            lblServiceStatus.Text = "Starting"
    '            Service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30))
    '            Dim theStatus = Service.Status
    '            If theStatus = ServiceControllerStatus.Running Then
    '                GetServiceStatus()
    '            Else
    '                lblServiceStatus.Text = "Did Not Start"
    '            End If
    '        Else
    '            GetServiceStatus()
    '        End If
    '    Else
    '        If Service.Status <> ServiceControllerStatus.Running And Service.Status <> ServiceControllerStatus.StartPending Then
    '            GetServiceStatus()
    '        Else
    '            Service.Stop()
    '            lblServiceStatus.Text = "Stopping"
    '            Service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30))
    '            Dim theStatus = Service.Status
    '            If theStatus = ServiceControllerStatus.Stopped Then
    '                GetServiceStatus()
    '            Else
    '                lblServiceStatus.Text = "Did Not Stop"
    '            End If
    '        End If
    '    End If
    '    impersonationContext.Undo()
    'End Sub

    Private Sub PictureBox2_Click(sender As System.Object, e As System.EventArgs)
        Using d As New ListenerDataContext


            Dim Did = CType(DataRepeater1.CurrentItem.Controls.Find("lblDatalinkId", True).First, Label).Text
            Dim q = From c In d.Datalinks Where c.DatalinkId = Did

            If q.Count > 0 Then
                MsgBox(q.First.ErrorMessage, MsgBoxStyle.Exclamation, "Datalink Error")

            End If

        End Using
    End Sub



    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

        Using s As New Settings()
            Using d As New ListenerDataContext

                If d.Settings.Count > 0 Then
                    s.tbACDatalinksVersion.Text = System.Reflection.Assembly.GetExecutingAssembly.GetName().Version.ToString
                   
                    s.tbPollDelay.Value = d.Settings.First.PollDelayInSeconds

                End If

            End Using
            Dim rtn = s.ShowDialog()
            If rtn = Windows.Forms.DialogResult.OK Then
                Using dnew As New ListenerDataContext
                    If dnew.Settings.Count > 0 Then
                      
                        dnew.Settings.First.PollDelayInSeconds = s.tbPollDelay.Value

                    Else
                        Dim insert As New Listener.Setting
                     
                        insert.PollDelayInSeconds = s.tbPollDelay.Value
                    End If
                    dnew.SubmitChanges()

                End Using
            End If

        End Using

    End Sub

    
End Class