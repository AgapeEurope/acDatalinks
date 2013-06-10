<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim PictureBox1 As System.Windows.Forms.PictureBox
        Dim PictureBox2 As System.Windows.Forms.PictureBox
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Config))
        Me.DatalinksBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.AcDatalinks = New AgapeConnectDatapump.acDatalinks()
        Me.DataRepeater1 = New Microsoft.VisualBasic.PowerPacks.DataRepeater()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lblDatalinkId = New System.Windows.Forms.Label()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnSync = New System.Windows.Forms.Button()
        Me.btnEnableDisable = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblLastRun = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.LinkLabel3 = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.btnEditDonorwise = New System.Windows.Forms.LinkLabel()
        Me.webURL = New System.Windows.Forms.Label()
        Me.solCon = New System.Windows.Forms.Label()
        Me.dwCon = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TableAdapterManager = New AgapeConnectDatapump.acDatalinksTableAdapters.TableAdapterManager()
        Me.DatalinksTableAdapter = New AgapeConnectDatapump.acDatalinksTableAdapters.DatalinksTableAdapter()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnAddNew = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblServiceStatus = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        PictureBox1 = New System.Windows.Forms.PictureBox()
        PictureBox2 = New System.Windows.Forms.PictureBox()
        CType(PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DatalinksBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AcDatalinks, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DataRepeater1.ItemTemplate.SuspendLayout()
        Me.DataRepeater1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        PictureBox1.DataBindings.Add(New System.Windows.Forms.Binding("Visible", Me.DatalinksBindingSource, "Active", True))
        PictureBox1.Image = Global.AgapeConnectDatapump.My.Resources.Resources.checkmark
        PictureBox1.ImageLocation = ""
        PictureBox1.InitialImage = Nothing
        PictureBox1.Location = New System.Drawing.Point(11, 23)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New System.Drawing.Size(50, 50)
        PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        PictureBox1.TabIndex = 26
        PictureBox1.TabStop = False
        '
        'DatalinksBindingSource
        '
        Me.DatalinksBindingSource.DataMember = "Datalinks"
        Me.DatalinksBindingSource.DataSource = Me.AcDatalinks
        '
        'AcDatalinks
        '
        Me.AcDatalinks.DataSetName = "acDatalinks"
        Me.AcDatalinks.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'PictureBox2
        '
        PictureBox2.DataBindings.Add(New System.Windows.Forms.Binding("Visible", Me.DatalinksBindingSource, "Error", True))
        PictureBox2.Image = Global.AgapeConnectDatapump.My.Resources.Resources.warning
        PictureBox2.ImageLocation = ""
        PictureBox2.InitialImage = Nothing
        PictureBox2.Location = New System.Drawing.Point(11, 22)
        PictureBox2.Name = "PictureBox2"
        PictureBox2.Size = New System.Drawing.Size(50, 50)
        PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        PictureBox2.TabIndex = 28
        PictureBox2.TabStop = False
        PictureBox2.Visible = False
        AddHandler PictureBox2.Click, AddressOf Me.PictureBox2_Click
        '
        'DataRepeater1
        '
        Me.DataRepeater1.DataBindings.Add(New System.Windows.Forms.Binding("Tag", Me.DatalinksBindingSource, "DatalinkId", True))
        Me.DataRepeater1.ItemHeaderVisible = False
        '
        'DataRepeater1.ItemTemplate
        '
        Me.DataRepeater1.ItemTemplate.BackColor = System.Drawing.Color.White
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.Label9)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.lblDatalinkId)
        Me.DataRepeater1.ItemTemplate.Controls.Add(PictureBox2)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.btnDelete)
        Me.DataRepeater1.ItemTemplate.Controls.Add(PictureBox1)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.btnSync)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.btnEnableDisable)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.Label8)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.lblLastRun)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.lblStatus)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.LinkLabel3)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.LinkLabel2)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.btnEditDonorwise)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.webURL)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.solCon)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.dwCon)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.Label4)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.Label3)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.Label2)
        Me.DataRepeater1.ItemTemplate.Controls.Add(Me.Label1)
        Me.DataRepeater1.ItemTemplate.Size = New System.Drawing.Size(814, 84)
        Me.DataRepeater1.Location = New System.Drawing.Point(12, 12)
        Me.DataRepeater1.Name = "DataRepeater1"
        Me.DataRepeater1.Size = New System.Drawing.Size(822, 488)
        Me.DataRepeater1.TabIndex = 1
        Me.DataRepeater1.Text = "DataRepeater1"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label9.Location = New System.Drawing.Point(400, 2)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(58, 13)
        Me.Label9.TabIndex = 30
        Me.Label9.Text = "DatalinkId:"
        '
        'lblDatalinkId
        '
        Me.lblDatalinkId.AutoSize = True
        Me.lblDatalinkId.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DatalinksBindingSource, "DatalinkId", True))
        Me.lblDatalinkId.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblDatalinkId.Location = New System.Drawing.Point(455, 2)
        Me.lblDatalinkId.Name = "lblDatalinkId"
        Me.lblDatalinkId.Size = New System.Drawing.Size(19, 13)
        Me.lblDatalinkId.TabIndex = 29
        Me.lblDatalinkId.Text = "00"
        '
        'btnDelete
        '
        Me.btnDelete.AutoSize = True
        Me.btnDelete.BackColor = System.Drawing.SystemColors.Control
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Location = New System.Drawing.Point(722, 51)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(60, 23)
        Me.btnDelete.TabIndex = 27
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnSync
        '
        Me.btnSync.AutoSize = True
        Me.btnSync.BackColor = System.Drawing.SystemColors.Control
        Me.btnSync.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSync.Location = New System.Drawing.Point(722, 27)
        Me.btnSync.Name = "btnSync"
        Me.btnSync.Size = New System.Drawing.Size(60, 23)
        Me.btnSync.TabIndex = 24
        Me.btnSync.Text = "Sync"
        Me.btnSync.UseVisualStyleBackColor = False
        '
        'btnEnableDisable
        '
        Me.btnEnableDisable.AutoSize = True
        Me.btnEnableDisable.BackColor = System.Drawing.SystemColors.Control
        Me.btnEnableDisable.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DatalinksBindingSource, "Command", True))
        Me.btnEnableDisable.DataBindings.Add(New System.Windows.Forms.Binding("Tag", Me.DatalinksBindingSource, "DatalinkId", True))
        Me.btnEnableDisable.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEnableDisable.Location = New System.Drawing.Point(722, 3)
        Me.btnEnableDisable.Name = "btnEnableDisable"
        Me.btnEnableDisable.Size = New System.Drawing.Size(60, 25)
        Me.btnEnableDisable.TabIndex = 23
        Me.btnEnableDisable.Text = "Disable"
        Me.btnEnableDisable.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label8.Location = New System.Drawing.Point(532, 2)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(53, 13)
        Me.Label8.TabIndex = 22
        Me.Label8.Text = "Last Run:"
        '
        'lblLastRun
        '
        Me.lblLastRun.AutoSize = True
        Me.lblLastRun.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DatalinksBindingSource, "LastSync", True))
        Me.lblLastRun.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblLastRun.Location = New System.Drawing.Point(586, 2)
        Me.lblLastRun.Name = "lblLastRun"
        Me.lblLastRun.Size = New System.Drawing.Size(98, 13)
        Me.lblLastRun.TabIndex = 21
        Me.lblLastRun.Text = " 01/01/2012 11:42"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DatalinksBindingSource, "Status", True))
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblStatus.Location = New System.Drawing.Point(483, 2)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(46, 13)
        Me.lblStatus.TabIndex = 20
        Me.lblStatus.Text = "Enabled"
        '
        'LinkLabel3
        '
        Me.LinkLabel3.AutoSize = True
        Me.LinkLabel3.Location = New System.Drawing.Point(661, 58)
        Me.LinkLabel3.Name = "LinkLabel3"
        Me.LinkLabel3.Size = New System.Drawing.Size(25, 13)
        Me.LinkLabel3.TabIndex = 19
        Me.LinkLabel3.TabStop = True
        Me.LinkLabel3.Text = "Edit"
        '
        'LinkLabel2
        '
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.Location = New System.Drawing.Point(661, 41)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(25, 13)
        Me.LinkLabel2.TabIndex = 18
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "Edit"
        '
        'btnEditDonorwise
        '
        Me.btnEditDonorwise.AutoSize = True
        Me.btnEditDonorwise.Location = New System.Drawing.Point(661, 26)
        Me.btnEditDonorwise.Name = "btnEditDonorwise"
        Me.btnEditDonorwise.Size = New System.Drawing.Size(25, 13)
        Me.btnEditDonorwise.TabIndex = 17
        Me.btnEditDonorwise.TabStop = True
        Me.btnEditDonorwise.Text = "Edit"
        '
        'webURL
        '
        Me.webURL.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DatalinksBindingSource, "webURL", True))
        Me.webURL.Location = New System.Drawing.Point(157, 58)
        Me.webURL.Name = "webURL"
        Me.webURL.Size = New System.Drawing.Size(500, 12)
        Me.webURL.TabIndex = 16
        Me.webURL.Text = "Label7"
        '
        'solCon
        '
        Me.solCon.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DatalinksBindingSource, "solConnectionString", True))
        Me.solCon.Location = New System.Drawing.Point(157, 41)
        Me.solCon.Name = "solCon"
        Me.solCon.Size = New System.Drawing.Size(500, 17)
        Me.solCon.TabIndex = 15
        Me.solCon.Text = "Label6"
        '
        'dwCon
        '
        Me.dwCon.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DatalinksBindingSource, "dsConnectionString", True))
        Me.dwCon.Location = New System.Drawing.Point(157, 26)
        Me.dwCon.Name = "dwCon"
        Me.dwCon.Size = New System.Drawing.Size(500, 15)
        Me.dwCon.TabIndex = 14
        Me.dwCon.Text = "Label5"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(76, 57)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(74, 13)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Web Portal:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(91, 41)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 13)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "Solomon:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(77, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Donorwise:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.DatalinksBindingSource, "Name", True))
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(1, -1)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 20)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "DevTest"
        '
        'TableAdapterManager
        '
        Me.TableAdapterManager.BackupDataSetBeforeUpdate = False
        Me.TableAdapterManager.DatalinksTableAdapter = Me.DatalinksTableAdapter
        Me.TableAdapterManager.UpdateOrder = AgapeConnectDatapump.acDatalinksTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete
        '
        'DatalinksTableAdapter
        '
        Me.DatalinksTableAdapter.ClearBeforeFill = True
        '
        'btnRefresh
        '
        Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefresh.BackColor = System.Drawing.SystemColors.Control
        Me.btnRefresh.Location = New System.Drawing.Point(755, 506)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 18
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'btnAddNew
        '
        Me.btnAddNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddNew.BackColor = System.Drawing.SystemColors.Control
        Me.btnAddNew.Location = New System.Drawing.Point(665, 506)
        Me.btnAddNew.Name = "btnAddNew"
        Me.btnAddNew.Size = New System.Drawing.Size(75, 23)
        Me.btnAddNew.TabIndex = 19
        Me.btnAddNew.Text = "Add New"
        Me.btnAddNew.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(14, 503)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 13)
        Me.Label5.TabIndex = 20
        Me.Label5.Text = "Status:"
        '
        'lblServiceStatus
        '
        Me.lblServiceStatus.AutoSize = True
        Me.lblServiceStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServiceStatus.Location = New System.Drawing.Point(57, 503)
        Me.lblServiceStatus.Name = "lblServiceStatus"
        Me.lblServiceStatus.Size = New System.Drawing.Size(43, 13)
        Me.lblServiceStatus.TabIndex = 21
        Me.lblServiceStatus.Text = "Status"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Gray
        Me.Label6.Location = New System.Drawing.Point(14, 519)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(315, 13)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "Use Windows Services to start and stop the ACDatalinks service."
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.Location = New System.Drawing.Point(566, 506)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 23
        Me.Button1.Text = "Settings..."
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Config
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(846, 541)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lblServiceStatus)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnAddNew)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.DataRepeater1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Config"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Agape Connect (BETA) - Datalink Manager"
        CType(PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DatalinksBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AcDatalinks, System.ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DataRepeater1.ItemTemplate.ResumeLayout(False)
        Me.DataRepeater1.ItemTemplate.PerformLayout()
        Me.DataRepeater1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AcDatalinks As AgapeConnectDatapump.acDatalinks
    Friend WithEvents DatalinksBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DataRepeater1 As Microsoft.VisualBasic.PowerPacks.DataRepeater
    Friend WithEvents TableAdapterManager As AgapeConnectDatapump.acDatalinksTableAdapters.TableAdapterManager
    Friend WithEvents LinkLabel3 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents btnEditDonorwise As System.Windows.Forms.LinkLabel
    Friend WithEvents webURL As System.Windows.Forms.Label
    Friend WithEvents solCon As System.Windows.Forms.Label
    Friend WithEvents dwCon As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblLastRun As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnEnableDisable As System.Windows.Forms.Button
    Friend WithEvents btnSync As System.Windows.Forms.Button
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents btnAddNew As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblServiceStatus As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents DatalinksTableAdapter As AgapeConnectDatapump.acDatalinksTableAdapters.DatalinksTableAdapter
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lblDatalinkId As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
End Class
