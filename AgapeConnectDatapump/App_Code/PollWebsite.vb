
Imports AgapeConnectDatapump.AgapeEncryption

Public Class PollWebsite

#Region "Properties"
    Private _Password As String = ""
    Private WebS As dynamicAgapeConnect.DatatSync
    Dim d As New ListenerDataContext
    Private dt As TNTDataContext
    Private ds As SolomonDataContext
    Private dl As New Listener.Datalink
#End Region
#Region "Contructors"
    Public Sub New(ByVal DatalinkId As Integer)
        'EventLog1.WriteEntry("Checking for new accounts/ cost Centers.")


        dl = (From c In d.Datalinks Where c.DatalinkId = DatalinkId).First



        WebS.UseDefaultCredentials = True
        WebS.RequestEncoding = System.Text.Encoding.UTF8
        WebS.Url = dl.webURL

        'CheckForNewAccounts
        dt = New TNTDataContext(dl.dsConnectionString)
        ds = New SolomonDataContext(dl.solConnectionString)
        _Password = AgapeEncrypt.Decrypt(dl.webPassword)

    End Sub
#End Region

#Region "Public Functions"
    Sub Sync()
        Try

            Dim _ccs As New List(Of dynamicAgapeConnect.ACUnit)
            Dim _accs As New List(Of dynamicAgapeConnect.ACUnit)
            Dim update As dynamicAgapeConnect.DownloadResponse
            Dim uResp As New dynamicAgapeConnect.UpdateResponse()
            If UpdateAccountsCodes(_ccs, _accs) Then
                update = WebS.RequestUpdateWithAccountRefresh(_Password, _ccs.ToArray, _accs.ToArray)
            Else
                update = WebS.RequestUpdate(_Password)
            End If

            dl.AccountCodeCount = _accs.Count
            dl.CostCenterCount = _ccs.Count
            d.SubmitChanges()


            _accs = Nothing
            _ccs = Nothing

            If (update.Status = "Nothing to do") Then
                ' EventLog1.WriteEntry("AgapeConnect service responded: 'Nothing to do'.")
                Return
            End If

            If (update.Status = "New Data") Then
                ' EventLog1.WriteEntry("AgapeConnect service found new data.")
                If Not update.WebUsers Is Nothing Then
                    Try
                        ProcessTNTWebUsers(update.WebUsers)
                    Catch ex As Exception
                        uResp.TntStatus.Status = "Error"
                        uResp.TntStatus.Message = ex.Message
                    End Try
                    uResp.TntStatus.Status = "OK"
                End If
                If Not update.Rmbs Is Nothing Then
                    ProcessRmbs(update.Rmbs, update.AcctsReceivable, update.AcctsPayable, uResp)
                End If


            End If

            'Send Responses back to webservice
            WebS.UpdateResponses(_Password, uResp)


            update = Nothing

            dt.Dispose()
            ds.Dispose()

        Catch ex As System.Exception
            '  EventLog1.WriteEntry("AgapeConnect encountered an error '" & _
            '      ex.Message & "'", EventLogEntryType.Error)
            ' EventLog1.WriteEntry("AgapeConnect service Stack Trace: " & _
            '     ex.StackTrace, EventLogEntryType.Error)
        End Try
    End Sub
#End Region

#Region "tntDataserver Interface"
    Private Sub ProcessTNTWebUsers(ByVal WebUsers As dynamicAgapeConnect.WebUser())
        'EventLog1.WriteEntry("Processing TNT Data")
        Dim dAC As New ListenerDataContext
        dAC.WebUserCostCenters.DeleteAllOnSubmit(dAC.WebUserCostCenters)
        dAC.WebUsers.DeleteAllOnSubmit(dAC.WebUsers)
        dAC.SubmitChanges()

        For Each row In WebUsers
            'Blank the interim tables
            Dim insert As New Listener.WebUser
            insert.UserName = row.Name
            insert.Email = row.GCXUserName
            insert.DefaultRegistrationCode = row.GCXUserName
            dAC.WebUsers.InsertOnSubmit(insert)
            For Each profile In row.PersonalAccounts
                Dim insertCC As New Listener.WebUserCostCenter
                insertCC.CostCenterCode = profile.CostCenter
                insertCC.ProfileCode = 0
                insertCC.ProfileDescription = "Personal Accounts"
                insertCC.UserName = row.Name
                dAC.WebUserCostCenters.InsertOnSubmit(insertCC)
                Dim insertCC2 As New Listener.WebUserCostCenter
                insertCC2.CostCenterCode = profile.CostCenter & "-Advances"
                insertCC2.ProfileCode = 0
                insertCC2.ProfileDescription = "Personal Accounts"
                insertCC2.UserName = row.Name
                dAC.WebUserCostCenters.InsertOnSubmit(insertCC2)
            Next
            For Each profile In row.DepartmentAccounts
                Dim insertCC As New Listener.WebUserCostCenter
                insertCC.CostCenterCode = profile.CostCenter
                insertCC.ProfileCode = 1
                insertCC.ProfileDescription = "Department Accounts"
                insertCC.UserName = row.Name

                dAC.WebUserCostCenters.InsertOnSubmit(insertCC)
            Next

            For Each profile In row.TeamAccounts
                Dim insertCC As New Listener.WebUserCostCenter
                insertCC.CostCenterCode = profile.CostCenter
                insertCC.ProfileCode = 2
                insertCC.ProfileDescription = "Team Accounts"
                insertCC.UserName = row.Name

                dAC.WebUserCostCenters.InsertOnSubmit(insertCC)
            Next
        Next

        dAC.SubmitChanges()


    End Sub
#End Region
#Region "Solomon Interface"
    Private Function GetNewBatchNumber() As Integer
        Dim ds As New SolomonDataContext
        Dim q = ds.GLSetups.First
        q.LastBatNbr = CInt(q.LastBatNbr + 1)
        ds.SubmitChanges()
        Return CInt(ZeroFill(q.LastBatNbr, 6))
    End Function

    Private Function ValidateRmb(ByVal theRmb As dynamicAgapeConnect.Rmb, ByRef inStatus As dynamicAgapeConnect.StatusDescription) As Boolean
        'Check if Rmb has already been processed (and not deleted)

        Dim q = From c In ds.GLTrans Join b In ds.Batches On c.BatNbr Equals b.BatNbr Where c.RefNbr = "R" & ZeroFill(theRmb.RID, 7) Select c.BatNbr, b.Status
        If q.Count > 0 Then
            inStatus.Status = False
            inStatus.Message = "Reimbursement has already been inserted into Solomon in Batch: " & q.First.BatNbr
        End If
        Return True
    End Function

    Private Sub ProcessRmbs(ByVal Rmbs As dynamicAgapeConnect.Rmb(), ByVal AccsReceivable As String, ByVal AccsPayable As String, ByRef uResp As dynamicAgapeConnect.UpdateResponse)

        ' EventLog1.WriteEntry("Processing Reimbursments")
        Dim rtn As New List(Of dynamicAgapeConnect.StatusDescription)
        
        ' Dim dS As New SolomonDataContext
        Dim BlankDate = New Date(1900, 1, 1)
        ' Dim PeriodStr = "YYYYMM"    ' Need to get this for the Fiscal period from the GLSetup table
        Dim BatNumber As Integer = GetNewBatchNumber()
        Dim TotalAmt = (From c In Rmbs Select CDbl(c.Lines.Max(Function(p) p.GrossAmount))).Max



        Dim batchHeader As New Batch
        batchHeader.Acct = ""
        batchHeader.AutoRev = 0
        batchHeader.AutoRevCopy = 0
        batchHeader.BalanceType = "A"
        batchHeader.BankAcct = ""
        batchHeader.BankSub = ""
        batchHeader.BaseCuryID = ds.GLSetups.First.BaseCuryId
        batchHeader.BatNbr = BatNumber
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
        batchHeader.OrigCpnyID = ""
        batchHeader.OrigScrnNbr = ""
        batchHeader.PerEnt = ds.GLSetups.First.PerNbr
        batchHeader.PerPost = ds.GLSetups.First.PerNbr
        batchHeader.Rlsed = 1
        batchHeader.Status = "U"
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
                rmbStatus.Status = "ERROR"
                rmbStatus.Message = "This Combination of Account and Responseibility Center is not valid."
                rtn.Add(rmbStatus)
            Else
                'Do the Transactions
                Dim ref = theRmb.UserInitials & "-"
                For Each Line In theRmb.Lines
                    ts.Add(createTransaction(BatNumber, i, ds.GLSetups.First, theRmb.RID, Line.CostCenter, Line.AccountCode, ref & Line.Comment, IIf(Line.GrossAmount > 0, 0, Line.GrossAmount), IIf(Line.GrossAmount > 0, Line.GrossAmount, 0), theRmb.Period, theRmb.Year, theRmb.ProcessDate))
                    i += 1
                Next
                Dim RmbTotal As Double = (From c In theRmb.Lines Select c.GrossAmount).Sum
                Dim rmbAdvance As Double = theRmb.AdvanceRequest


                RmbTotal -= rmbAdvance
                If RmbTotal <> 0 Then
                    ts.Add(createTransaction(BatNumber, i, ds.GLSetups.First, theRmb.RID, theRmb.PersonalCostCenter, AccsPayable, ref & "-Payment for R" & theRmb.RmbNo, IIf(RmbTotal, RmbTotal, 0), IIf(RmbTotal > 0, 0, RmbTotal), 12, 2012, theRmb.ProcessDate))
                    i += 1
                End If
                If rmbAdvance <> 0 Then
                    ts.Add(createTransaction(BatNumber, i, ds.GLSetups.First, theRmb.RID, theRmb.PersonalCostCenter, AccsReceivable, ref & "-Pay off advance with R" & theRmb.RmbNo, IIf(rmbAdvance > 0, rmbAdvance, 0), IIf(rmbAdvance > 0, 0, rmbAdvance), 12, 2012, theRmb.ProcessDate))
                    i += 1
                End If
                rmbStatus.Status = "SUCCESS"
                rtn.Add(rmbStatus)
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
    Private Function createTransaction(ByVal BatchNo As Integer, ByVal LineNumber As Integer, ByVal settings As GLSetup, ByVal RID As String, ByVal CostCenter As String, ByVal AccountCode As String, ByVal Description As String, ByVal Credit As String, ByVal Debit As String, ByVal FPeriod As String, ByVal FYear As String, ByVal TransDate As Date)
        Dim BlankDate = New Date(1900, 1, 1)

        Dim insert As New GLTran
        insert.Acct = AccountCode
        insert.AppliedDate = BlankDate
        insert.BalanceType = "A"
        insert.BaseCuryID = settings.BaseCuryId
        insert.BatNbr = BatchNo
        insert.CpnyID = settings.CpnyId
        insert.CrAmt = Credit
        insert.Crtd_DateTime = TransDate   '????
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
        insert.FiscYr = settings.BegFiscalYr
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
        insert.OrigCpnyID = ""
        insert.OrigSub = ""
        insert.PC_Flag = ""
        insert.PC_ID = ""
        insert.PC_Status = ""
        insert.PerEnt = settings.PerNbr
        insert.PerPost = settings.PerNbr
        insert.Posted = "U"
        insert.ProjectID = ""
        insert.Qty = 0
        insert.RefNbr = "R" & ZeroFill(RID, 7)
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

        Return insert




    End Function
    Private Function UpdateAccountsCodes(ByRef _ccs As List(Of dynamicAgapeConnect.ACUnit), ByRef _accs As List(Of dynamicAgapeConnect.ACUnit)) As Boolean
        Dim ccs = From c In ds.SubAccts Where c.Active
        Dim accs = From c In ds.Accounts Where c.Active


        If dl.CostCenterCount <> _ccs.Count Or dl.AccountCodeCount <> accs.Count Then
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
        If Right(CC, 4) = "0000" Then
            Return 2 ' Other
        ElseIf CC(2) = "1" Or CC(2) = "2" Then
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


#End Region

   
   

  

   
    
   



    


   
End Class
