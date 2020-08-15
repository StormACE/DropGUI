Imports System.Windows.Forms

Public Class DialogNew

    Dim N As String
    Dim I As String
    Dim p As String
    Dim C As String
    Dim O As String
    Dim ed As Boolean

    'Constructor
    Public Sub New(ByVal Name As String, ByVal Input As String, ByVal Path As String, Com As String, Output As String, Edit As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        N = Name
        I = Input
        p = Path
        C = Com
        O = Output
        ed = Edit

    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        Close()
    End Sub

    Private Sub DialogNew_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If ed = True Then
            Text = "Edit"
            TextBox1.Text = I
            TextBox2.Text = O
            TextBox3.Text = N
            TextBox4.Text = p
            TextBox5.Text = C
        End If
    End Sub
End Class
