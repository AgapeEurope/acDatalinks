<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Settings
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnTntDownload = New System.Windows.Forms.LinkLabel()
        Me.btnACDatalinkDownload = New System.Windows.Forms.LinkLabel()
        Me.tbTntPath = New System.Windows.Forms.TextBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.tbPollDelay = New System.Windows.Forms.NumericUpDown()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.tbACDatalinksVersion = New System.Windows.Forms.TextBox()
        Me.tbTntVersion = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        CType(Me.tbPollDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(126, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "ACDatalinks Version:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(138, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "TntDataserver Version:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 76)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(160, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "TntDataserver Install Path:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 105)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(143, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Poll Delay (in Seconds):"
        '
        'btnTntDownload
        '
        Me.btnTntDownload.AutoSize = True
        Me.btnTntDownload.Location = New System.Drawing.Point(369, 51)
        Me.btnTntDownload.Name = "btnTntDownload"
        Me.btnTntDownload.Size = New System.Drawing.Size(131, 13)
        Me.btnTntDownload.TabIndex = 4
        Me.btnTntDownload.TabStop = True
        Me.btnTntDownload.Text = "TntDataserver Downloads"
        '
        'btnACDatalinkDownload
        '
        Me.btnACDatalinkDownload.AutoSize = True
        Me.btnACDatalinkDownload.Location = New System.Drawing.Point(369, 25)
        Me.btnACDatalinkDownload.Name = "btnACDatalinkDownload"
        Me.btnACDatalinkDownload.Size = New System.Drawing.Size(121, 13)
        Me.btnACDatalinkDownload.TabIndex = 5
        Me.btnACDatalinkDownload.TabStop = True
        Me.btnACDatalinkDownload.Text = "ACDatalinks Downloads"
        '
        'tbTntPath
        '
        Me.tbTntPath.BackColor = System.Drawing.SystemColors.ControlLight
        Me.tbTntPath.Enabled = False
        Me.tbTntPath.Location = New System.Drawing.Point(171, 73)
        Me.tbTntPath.Name = "tbTntPath"
        Me.tbTntPath.Size = New System.Drawing.Size(450, 20)
        Me.tbTntPath.TabIndex = 6
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(627, 71)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(64, 23)
        Me.btnBrowse.TabIndex = 7
        Me.btnBrowse.Text = "Browse..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'tbPollDelay
        '
        Me.tbPollDelay.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.tbPollDelay.Location = New System.Drawing.Point(171, 103)
        Me.tbPollDelay.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.tbPollDelay.Name = "tbPollDelay"
        Me.tbPollDelay.Size = New System.Drawing.Size(93, 20)
        Me.tbPollDelay.TabIndex = 8
        Me.tbPollDelay.ThousandsSeparator = True
        Me.tbPollDelay.Value = New Decimal(New Integer() {300, 0, 0, 0})
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(616, 127)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 9
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(533, 127)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'tbACDatalinksVersion
        '
        Me.tbACDatalinksVersion.BackColor = System.Drawing.SystemColors.ControlLight
        Me.tbACDatalinksVersion.Enabled = False
        Me.tbACDatalinksVersion.Location = New System.Drawing.Point(171, 22)
        Me.tbACDatalinksVersion.Name = "tbACDatalinksVersion"
        Me.tbACDatalinksVersion.Size = New System.Drawing.Size(192, 20)
        Me.tbACDatalinksVersion.TabIndex = 11
        '
        'tbTntVersion
        '
        Me.tbTntVersion.BackColor = System.Drawing.SystemColors.ControlLight
        Me.tbTntVersion.Enabled = False
        Me.tbTntVersion.Location = New System.Drawing.Point(171, 48)
        Me.tbTntVersion.Name = "tbTntVersion"
        Me.tbTntVersion.Size = New System.Drawing.Size(192, 20)
        Me.tbTntVersion.TabIndex = 12
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.DefaultExt = "exe"
        Me.OpenFileDialog1.Filter = "TntMPD.DataServer.exe|TntMPD.DataServer.exe"
        Me.OpenFileDialog1.ReadOnlyChecked = True
        Me.OpenFileDialog1.SupportMultiDottedExtensions = True
        Me.OpenFileDialog1.Title = "TntDataserver Install Location (TntMPD.DataServer.exe)"
        '
        'Settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(703, 159)
        Me.Controls.Add(Me.tbTntVersion)
        Me.Controls.Add(Me.tbACDatalinksVersion)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.tbPollDelay)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.tbTntPath)
        Me.Controls.Add(Me.btnACDatalinkDownload)
        Me.Controls.Add(Me.btnTntDownload)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Settings"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ACDatalinks - Settings"
        CType(Me.tbPollDelay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnTntDownload As System.Windows.Forms.LinkLabel
    Friend WithEvents btnACDatalinkDownload As System.Windows.Forms.LinkLabel
    Friend WithEvents tbTntPath As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents tbPollDelay As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents tbACDatalinksVersion As System.Windows.Forms.TextBox
    Friend WithEvents tbTntVersion As System.Windows.Forms.TextBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
End Class
