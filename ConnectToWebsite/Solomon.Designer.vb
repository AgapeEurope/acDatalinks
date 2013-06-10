<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Solomon
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
        Me.SqlConnectionUIControl1 = New Microsoft.Data.ConnectionUI.SqlConnectionUIControl()
        Me.SuspendLayout()
        '
        'SqlConnectionUIControl1
        '
        Me.SqlConnectionUIControl1.Location = New System.Drawing.Point(118, 13)
        Me.SqlConnectionUIControl1.Margin = New System.Windows.Forms.Padding(0)
        Me.SqlConnectionUIControl1.MinimumSize = New System.Drawing.Size(350, 360)
        Me.SqlConnectionUIControl1.Name = "SqlConnectionUIControl1"
        Me.SqlConnectionUIControl1.Size = New System.Drawing.Size(350, 360)
        Me.SqlConnectionUIControl1.TabIndex = 0
        '
        'Solomon
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(586, 379)
        Me.Controls.Add(Me.SqlConnectionUIControl1)
        Me.Name = "Solomon"
        Me.Text = "Solomon"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SqlConnectionUIControl1 As Microsoft.Data.ConnectionUI.SqlConnectionUIControl
End Class
