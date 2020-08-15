Imports Microsoft.Win32

Public Class FormGUImanager

#Region "Declarations"
    'Use for registry
    Private regKey As RegistryKey
#End Region

#Region "Methods"
    Private Sub FormGUImanager_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Loadguis()

    End Sub

    Private Sub Form1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        Dim f As New Font("Comic Sans Ms", 12, FontStyle.Italic)
        Dim br As New SolidBrush(Color.BlueViolet)
        Dim pt As New Point(446, 410)
        e.Graphics.DrawString("Copyright Martin Laflamme 2004/2020", f, br, pt)
    End Sub

    Private Sub ListViewGUI_MouseUp(sender As Object, e As MouseEventArgs) Handles ListViewGUI.MouseUp

        If e.Button = MouseButtons.Right Then
            If ListViewGUI.SelectedItems.Count > 0 Then
                Dim status As String = ListViewGUI.SelectedItems(0).SubItems(5).Text
                If status = "Activated" Then
                    ContextMenuStrip3.Show(ListViewGUI, e.Location)
                Else
                    ContextMenuStrip1.Show(ListViewGUI, e.Location)
                End If

            Else
                ContextMenuStrip2.Show(ListViewGUI, e.Location)
            End If
        End If
    End Sub
#End Region

#Region "ContextMenu"

    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        DialogNew.ShowDialog()
    End Sub

    Private Sub NewToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem1.Click
        DialogNew.ShowDialog()
    End Sub

    Private Sub NewToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem2.Click
        DialogNew.ShowDialog()
    End Sub

    Private Sub ActivateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ActivateToolStripMenuItem.Click
        Dim GUIName As String = ListViewGUI.SelectedItems(0).Text
        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS\" & GUIName, True)
        If regKey IsNot Nothing Then
            regKey.SetValue("Status", 1)
            ListViewGUI.SelectedItems(0).SubItems(5).Text = "Activated"
        End If
    End Sub

    Private Sub DeactivateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeactivateToolStripMenuItem.Click
        Dim GUIName As String = ListViewGUI.SelectedItems(0).Text
        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS\" & GUIName, True)
        If regKey IsNot Nothing Then
            regKey.SetValue("Status", 0)
            ListViewGUI.SelectedItems(0).SubItems(5).Text = "Deactivated"
        End If
    End Sub

#End Region

#Region "Buttons"

    Private Sub ButtonClose_Click(sender As Object, e As EventArgs) Handles ButtonClose.Click
        Close()
    End Sub

#End Region

#Region "Subroutines"
    Private Sub Loadguis()
        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS", True)
        If regKey IsNot Nothing Then
            For Each Name As String In regKey.GetSubKeyNames
                regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS\" & Name, True)
                Dim Comd As String = regKey.GetValue("Command")
                Dim input As String = regKey.GetValue("Input")
                Dim output As String = regKey.GetValue("Output")
                Dim path As String = regKey.GetValue("Path")
                Dim status As Integer = regKey.GetValue("Status")
                Dim stat As String = ""
                If status = 0 Then
                    stat = "Deactivated"
                Else
                    stat = "Activated"
                End If

                'Add to listview
                Dim Lvi As ListViewItem
                Lvi = ListViewGUI.Items.Add(Name)
                With Lvi
                    .SubItems.Add(input)
                    .SubItems.Add(path)
                    .SubItems.Add(Comd)
                    .SubItems.Add(output)
                    .SubItems.Add(stat)
                End With
            Next
        End If
    End Sub

#End Region




End Class