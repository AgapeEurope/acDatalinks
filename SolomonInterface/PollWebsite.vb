
Imports SolomonInterface.AgapeEncryption
Imports System.Xml
Public Class PollWebsite



#Region "Properties"
    Private _Password As String = ""
    Private WebS As New dynamicAgapeConnect.DatatSync
    Private d As Listener2DataContext
    Private dt As TNTDataContext
    Private ds As SolomonDataContext
    Private dl As New Listener2.Datalink
    Private si As New dynamicAgapeConnect.SetupInfo
#End Region
#Region "Contructors"
    Public Sub New(ByVal DatalinkId As Integer, ByVal ACConnectionString As String, ByRef Status As String)
        'EventLog1.WriteEntry("Checking for new accounts/ cost Centers.")

        Try


            d = New Listener2DataContext(ACConnectionString)
            'MsgBox("T1: " & d.Connection.ConnectionString)
            dl = (From c In d.Datalinks Where c.DatalinkId = DatalinkId).First
            If dl.Active = False And Status <> "Force" Then
                Status = "Off"
                Return
            End If
            If String.IsNullOrEmpty(dl.dsConnectionString) Or String.IsNullOrEmpty(dl.solConnectionString) Or String.IsNullOrEmpty(dl.webURL) Or String.IsNullOrEmpty(dl.webPassword) Or (Not dl.solConnectionString.Contains("Catalog")) Or (Not dl.dsConnectionString.Contains("Catalog")) Then

                Status = "This datalink has not been setup. Please setup the connection to Dataserver, Solomon(Dynamics) and your Website in ACDatalinks Manager"
                dl.Error = True
                dl.ErrorMessage = Status
                d.SubmitChanges()
                Return

            End If



            SetSetupInfo()
            WebS.UseDefaultCredentials = True
            WebS.RequestEncoding = System.Text.Encoding.UTF8
            WebS.Url = dl.webURL

            'CheckForNewAccounts
            dt = New TNTDataContext(dl.dsConnectionString)
            ds = New SolomonDataContext(dl.solConnectionString)
            _Password = AgapeEncrypt.Decrypt(dl.webPassword)
            Status = "OK"
        Catch ex As Exception
            Status = ex.Message
        End Try
    End Sub

#End Region

#Region "Public Functions"
    Public Sub CleanUp()
        ds.Dispose()
        dt.Dispose()
        d.Dispose()
        WebS.Dispose()
    End Sub

    Private Sub CheckForNewAPBalances()
        Dim curPeriod = ds.GLSetups.First.PerNbr


        Dim AP = "UNKNOWN" 'NEED TO GET THESE DYNAMICALLY
        Dim APtax = "UNKNOWN"  'NEED TO GET THESE DYNAMICALLY
        Dim AdvAcc = "1200"

        If Not String.IsNullOrEmpty(dl.Spare2) Then
            Dim accounts = dl.Spare2.Split(";")
            AP = accounts(0).Trim(";")
            APtax = accounts(1).Trim(";")
            If accounts.Count > 2 Then
                AdvAcc = accounts(2).Trim(";")
            End If
           
        End If
        If AP = "UNKNOWN" Then
            Return 'Accounts Payable has not yet been setup
        End If



        si.currentFiscalPeriod = curPeriod

        'Dim q = From c In ds.vr_01620_AcctHists Where c.AccountActive = 1 And c.PeriodPost = curPeriod And c.AcctHistAcct = 2120
        Dim q = From c In ds.GLTrans Where c.Posted = "P" And c.PerPost = curPeriod And c.Acct.StartsWith(AP) And c.BalanceType = "A"

        If q.Count <> dl.Spare1 Then
            ' Dim b = From c In ds.vr_01620_AcctHists Where c.PeriodPost = curPeriod And c.AcctHistBalanceType = "A" And (c.AcctHistAcct.StartsWith(AP) Or c.AcctHistAcct.StartsWith(APtax))
            '        Group By c.AcctHistSub Into g = Group Select New With {AcctHistSub, .AccPay = g.Where(Function(x) x.AcctHistAcct.StartsWith(AP)).Select(Function(x) x.EndingBalance).Sum, .TaxAccPay = g.Where(Function(x) x.AcctHistAcct.StartsWith(APtax)).Select(Function(x) x.EndingBalance).Sum}
            ' Dim b = From c In ds.vr_01620_AcctHists Where c.PeriodPost = curPeriod And c.AcctHistBalanceType = "A"
            '                        Group By c.AcctHistSub Into g = Group Select New With {AcctHistSub, .AccBal = g.Select(Function(x) x.EndingBalance).Sum, .AdvanceBalance = g.Where(Function(x) x.AcctHistAcct.StartsWith(AdvAcc)).Select(Function(x) x.EndingBalance).Sum, .AccPay = g.Where(Function(x) x.AcctHistAcct.StartsWith(AP)).Select(Function(x) x.EndingBalance).Sum, .TaxAccPay = g.Where(Function(x) x.AcctHistAcct.StartsWith(APtax)).Select(Function(x) x.EndingBalance).Sum}

            Dim b = From c In ds.vr_01620_AcctHists Where c.PeriodPost = curPeriod And c.AcctHistBalanceType = "A" Group By c.AcctHistSub Into g = Group





            'New Accounts Payable transactions detected - so repost suggested payments!
            Dim Balances() As dynamicAgapeConnect.APBalanceInfo = New dynamicAgapeConnect.APBalanceInfo(b.Count - 1) {}
            Dim i As Integer = 0
            For Each row In b
                Dim accBal As Double = 0
                Dim accPay As Double = 0
                Dim taxAccPay As Double = 0
                Dim accRec As Double = 0
                Dim startBal As Double = 0
                If row.g.Count > 0 Then
                    startBal = row.g.Where(Function(y) Not y.AcctHistAcct.StartsWith(AdvAcc)).Sum(Function(x) x.StartingBalance1)
                    Dim r = From c In row.g Where {"I", "E"}.Contains(c.AccountAcctType.Substring(1, 1))

                    accBal = startBal + r.Sum(Function(x) x.EndingBalance * IIf(x.AccountAcctType.Substring(1, 1) = "E", -1, 1))
                    r = From c In row.g Where c.AcctHistAcct.StartsWith(AP) And {"I", "E"}.Contains(c.AccountAcctType.Substring(1, 1))

                    If r.Count > 0 Then
                        accPay = r.Sum(Function(x) x.EndingBalance * IIf(x.AccountAcctType.Substring(1, 1) = "I", 1, -1))
                    End If
                    r = From c In row.g Where c.AcctHistAcct.StartsWith(APtax)
                    If r.Count > 0 Then
                        taxAccPay = r.Sum(Function(x) x.EndingBalance * IIf(x.AccountAcctType.Substring(1, 1) = "I", 1, -1))
                    End If
                    r = From c In row.g Where c.AcctHistAcct.StartsWith(AdvAcc)
                    If r.Count > 0 Then
                        accRec = r.Sum(Function(x) x.EndingBalance * IIf(x.AccountAcctType.Substring(1, 1) = "I", 1, -1))
                    End If
                End If


                Dim insert As New dynamicAgapeConnect.APBalanceInfo
                insert.CostCenter = row.AcctHistSub
                set_if(insert.ExpensesPayable, accPay)
                set_if(insert.SpSalaryAdv, taxAccPay)
                set_if(insert.AdvanceBalance, accRec)
                set_if(insert.AccountBalance, accBal)

                Balances(i) = insert
                i += 1
            Next
            If WebS.SetAPBalances(_Password, Balances) Then
                dl.Spare1 = q.Count
            End If

        End If

    End Sub
    Private Sub set_if(ByRef setting As Object, ByVal value As Object)
        If value Is Nothing Then
            Return
        Else
            setting = value

        End If
    End Sub

    Public Function Sync() As dynamicAgapeConnect.StatusDescription
        Dim rtn As New dynamicAgapeConnect.StatusDescription
        Try

            Dim _ccs As New List(Of dynamicAgapeConnect.ACUnit)
            Dim _accs As New List(Of dynamicAgapeConnect.ACUnit)
            Dim update As dynamicAgapeConnect.DownloadResponse
            Dim uResp As New dynamicAgapeConnect.UpdateResponse()
            uResp.TntStatus = New dynamicAgapeConnect.StatusDescription()
            uResp.TntStatus.Status = "OK"


            CheckForNewAPBalances()


            If UpdateAccountsCodes(_ccs, _accs) Then
                dl.AccountCodeCount = _accs.Count
                dl.CostCenterCount = _ccs.Count
                d.SubmitChanges()
                update = WebS.RequestUpdateWithAccountRefresh(_Password, _ccs.ToArray, _accs.ToArray, si)

            Else
                update = WebS.RequestUpdate(_Password, si)
            End If

            
            If update Is Nothing Then
                'Incorrect password?
                rtn.Status = "ERROR"
                rtn.Message = "The Datalink received a null response from the website. This is usually because the password is incorrect. " _
                    & "Check your web service password by logging into your website as an administrator and going to Admin->Agape Connect->Settings."

                dl.Error = True
                dl.ErrorMessage = rtn.Message
                d.SubmitChanges()
                Return rtn
            End If

            dl.Spare2 = update.AcctsPayable & ";" & update.TaxableAcctsReceivable & ";" & update.AcctsReceivable
            d.SubmitChanges()
            If update.Status = "ERROR" Then
                rtn.Status = "ERROR"
                rtn.Message = update.Message & vbNewLine

                If update.VersionStatus = "CRITICAL" Then
                    rtn.Message &= vbNewLine & "Please upgrade to the latest version (" & update.APIVersion & ")."
                End If
                dl.Error = True
                dl.ErrorMessage = rtn.Message
                d.SubmitChanges()
                Return rtn


            ElseIf (update.Status = "Nothing to do") Then
                ' EventLog1.WriteEntry("AgapeConnect service responded: 'Nothing to do'.")
                rtn.Message = "Datapump Complete (Nothing to do)"
                rtn.Status = "OK"
                dl.Error = False
                dl.ErrorMessage = ""
                d.SubmitChanges()
                Return rtn
            ElseIf (update.Status = "New Data") Then
                ' EventLog1.WriteEntry("AgapeConnect service found new data.")
                If Not update.WebUsers Is Nothing Then
                    Try
                        ProcessTNTWebUsers(update.WebUsers)

                    Catch ex As Exception
                        uResp.TntStatus.Status = "Error"
                        uResp.TntStatus.Message = ex.Message
                    End Try

                End If
                If Not update.Rmbs Is Nothing Then
                    ProcessRmbs(update.Rmbs, update.Advances, update.AcctsReceivable, update.AcctsPayable, update.TaxableAcctsReceivable, update.ControlAccount, uResp)
                End If
                'Send Responses back to webservice
                WebS.UpdateResponses(_Password, uResp)


            End If

            update = Nothing


            rtn.Message = "Datapump Complete"
            rtn.Status = "OK"
            dl.LastSync = Now

            dl.Error = False
            dl.ErrorMessage = ""
            d.SubmitChanges()

            Return rtn
        Catch ex As System.Exception
            rtn.Message = "AgapeConnect encountered an error '" & ex.Message & "'"
            rtn.Status = "ERROR"
            dl.Error = True
            dl.ErrorMessage = rtn.Message
            d.SubmitChanges()
            Return rtn
        End Try
    End Function



    Shared Sub SaveTnTSettings(ByVal dsConnString As String, ByVal acConString As String, ByVal DatalinkId As Integer)
        Try


            Using dAC1 As New Listener2DataContext(acConString)


                Dim dl1 = From c In dAC1.Datalinks Where c.DatalinkId = DatalinkId


                If dl1.Count > 0 Then

                    Using dd = New TNTDataContext(dsConnString)
                        dl1.First.tntWebPortal = (From c In dd.Properties Where c.PropName = "WebAppUrl" Select c.PropValue).First
                        dl1.First.CurrencySymbol = (From c In dd.Properties Where c.PropName = "DefaultCurrencyCode" Select c.PropValue).First
                        Dim temp = (From c In dd.DataPumpScannerSources Where c.Name.Contains("Solomon") Or c.TemplateXML.Contains("Solomon") Select c.PropertiesXML).First

                        Dim doc As New XmlDocument()

                        doc.LoadXml(temp)

                        Dim pls = doc.SelectSingleNode("/Data_Source/Placeholder_List")

                        For Each child As XmlNode In pls.ChildNodes

                            If child.OuterXml.Contains("$FIN_ACCT_ADV_SUFFIX$") Then
                                dl1.First.AdvanceSuffix = child.InnerText

                            ElseIf child.OuterXml.Contains("$ADVANCE_ACCT_PREFIX$") Then
                                dl1.First.AdvancePrefix = child.InnerText
                            ElseIf child.OuterXml.Contains("$COMPANY_ID$") Then
                                dl1.First.CompanyID = child.InnerText
                            ElseIf child.OuterXml.Contains("$APP_DATABASE$") Then
                                If Not String.IsNullOrEmpty(dl1.First.solConnectionString) Then
                                    If dl1.First.solConnectionString.Contains("Catalog") Then
                                        Dim TntSol As String = child.InnerText
                                        If Not dl1.First.solConnectionString.Contains(TntSol) Then
                                            MsgBox("Warning: TnT appears to be connected to a different Solomon(Dynamics) Database than you specified in the datalink. Please check the datalink settings in ACDatalinks Manager.", MsgBoxStyle.Exclamation)

                                        End If
                                    End If
                                End If
                            End If
                        Next

                        dAC1.SubmitChanges()
                    End Using
                End If
            End Using
        Catch ex As Exception
            ' MsgBox("Error: " & ex.Message)
        End Try
    End Sub
#End Region

#Region "tntDataserver Interface"
    Private Sub ProcessTNTWebUsers(ByVal WebUsers As dynamicAgapeConnect.WebUser())
        'EventLog1.WriteEntry("Processing TNT Data")

        Dim dAC As New Listener2DataContext(d.Connection.ConnectionString)

        dAC.WebUserCostCenters.DeleteAllOnSubmit(From c In dAC.WebUserCostCenters Where c.DatalinkId = dl.DatalinkId)
        dAC.WebUsers.DeleteAllOnSubmit(From c In dAC.WebUsers Where c.DatalinkId = dl.DatalinkId)
        dAC.WebUserFinancialCategories.DeleteAllOnSubmit(From c In dAC.WebUserFinancialCategories Where c.DatalinkId = dl.DatalinkId)
        dAC.WebUserDesignations.DeleteAllOnSubmit(From c In dAC.WebUserDesignations Where c.DatalinkId = dl.DatalinkId)

        dAC.SubmitChanges()

        For Each row In WebUsers
            'Blank the interim tables
            Try


                Dim insert As New Listener2.WebUser
                insert.UserName = row.Name
                insert.Email = row.GCXUserName
                insert.DefaultRegistrationCode = row.GCXUserName
                insert.DatalinkId = dl.DatalinkId
                dAC.WebUsers.InsertOnSubmit(insert)

                For Each profile In row.PersonalAccounts
                    Dim insertCC As New Listener2.WebUserCostCenter
                    insertCC.CostCenterCode = profile.CostCenter
                    insertCC.ProfileCode = ""
                    insertCC.ProfileDescription = "Personal Accounts"
                    insertCC.UserName = row.Name
                    insertCC.DatalinkId = dl.DatalinkId
                    dAC.WebUserCostCenters.InsertOnSubmit(insertCC)
                    If dl.AdvanceSuffix <> "" Then
                        Dim insertCC2 As New Listener2.WebUserCostCenter
                        insertCC2.CostCenterCode = profile.CostCenter & "-" & dl.AdvanceSuffix
                        insertCC2.ProfileCode = ""
                        insertCC2.ProfileDescription = "Personal Accounts"
                        insertCC2.UserName = row.Name
                        insertCC2.DatalinkId = dl.DatalinkId
                        dAC.WebUserCostCenters.InsertOnSubmit(insertCC2)


                    End If
                    Dim designations = profile.Designations.Split(";")
                    For Each desig In designations
                        Dim theDesig = desig.Trim()
                        If theDesig.Length > 0 Then
                            If (From c In dt.Desigs Where c.Code = theDesig).Count = 0 Then
                                Dim insertD As New Listener2.WebUserDesignation
                                insertD.DatalinkId = dl.DatalinkId
                                insertD.ProfileCode = ""
                                insertD.ProfileDescription = "Personal Accounts"
                                insertD.DesignationCode = theDesig
                                insertD.Username = row.Name
                                dAC.WebUserDesignations.InsertOnSubmit(insertD)
                            End If
                        End If
                    Next

                   

                Next
                For Each profile In row.DepartmentAccounts
                    Dim insertCC As New Listener2.WebUserCostCenter
                    insertCC.CostCenterCode = profile.CostCenter
                    insertCC.ProfileCode = 1
                    insertCC.ProfileDescription = "Department Accounts"
                    insertCC.UserName = row.Name
                    insertCC.DatalinkId = dl.DatalinkId
                    dAC.WebUserCostCenters.InsertOnSubmit(insertCC)

                    Dim designations = profile.Designations.Split(";")
                    For Each desig In designations
                        Dim theDesig = desig.Trim()
                        If theDesig.Length > 0 Then
                            If (From c In dt.Desigs Where c.Code = theDesig).Count = 0 Then
                                Dim insertD As New Listener2.WebUserDesignation
                                insertD.DatalinkId = dl.DatalinkId
                                insertD.ProfileCode = 1
                                insertD.ProfileDescription = "Department Accounts"
                                insertD.DesignationCode = theDesig
                                insertD.Username = row.Name
                                dAC.WebUserDesignations.InsertOnSubmit(insertD)
                            End If
                        End If
                    Next

                Next

                For Each profile In row.TeamAccounts
                    Dim insertCC As New Listener2.WebUserCostCenter
                    insertCC.CostCenterCode = profile.CostCenter
                    insertCC.ProfileCode = 2
                    insertCC.ProfileDescription = "Team Accounts"
                    insertCC.UserName = row.Name
                    insertCC.DatalinkId = dl.DatalinkId
                    dAC.WebUserCostCenters.InsertOnSubmit(insertCC)
                Next

                If row.AccountsUser Then
                    Dim insertFC As New Listener2.WebUserFinancialCategory
                    insertFC.Category = ""
                    insertFC.DatalinkId = dl.DatalinkId
                    insertFC.StaffProfileCode = 3
                    insertFC.StaffProfileDesc = "All Accounts"
                    insertFC.Username = row.Name
                    dAC.WebUserFinancialCategories.InsertOnSubmit(insertFC)
                End If
            Catch ex As Exception
                'error inserting webuser.
                dl.ErrorMessage &= "Problem setting up dataserver webprofile for " & row.Name & ". " & ex.Message
                d.SubmitChanges()
            End Try
        Next
        Try
            dAC.SubmitChanges()
        Catch ex As Exception
            MsgBox(ex.Message)
            Throw
        End Try

        'Now create the webUsers manually // DISABLED - REPLACED BY TROY'S NEW WEBSERVICES. WEB USERS WILL NO LONGER GO THROUGHT THE PUMP

        'Dim allwebUsers = From c In d.WebUsers Where c.DatalinkId = dl.DatalinkId
        'For Each row In allwebUsers
        '    Dim tntWebUsers = From c In dt.WebUsers Where c.Code = row.UserName

        '    If tntWebUsers.Count = 0 Then
        '        'create a new webuser
        '        Dim insertWU As New TNT.WebUser
        '        insertWU.Code = row.UserName
        '        insertWU.DefaultEmail = row.Email
        '        insertWU.DefaultEmailPassword = ""
        '        insertWU.DefaultRegistrationCode = row.DefaultRegistrationCode
        '        insertWU.Email = row.Email
        '        insertWU.EmailPassword = ""
        '        insertWU.IsTranslator = False
        '        insertWU.RegistrationCode = row.DefaultRegistrationCode
        '        insertWU.RowGuid = Guid.NewGuid()
        '        insertWU.ScanCode = ""
        '        insertWU.SsoCode = ""
        '        insertWU.SsoEmail = row.DefaultRegistrationCode
        '        insertWU.WebUserID = dt.WebUsers.Max(Function(x) x.WebUserID) + 1
        '        dt.WebUsers.InsertOnSubmit(insertWU)
        '        dt.SubmitChanges()
        '    End If

        'Next



        'TriggerTnT()   'DISABLED> TNT WEB USERS NO LONGER GO THROUGH ACDATALINKS (THEY ARE PRROCESSED DIRECTLY USING DATASERVER'S WEB USERS
        'THERE IS NO POINT IN TRIGGERING TNT AT THIS POINT, AS BATCHES ARE NOT GET POSTED.
    End Sub
    Function ConvertToSecureString(ByVal str As String)
        Dim password As New Security.SecureString
        For Each c As Char In str.ToCharArray
            password.AppendChar(c)
        Next
        Return password
    End Function
    Private Sub TriggerTnT()
        Try
           

            Dim myProcess As New Process()
            Dim tntPath = d.Settings.First.tntPath
            If Not tntPath Is Nothing Then
                If tntPath <> "" Then
                    Dim tnt As New ProcessStartInfo()
                    tnt.WorkingDirectory = tntPath
                    tnt.CreateNoWindow = True

                    tnt.LoadUserProfile = True

                    tnt.UseShellExecute = False


                    Dim ssb As New System.Data.SqlClient.SqlConnectionStringBuilder(dt.Connection.ConnectionString)
                    tnt.FileName = tntPath & "TntMPD.DataServer.Run.exe"
                    tnt.Arguments = "/server=" & ssb.DataSource & " /database=" & ssb.InitialCatalog
                    If ssb.IntegratedSecurity = False Then
                        tnt.Arguments &= " /login=" & ssb.UserID & " /password=" & ssb.Password & " /autorun"
                    Else

                        tnt.Arguments &= " /ntauth /autorun"
                    End If

                    myProcess.StartInfo = tnt
                    myProcess.Start()

                Else
                    '   MsgBox("Error2: Couldn't Find Tnt registry key ")
                    dl.Error = True
                    dl.ErrorMessage &= "Error2: Couldn't Find Tnt registry key "
                    d.SubmitChanges()
                End If
            Else
                ' MsgBox("Error1: Couldn't Find Tnt registry key ")
                dl.Error = True
                dl.ErrorMessage &= "Error1: Couldn't Find Tnt registry key "
                d.SubmitChanges()
            End If
        Catch ex As Exception
            'couldn't launch tnt
            dl.Error = True
            MsgBox("Error1: Couldn't Launch Tnt: " & ex.Message)
            dl.ErrorMessage &= "Error: Can't launch tnt: " & ex.Message
            d.SubmitChanges()
        End Try
    End Sub

#End Region
#Region "Solomon Interface"
    Private Function GetNewBatchNumber() As Integer
        Dim q = ds.GLSetups.First
        q.LastBatNbr = ZeroFill(CInt(q.LastBatNbr + 1), 6)
        ds.SubmitChanges()
        Return CInt(q.LastBatNbr)
    End Function


    Private Function GetSolPeriod(ByVal Year As Integer, ByVal Period As Integer) As String
        'Takes a Year and Period (where Jan = 1) and converts it to the solomon period.
        Dim q = ds.GLSetups.First
        If CStr(Year).Length <> 4 Or Period > 12 Or Period < 1 Then
            Return ""

        End If
        Dim periodStr = ZeroFill(Period, 2)

        If q.BegFiscalYr = 0 Then
            ' Year -= 1

        End If

        Select Case True
            Case q.FiscalPerEnd00.StartsWith(periodStr)

                Return CStr(IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 1, Year + 1, Year)) & "01"
            Case q.FiscalPerEnd01.StartsWith(periodStr)
                Return CStr(IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 2, Year + 1, Year)) & "02"
            Case q.FiscalPerEnd02.StartsWith(periodStr)
                Return CStr(IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 3, Year + 1, Year)) & "03"
            Case q.FiscalPerEnd03.StartsWith(periodStr)
                Return CStr(IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 4, Year + 1, Year)) & "04"
            Case q.FiscalPerEnd04.StartsWith(periodStr)
                Return CStr(IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 5, Year + 1, Year)) & "05"
            Case q.FiscalPerEnd05.StartsWith(periodStr)
                Return CStr(IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 6, Year + 1, Year)) & "06"
            Case q.FiscalPerEnd06.StartsWith(periodStr)
                Return CStr(IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 7, Year + 1, Year)) & "07"
            Case q.FiscalPerEnd07.StartsWith(periodStr)
                Return CStr(IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 8, Year + 1, Year)) & "08"
            Case q.FiscalPerEnd08.StartsWith(periodStr)
                Return CStr(IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 9, Year + 1, Year)) & "09"
            Case q.FiscalPerEnd09.StartsWith(periodStr)
                Return CStr(IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 10, Year + 1, Year)) & "10"
            Case q.FiscalPerEnd10.StartsWith(periodStr)
                Return CStr(IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 11, Year + 1, Year)) & "11"
            Case q.FiscalPerEnd11.StartsWith(periodStr)
                Return CStr(IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 12, Year + 1, Year)) & "12"
            Case q.FiscalPerEnd12.StartsWith(periodStr)
                Return CStr(IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 1, Year + 1, Year)) & "13"
        End Select
        Return ""

    End Function
    Private Sub NormalizeSolPeriod(ByVal SolPeriod As String, ByRef outYear As Integer, ByRef outPeriod As Integer)
        'takes a Solomon period and converts it to a normal year and period (where Jan=1)
        Dim q = ds.GLSetups.First
        outYear = Nothing
        outPeriod = Nothing
        If SolPeriod.Length <> 6 Then
            Return
        End If
        Select Case Right(SolPeriod, 2)
            Case "01"
                outPeriod = Left(q.FiscalPerEnd00, 2)
                outYear = IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 1, CInt(Left(SolPeriod, 4)) - 1, CInt(Left(SolPeriod, 4)))
                Return
            Case "02"
                outPeriod = Left(q.FiscalPerEnd01, 2)
                outYear = IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 2, CInt(Left(SolPeriod, 4)) - 1, CInt(Left(SolPeriod, 4)))
                Return
            Case "03"
                outPeriod = Left(q.FiscalPerEnd02, 2)
                outYear = IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 3, CInt(Left(SolPeriod, 4)) - 1, CInt(Left(SolPeriod, 4)))
                Return
            Case "04"
                outPeriod = Left(q.FiscalPerEnd03, 2)
                outYear = IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 4, CInt(Left(SolPeriod, 4)) - 1, CInt(Left(SolPeriod, 4)))
                Return
            Case "05"
                outPeriod = Left(q.FiscalPerEnd04, 2)
                outYear = IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 5, CInt(Left(SolPeriod, 4)) - 1, CInt(Left(SolPeriod, 4)))
                Return
            Case "06"
                outPeriod = Left(q.FiscalPerEnd05, 2)
                outYear = IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 6, CInt(Left(SolPeriod, 4)) - 1, CInt(Left(SolPeriod, 4)))
                Return
            Case "07"
                outPeriod = Left(q.FiscalPerEnd06, 2)
                outYear = IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 7, CInt(Left(SolPeriod, 4)) - 1, CInt(Left(SolPeriod, 4)))
                Return
            Case "08"
                outPeriod = Left(q.FiscalPerEnd07, 2)
                outYear = IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 8, CInt(Left(SolPeriod, 4)) - 1, CInt(Left(SolPeriod, 4)))
                Return
            Case "09"
                outPeriod = Left(q.FiscalPerEnd08, 2)
                outYear = IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 9, CInt(Left(SolPeriod, 4)) - 1, CInt(Left(SolPeriod, 4)))
                Return
            Case "10"
                outPeriod = Left(q.FiscalPerEnd09, 2)
                outYear = IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 10, CInt(Left(SolPeriod, 4)) - 1, CInt(Left(SolPeriod, 4)))
                Return
            Case "11"
                outPeriod = Left(q.FiscalPerEnd10, 2)
                outYear = IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 11, CInt(Left(SolPeriod, 4)) - 1, CInt(Left(SolPeriod, 4)))
                Return
            Case "12"
                outPeriod = Left(q.FiscalPerEnd11, 2)
                outYear = IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 12, CInt(Left(SolPeriod, 4)) - 1, CInt(Left(SolPeriod, 4)))
                Return
            Case "13"
                outPeriod = Left(q.FiscalPerEnd12, 2)
                outYear = IIf(CInt(Left(q.FiscalPerEnd00, 2)) > 13, CInt(Left(SolPeriod, 4)) - 1, CInt(Left(SolPeriod, 4)))
                Return

        End Select
        Return

    End Sub

    Public Sub TestPeriods()
        Dim aPeriod As Integer
        Dim aYear As Integer
        Dim OutStr As String = ""

        Dim startDate As Date = New Date(2011, 9, 1)
        While startDate < New Date(2012, 12, 1)
            Dim SolPeriod As String = GetSolPeriod(Year(startDate), Month(startDate))
            NormalizeSolPeriod(SolPeriod, aYear, aPeriod)
            OutStr &= Month(startDate) & "/" & Year(startDate) & " -> " & SolPeriod & " -> " & aPeriod & "/" & aYear & vbNewLine
            startDate = startDate.AddMonths(1)
        End While
        MsgBox(OutStr)
    End Sub

    Private Function ValidateRmb(ByVal theRmb As dynamicAgapeConnect.Rmb, ByRef inStatus As dynamicAgapeConnect.StatusDescription) As Boolean
        'Check if Rmb has already been processed (and not deleted)
        Dim q = From c In ds.GLTrans Join b In ds.Batches On c.BatNbr Equals b.BatNbr Where c.RefNbr = "R" & ZeroFill(theRmb.RID, 7) Select c.BatNbr, b.Status
        If q.Count > 0 Then
            inStatus.Status = "ERROR"
            inStatus.Message = "Reimbursement has already been inserted into Solomon in Batch: " & q.First.BatNbr
            Return False
        End If


        Return True
    End Function


    Private Function ValidateCombo(ByVal CostCenter As String, ByVal Account As String) As Boolean
        Dim q = From c In ds.vs_AcctSubs Where c.Acct = Account And c.Sub = CostCenter And c.Active
        Return q.Count > 0

        
    End Function


    Private Function ValidateAdv(ByVal theAdv As dynamicAgapeConnect.Adv, ByRef inStatus As dynamicAgapeConnect.StatusDescription, ByVal AccsReceivable As String, ByVal AccsPayable As String) As Boolean
        'Check if Rmb has already been processed (and not deleted)

        Dim q = From c In ds.GLTrans Join b In ds.Batches On c.BatNbr Equals b.BatNbr Where c.RefNbr = "A" & ZeroFill(theAdv.LocalAdvanceId, 7) Select c.BatNbr, b.Status
        If q.Count > 0 Then
            inStatus.Status = "ERROR"
            inStatus.Message = "Advance has already been inserted into Dynamics in Batch: " & q.First.BatNbr
            Return False
        End If
        If Not ValidateCombo(theAdv.PersonalCostCenter, AccsPayable) Then
            inStatus.Status = "ERROR"
            inStatus.Message = "This combination of Account and Responsebility Center is not valid: " & AccsPayable & " " & theAdv.PersonalCostCenter
            Return False
        End If
        If Not ValidateCombo(theAdv.PersonalCostCenter, AccsReceivable) Then
            inStatus.Status = "ERROR"
            inStatus.Message = "This combination of Account and Responsebility Center is not valid: " & AccsReceivable & " " & theAdv.PersonalCostCenter
            Return False
        End If
        Return True
    End Function

    Private Sub ProcessRmbs(ByVal Rmbs As dynamicAgapeConnect.Rmb(), ByVal Advances As dynamicAgapeConnect.Adv(), ByVal AccsReceivable As String, ByVal AccsPayable As String, ByVal TaxAccountsReceivable As String, ByVal ControlAccount As String, ByRef uResp As dynamicAgapeConnect.UpdateResponse)

        ' EventLog1.WriteEntry("Processing Reimbursments")
        Dim rtn As New List(Of dynamicAgapeConnect.StatusDescription)

        ' Dim dS As New SolomonDataContext
        Dim BlankDate = New Date(1900, 1, 1)
        ' Dim PeriodStr = "YYYYMM"    ' Need to get this for the Fiscal period from the GLSetup table
        Dim BatNumber As Integer = GetNewBatchNumber()
        Dim TotalAmt = (From c In Rmbs Select CDbl(c.Lines.Sum(Function(p) p.GrossAmount))).Sum  ' What about advances
        TotalAmt += (From c In Advances Select CDbl(c.Amount)).Sum



        Dim batchHeader As New Batch
        batchHeader.Acct = ""
        batchHeader.AutoRev = 0
        batchHeader.AutoRevCopy = 0
        batchHeader.BalanceType = "A"
        batchHeader.BankAcct = ""
        batchHeader.BankSub = ""
        batchHeader.BaseCuryID = ds.GLSetups.First.BaseCuryId
        batchHeader.BatNbr = ZeroFill(BatNumber, 6)
        batchHeader.BatType = "N"
        batchHeader.clearamt = 0
        batchHeader.Cleared = 0
        batchHeader.CpnyID = ds.GLSetups.First.CpnyId
        batchHeader.Crtd_DateTime = Today
        batchHeader.Crtd_Prog = "01010"
        batchHeader.Crtd_User = "SYSADMIN"
        batchHeader.CrTot = TotalAmt
        batchHeader.CtrlTot = TotalAmt
        batchHeader.CuryCrTot = TotalAmt
        batchHeader.CuryCtrlTot = TotalAmt
        batchHeader.CuryDepositAmt = 0
        batchHeader.CuryDrTot = TotalAmt
        batchHeader.CuryEffDate = Today
        batchHeader.CuryId = ds.GLSetups.First.BaseCuryId
        batchHeader.CuryMultDiv = "M"
        batchHeader.CuryRate = 1.0
        batchHeader.CuryRateType = ""
        batchHeader.Cycle = 0
        batchHeader.DateClr = BlankDate
        batchHeader.DateEnt = Today
        batchHeader.DepositAmt = 0
        batchHeader.Descr = ""
        batchHeader.DrTot = TotalAmt
        batchHeader.EditScrnNbr = "01010"
        batchHeader.GLPostOpt = "D"
        batchHeader.JrnlType = "GJ"
        batchHeader.LedgerID = ds.GLSetups.First.LedgerID
        batchHeader.LUpd_DateTime = Today
        batchHeader.LUpd_Prog = "01010"
        batchHeader.LUpd_User = "SYSADMIN"
        batchHeader.Module = "GL"
        batchHeader.NbrCycle = 0
        batchHeader.NoteID = 0
        batchHeader.OrigBatNbr = ""
        batchHeader.OrigCpnyID = "" ' ds.GLSetups.First.CpnyId   ' (Troy puts CpnyId, but all other batches are blank for batch header)
        batchHeader.OrigScrnNbr = ""
        batchHeader.PerEnt = ds.GLSetups.First.PerNbr
        batchHeader.PerPost = ds.GLSetups.First.PerNbr
        batchHeader.Rlsed = 0
        batchHeader.Status = "H"
        batchHeader.Sub = ""
        batchHeader.S4Future01 = ""
        batchHeader.S4Future02 = ""
        batchHeader.S4Future03 = 0
        batchHeader.S4Future04 = 0
        batchHeader.S4Future05 = 0
        batchHeader.S4Future06 = 0
        batchHeader.S4Future07 = BlankDate
        batchHeader.S4Future08 = BlankDate
        batchHeader.S4Future09 = 0
        batchHeader.S4Future10 = 0
        batchHeader.S4Future11 = ""
        batchHeader.S4Future12 = ""
        batchHeader.User1 = ""
        batchHeader.User2 = ""
        batchHeader.User3 = 0
        batchHeader.User4 = 0
        batchHeader.User5 = ""
        batchHeader.User6 = ""
        batchHeader.User7 = BlankDate
        batchHeader.User8 = BlankDate



        'Now the transactions
        Dim ts As New List(Of GLTran)

        Dim i As Integer = 1

        For Each theRmb In Rmbs
            Dim rmbStatus As New dynamicAgapeConnect.StatusDescription
            rmbStatus.RowId = theRmb.RmbNo
            If Not ValidateRmb(theRmb, rmbStatus) Then ' Validate

                rtn.Add(rmbStatus)
            Else
                'Do the Transactions
                Dim ref = theRmb.UserInitials & "-"
                Dim refNbr = "R" & ZeroFill(theRmb.RID, 7)
                For Each Line In theRmb.Lines
                    ts.Add(createTransaction(refNbr, BatNumber, i, ds.GLSetups.First, theRmb.RID, Line.CostCenter, IIf(Line.Taxable, TaxAccountsReceivable, Line.AccountCode), ref & IIf(Line.Taxable, "(taxable)", "") & Line.Comment, IIf(Line.GrossAmount > 0, 0, Line.GrossAmount), IIf(Line.GrossAmount > 0, Line.GrossAmount, 0), theRmb.ProcessDate, rmbStatus))
                    i += 1
                Next
                Dim RmbTotal As Double = (From c In theRmb.Lines Select c.GrossAmount).Sum
                Dim rmbAdvance As Double = theRmb.AdvanceRequest

                If String.IsNullOrEmpty(theRmb.PersonalAccountCode) Then
                    'Normal Mode!'
                    RmbTotal -= rmbAdvance

                    If RmbTotal <> 0 Then
                        ts.Add(createTransaction(refNbr, BatNumber, i, ds.GLSetups.First, theRmb.RID, theRmb.PersonalCostCenter, AccsPayable, ref & "-Payment for R" & theRmb.RID, IIf(RmbTotal, RmbTotal, 0), IIf(RmbTotal > 0, 0, RmbTotal), theRmb.ProcessDate, rmbStatus))
                        i += 1
                    End If
                    If rmbAdvance <> 0 Then
                        ts.Add(createTransaction(refNbr, BatNumber, i, ds.GLSetups.First, theRmb.RID, theRmb.PersonalCostCenter, AccsReceivable, ref & "-Pay off advance with R" & theRmb.RID, IIf(rmbAdvance > 0, rmbAdvance, 0), IIf(rmbAdvance > 0, 0, rmbAdvance), theRmb.ProcessDate, rmbStatus))
                        i += 1
                    End If
                Else
                    'PersonalAccountCode Mode
                    If RmbTotal <> 0 Then
                        ts.Add(createTransaction(refNbr, BatNumber, i, ds.GLSetups.First, theRmb.RID, ControlAccount, theRmb.PersonalAccountCode, ref & "-Payment for R" & theRmb.RID, IIf(RmbTotal, RmbTotal, 0), IIf(RmbTotal > 0, 0, RmbTotal), theRmb.ProcessDate, rmbStatus))
                        i += 1
                    End If



                End If
                If rmbStatus.Status = "ERROR" Then
                    ts.RemoveAll(Function(x) x.BatNbr = BatNumber And x.RefNbr = refNbr)
                Else
                    rmbStatus.Status = "SUCCESS"

                End If
                rtn.Add(rmbStatus)
            End If




        Next


        'Process Advances
        For Each adv In Advances

            Dim advStatus As New dynamicAgapeConnect.StatusDescription
            advStatus.RowId = -adv.AdvanceId
            If Not ValidateAdv(adv, advStatus, AccsReceivable, AccsPayable) Then ' Validate
                rtn.Add(advStatus)
            Else
                Dim ref = adv.UserInitials & "-" & ZeroFill(adv.LocalAdvanceId, 7)
                Dim refNbr = "R" & ZeroFill(adv.LocalAdvanceId, 7)
                ts.Add(createTransaction(refNbr, BatNumber, i, ds.GLSetups.First, adv.LocalAdvanceId, adv.PersonalCostCenter, AccsReceivable, ref, IIf(adv.Amount > 0, 0, adv.Amount), IIf(adv.Amount > 0, adv.Amount, 0), adv.ProcessDate, advStatus))
                i += 1
                ts.Add(createTransaction(refNbr, BatNumber, i, ds.GLSetups.First, adv.LocalAdvanceId, adv.PersonalCostCenter, AccsPayable, ref & " - Payment", IIf(adv.Amount, adv.Amount, 0), IIf(adv.Amount > 0, 0, adv.Amount), adv.ProcessDate, advStatus))
                i += 1
                advStatus.BatchId = BatNumber
                advStatus.Status = "SUCCESS"
                rtn.Add(advStatus)


            End If


        Next









        Try
            ds.Batches.InsertOnSubmit(batchHeader)
            ds.GLTrans.InsertAllOnSubmit(ts.AsEnumerable)
            ds.SubmitChanges()



            '  EventLog1.WriteEntry("Successfully added Reimbursments")

        Catch ex As Exception
            'backout everything for  this batchnumber
            ' EventLog1.WriteEntry(ex.Message)

            'EventLog1.WriteEntry("Backing Out Transactions")
            ds.GLTrans.DeleteAllOnSubmit(From c In ds.GLTrans Where c.BatNbr = batchHeader.BatNbr And c.Module = "GL")
            ds.Batches.DeleteAllOnSubmit(From c In ds.Batches Where c.BatNbr = batchHeader.BatNbr And c.Module = "GL")
            ds.SubmitChanges()
            For Each row In rtn
                row.Status = "ERROR"
                row.Message = "Error committing transactions to Solomon. More info: " & ex.Message

            Next


        End Try
        uResp.Rmbs = rtn.ToArray


    End Sub
    Private Function createTransaction(ByVal refNbr As String, ByVal BatchNo As Integer, ByVal LineNumber As Integer, ByVal settings As GLSetup, ByVal RID As String, ByVal CostCenter As String, ByVal AccountCode As String, ByVal Description As String, ByVal Credit As String, ByVal Debit As String, ByVal TransDate As Date, ByRef RmbStatus As dynamicAgapeConnect.StatusDescription)

        If ValidateCombo(CostCenter, AccountCode) = False Then
            RmbStatus.Status = "ERROR"
            RmbStatus.Message = "This combination of Account and Responsebility Center is not valid: " & AccountCode & " " & CostCenter
        End If

        Dim BlankDate = New Date(1900, 1, 1)
        Dim period As String = settings.PerNbr

        
        'set trandate to be in the correct period.
        Dim aMonth As Integer
        Dim aYear As Integer
        NormalizeSolPeriod(period, aYear, aMonth)


        Dim StartDate = New Date(aYear, aMonth, 1)
        Dim EndDate = StartDate.AddMonths(1).AddDays(-1)
        If TransDate < StartDate Then
            TransDate = StartDate
        End If
        If TransDate > EndDate Then
            TransDate = EndDate
        End If
        Dim insert As New GLTran
        insert.Acct = AccountCode
        insert.AppliedDate = BlankDate
        insert.BalanceType = "A"
        insert.BaseCuryID = settings.BaseCuryId
        insert.BatNbr = ZeroFill(BatchNo, 6)
        insert.CpnyID = settings.CpnyId
        insert.CrAmt = Credit
        insert.Crtd_DateTime = TransDate
        insert.Crtd_Prog = "01010"
        insert.Crtd_User = "SYSADMIN"
        insert.CuryCrAmt = Credit
        insert.CuryDrAmt = Debit
        insert.CuryEffDate = TransDate
        insert.CuryId = settings.BaseCuryId
        insert.CuryMultDiv = "M"
        insert.CuryRate = 1.0
        insert.CuryRateType = ""
        insert.DrAmt = Debit
        insert.EmployeeID = ""
        insert.ExtRefNbr = ""
        insert.FiscYr = Left(settings.PerNbr, 4)
        insert.IC_Distribution = 0
        insert.Id = ""
        insert.JrnlType = "GJ"
        insert.Labor_Class_Cd = ""
        insert.LedgerID = settings.LedgerID
        insert.LineId = 0
        insert.LineNbr = LineNumber
        insert.LineRef = ""
        insert.LUpd_DateTime = Now
        insert.LUpd_Prog = "01010"
        insert.LUpd_User = "SYSADMIN"
        insert.Module = "GL"
        insert.NoteID = 0
        insert.OrigAcct = ""
        insert.OrigBatNbr = ""
        insert.OrigCpnyID = ds.GLSetups.First.CpnyId
        insert.OrigSub = ""
        insert.PC_Flag = ""
        insert.PC_ID = ""
        insert.PC_Status = ""
        insert.PerEnt = settings.PerNbr
        insert.PerPost = settings.PerNbr
        insert.Posted = "U"
        insert.ProjectID = ""
        insert.Qty = 0
        insert.RefNbr = refNbr
        insert.RevEntryOption = ""
        insert.Rlsed = 0
        insert.S4Future01 = ""
        insert.S4Future02 = ""
        insert.S4Future03 = 0
        insert.S4Future04 = 0
        insert.S4Future05 = 0
        insert.S4Future06 = 0
        insert.S4Future07 = BlankDate
        insert.S4Future08 = BlankDate
        insert.S4Future09 = 0
        insert.S4Future10 = 0
        insert.S4Future11 = ""
        insert.S4Future12 = ""
        insert.ServiceDate = BlankDate
        insert.Sub = CostCenter
        insert.TaskID = ""
        insert.TranDate = TransDate
        insert.TranDesc = Left(Description, 30)
        insert.TranType = "GL"
        insert.Units = 0
        insert.User1 = ""
        insert.User2 = ""
        insert.User3 = 0
        insert.User4 = 0
        insert.User5 = ""
        insert.User6 = ""
        insert.User7 = BlankDate
        insert.User8 = BlankDate
        RmbStatus.ActualPeriod = aMonth

        RmbStatus.ActualYear = aYear
        Return insert




    End Function
    Private Function UpdateAccountsCodes(ByRef _ccs As List(Of dynamicAgapeConnect.ACUnit), ByRef _accs As List(Of dynamicAgapeConnect.ACUnit)) As Boolean
        Dim ccs = From c In ds.SubAccts Where c.Active
        Dim accs = From c In ds.Accounts Where c.Active


        If dl.CostCenterCount <> ccs.Count Or dl.AccountCodeCount <> accs.Count Then
            Dim update As New dynamicAgapeConnect.DownloadResponse
            For Each row In ccs

                Dim therow = row
                Dim _update As New dynamicAgapeConnect.ACUnit
                _update.Code = therow.Sub.Trim()
                _update.Name = therow.Descr.Trim()
                _update.Type = GetCCType(therow.Sub.Trim())
                _ccs.Add(_update)
            Next

            For Each row In accs
                Dim therow = row
                Dim _update As New dynamicAgapeConnect.ACUnit
                _update.Code = therow.Acct.Trim()
                _update.Name = therow.Descr.Trim()
                _update.Type = GetAccountType(therow.AcctType)
                _accs.Add(_update)
            Next
            Return True
        Else
            Return False

        End If
    End Function

#End Region

#Region "Utilities"
    Protected Function ZeroFill(ByVal number As Integer, ByVal len As Integer) As String
        If number.ToString.Length > len Then
            Return Right(number.ToString, len)
        Else
            Dim Filler As String = ""
            For i As Integer = 1 To len - number.ToString.Length
                Filler &= "0"
            Next
            Return Filler & number.ToString
        End If


    End Function
    Private Function GetCCType(ByVal CC As String) As Integer
        CC = CC.Trim()

        If (CC.Length = 4 And Right(CC, 4) = "0000") Or (CC.Length = 5 And Right(CC, 5) = "00000") Then
            Return 2 ' Other
        ElseIf CC(IIf(CC.Length = 6, 2, 0)) = "1" Or CC(IIf(CC.Length = 6, 2, 0)) = "2" Then
            Return 1 'Staff
        Else
            Return 0 'Dept

        End If
    End Function
    Private Function GetAccountType(ByVal SolType As String) As Integer
        Select Case SolType.Last
            Case "E"
                Return 4
            Case "I"
                Return 3
            Case "A"
                Return 1
            Case "L"
                Return 2
            Case Else
                Return 5

        End Select
    End Function

    Protected Sub SetSetupInfo()
        si = New dynamicAgapeConnect.SetupInfo()
        si.AdvPrefix = dl.AdvancePrefix
        si.AdvSuffix = dl.AdvanceSuffix
        si.CompanyId = dl.CompanyID
        si.CurrencyCode = dl.CurrencySymbol
        si.urlDataserverPortal = dl.tntWebPortal
        si.acDatalink_acDatalink_Error = IIf(dl.Error, dl.ErrorMessage, "")
        si.acDatalink_Active = dl.Active
        si.acDatalink_DatalinkId = dl.DatalinkId
        si.acDatalink_ComputerName = My.Computer.Name

        si.acDatalink_Version = System.Reflection.Assembly.GetExecutingAssembly.GetName().Version.ToString
        si.acDatalink_WindowsVersion = My.Computer.Info.OSFullName

        If d.Settings.Count > 0 Then
            si.acDatalink_TNT_InstallPath = d.Settings.First.tntPath
            si.acDatalink_DataserverVersion = d.Settings.First.tntVersion
            si.acDatalink_PollDelayInSeconds = d.Settings.First.PollDelayInSeconds
        End If



    End Sub
#End Region

















End Class
