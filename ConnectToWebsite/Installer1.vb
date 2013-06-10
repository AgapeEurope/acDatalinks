Imports System.ComponentModel
Imports System.Configuration.Install

Public Class Installer1

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add initialization code after the call to InitializeComponent
        Dim s As New Solomon()
        s.BringToFront()

        s.Show()



    End Sub


    'Public Overrides Sub Install(ByVal stateSaver As  _
    'System.Collections.IDictionary)

    '    MyBase.Install(stateSaver)
    '    Dim s As New Solomon()
    '    s.Show()

    'End Sub
End Class
