Imports System.Windows.Forms
Imports Microsoft.Win32

Public Class DialogNew

    'Use for registry
    Private regKey As RegistryKey

    Dim N As String
    Dim I As String
    Dim p As String
    Dim C As String
    Dim O As String
    Dim ed As Boolean
    Dim ListviewGUI As ListView

    'Constructor
    Public Sub New(ByVal Name As String, ByVal Input As String, ByVal Path As String, Com As String, Output As String, Edit As Boolean, lv As ListView)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        N = Name
        I = Input
        p = Path
        C = Com
        O = Output
        ed = Edit
        ListviewGUI = lv

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

    Private Sub ButtonBrowse_Click(sender As Object, e As EventArgs) Handles ButtonBrowse.Click
        OpenFileDialog1.DefaultExt = "exe"
        OpenFileDialog1.Filter = "Console App File|*.exe|All files|*.*"
        OpenFileDialog1.Title = "Open Console App File ..."
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.ShowDialog()
        TextBox4.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        Close()
    End Sub

    Private Sub ButtonNewEdit_Click(sender As Object, e As EventArgs) Handles ButtonNewEdit.Click

        Dim Input As String = TextBox1.Text
        Dim Output As String = TextBox2.Text
        Dim Name As String = TextBox3.Text
        Dim Path As String = TextBox4.Text
        Dim Com As String = TextBox5.Text

        'New Item
        If ed = False Then
            If TextBox1.Text <> "" And TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" Then
                regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS", True)
                If regKey Is Nothing Then
                    regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI", True)
                    regKey.CreateSubKey("GUIS")
                    regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS", True)
                End If

                regKey.CreateSubKey(Name)
                regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS\" & Name, True)
                regKey.SetValue("Command", Com)
                regKey.SetValue("Input", Input)
                regKey.SetValue("Output", Output)
                regKey.SetValue("Path", Path)
                regKey.SetValue("Status", 0)

                Dim Lvi As ListViewItem
                Lvi = ListViewGUI.Items.Add(Name)
                With Lvi
                    .SubItems.Add(Input)
                    .SubItems.Add(Path)
                    .SubItems.Add(Com)
                    .SubItems.Add(Output)
                    .SubItems.Add("Deactivated")
                End With
                Close()
            Else
                MessageBox.Show("You must fill all the cases")
            End If
            'Edit Item
        Else
            If TextBox1.Text <> "" And TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" Then
                'The Name already exist
                If Name = ListviewGUI.SelectedItems(0).Text Then
                    regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS\" & Name, True)
                    regKey.SetValue("Command", Com)
                    regKey.SetValue("Input", Input)
                    regKey.SetValue("Output", Output)
                    regKey.SetValue("Path", Path)

                    ListviewGUI.SelectedItems(0).SubItems(1).Text = Input
                    ListviewGUI.SelectedItems(0).SubItems(2).Text = Path
                    ListviewGUI.SelectedItems(0).SubItems(3).Text = Com
                    ListviewGUI.SelectedItems(0).SubItems(4).Text = Output
                    Close()
                Else
                    regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS\" & ListviewGUI.SelectedItems(0).Text, True)
                    Dim Status As Integer = regKey.GetValue("Status")
                    regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS", True)
                    regKey.CreateSubKey(Name)
                    regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS\" & Name, True)
                    regKey.SetValue("Command", Com)
                    regKey.SetValue("Input", Input)
                    regKey.SetValue("Output", Output)
                    regKey.SetValue("Path", Path)
                    regKey.SetValue("Status", Status)
                    regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS", True)
                    regKey.DeleteSubKey(ListviewGUI.SelectedItems(0).Text)

                    Dim stat As String = ""
                    If Status = 0 Then
                        stat = "Deactivated"
                    Else
                        stat = "Activated"
                    End If

                    Dim Lvi As ListViewItem
                    Lvi = ListviewGUI.Items.Add(Name)
                    With Lvi
                        .SubItems.Add(Input)
                        .SubItems.Add(Path)
                        .SubItems.Add(Com)
                        .SubItems.Add(Output)
                        .SubItems.Add(stat)
                    End With

                    Lvi = ListviewGUI.FindItemWithText(ListviewGUI.SelectedItems(0).Text)
                    Lvi.Remove()
                    Close()
                End If

            Else
                MessageBox.Show("You must fill all the cases")
            End If
        End If
    End Sub
End Class
