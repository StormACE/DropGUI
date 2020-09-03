Imports System.IO
Imports System.Text
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
        Dim br As New SolidBrush(Color.DarkBlue)
        Dim pt As New Point(446, 410)
        e.Graphics.DrawString("Copyright Martin Laflamme 2004/2020", f, br, pt)
        pt = New Point(446, 430)
        e.Graphics.DrawString("Version: " & Application.ProductVersion, f, br, pt)
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

    Private Sub ListViewGUI_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListViewGUI.MouseDoubleClick
        If e.Button = MouseButtons.Left Then
            LaunchEditDialogue()
        End If
    End Sub
#End Region

#Region "ContextMenu"

    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        Dim Newdia As New DialogNew("", "", "", "", "", False, ListViewGUI)
        Newdia.ShowDialog()
        Newdia.Dispose()
    End Sub

    Private Sub NewToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem1.Click
        Dim Newdia As New DialogNew("", "", "", "", "", False, ListViewGUI)
        Newdia.ShowDialog()
        Newdia.Dispose()
    End Sub

    Private Sub NewToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem2.Click
        Dim Newdia As New DialogNew("", "", "", "", "", False, ListViewGUI)
        Newdia.ShowDialog()
        Newdia.Dispose()
    End Sub

    Private Sub ActivateToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ActivateToolStripMenuItem.Click
        Dim GUIName As String = ListViewGUI.SelectedItems(0).Text
        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS\" & GUIName, True)
        If regKey IsNot Nothing Then
            regKey.SetValue("Status", 1)
            ListViewGUI.SelectedItems(0).SubItems(5).Text = "Activated"
            DeactivateAll(GUIName)
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

    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        LaunchEditDialogue()
    End Sub

    Private Sub EditToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem1.Click
        LaunchEditDialogue()
    End Sub

    Private Sub CloneToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles CloneToolStripMenuItem2.Click
        CloneItem()
    End Sub

    Private Sub CloneToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloneToolStripMenuItem.Click
        CloneItem()
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        DeleteItem()
    End Sub

    Private Sub DeleteToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem1.Click
        DeleteItem()
    End Sub

#End Region

#Region "Buttons"
    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        SaveFileDialog1.Title = "DropGUI - Save to .reg"
        SaveFileDialog1.DefaultExt = "reg"
        SaveFileDialog1.Filter = "Registery File|*.reg"
        SaveFileDialog1.FilterIndex = 1


        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            SaveFileDialog1.Dispose()
            Exit Sub
        End If


        Dim regKeyGUIs As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS", True)
        If regKeyGUIs IsNot Nothing Then
            Dim KeyGUICount As Integer = regKeyGUIs.SubKeyCount()
            If KeyGUICount > 0 Then
                Dim sb As New StringBuilder()
                sb.AppendLine("Windows Registry Editor Version 5.00")
                sb.AppendLine()
                If regKeyGUIs IsNot Nothing Then
                    For Each KeyGUI As String In regKeyGUIs.GetSubKeyNames()
                        Dim regKeyGUI As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS\" & KeyGUI, True)
                        If regKeyGUI IsNot Nothing Then
                            Dim Command As String = CType(regKeyGUI.GetValue("Command"), String)
                            Dim Input As String = CType(regKeyGUI.GetValue("Input"), String)
                            Dim Output As String = CType(regKeyGUI.GetValue("Output"), String)
                            Dim Path As String = CType(regKeyGUI.GetValue("Path"), String)

                            'replace "\" by "\\" in path
                            If Path <> "" Then
                                Dim Pos As Integer = InStr(1, Path, "\", CompareMethod.Text)
                                If Pos <> 0 Then
                                    Do
                                        Path = Path.Insert(Pos, "\")
                                        If Pos < Path.Length Then
                                            Pos = InStr(Pos + 2, Path, "\", CompareMethod.Text)
                                        Else
                                            Pos = 0
                                        End If
                                    Loop Until Pos = 0
                                End If
                            End If


                            Dim Status As Integer = CType(regKeyGUI.GetValue("Status"), Integer)
                            sb.AppendLine("[HKEY_CURRENT_USER\Software\DropGUI\GUIS\" & KeyGUI & "]")
                            sb.AppendLine(Chr(34) & "Command" & Chr(34) & "=" & Chr(34) & Command & Chr(34))
                            sb.AppendLine(Chr(34) & "Input" & Chr(34) & "=" & Chr(34) & Input & Chr(34))
                            sb.AppendLine(Chr(34) & "Output" & Chr(34) & "=" & Chr(34) & Output & Chr(34))
                            sb.AppendLine(Chr(34) & "Path" & Chr(34) & "=" & Chr(34) & Path & Chr(34))
                            sb.AppendLine(Chr(34) & "Status" & Chr(34) & "=dword:" & Status)
                            sb.AppendLine()
                        End If
                    Next

                    'Write to file UNICODE
                    Using outfile As New StreamWriter(SaveFileDialog1.FileName, False, Encoding.Unicode)
                        outfile.Write(sb.ToString())
                    End Using
                    MessageBox.Show("GUIs are saved to the reg file successfully", "DropGUI", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                MessageBox.Show("There is no GUI in the list to save", "DropGUI", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If

    End Sub

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

    'Deactivate all same input extention when one is activated
    Private Sub DeactivateAll(GUIName As String)
        Dim x As Integer = 0
        Dim input As String = ListViewGUI.SelectedItems(0).SubItems(1).Text
        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS", True)
        If regKey IsNot Nothing Then
            For Each Name As String In regKey.GetSubKeyNames
                regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS\" & Name, True)
                Dim input2 As String = regKey.GetValue("Input")
                If input = input2 And Name <> GUIName Then
                    regKey.SetValue("Status", 0)
                    ListViewGUI.Items(x).SubItems(5).Text = "Deactivated"
                End If
                x += 1
            Next
        End If
    End Sub

    Private Sub LaunchEditDialogue()
        Dim Name As String = ListViewGUI.SelectedItems(0).Text
        Dim input As String = ListViewGUI.SelectedItems(0).SubItems(1).Text
        Dim Path As String = ListViewGUI.SelectedItems(0).SubItems(2).Text
        Dim Com As String = ListViewGUI.SelectedItems(0).SubItems(3).Text
        Dim output As String = ListViewGUI.SelectedItems(0).SubItems(4).Text
        Dim Newdia As New DialogNew(Name, input, Path, Com, output, True, ListViewGUI)
        Newdia.ShowDialog()
        Newdia.Dispose()
    End Sub

    Private Sub CloneItem()
        Dim Name As String = ListViewGUI.SelectedItems(0).Text
        Dim input As String = ListViewGUI.SelectedItems(0).SubItems(1).Text
        Dim Path As String = ListViewGUI.SelectedItems(0).SubItems(2).Text
        Dim Com As String = ListViewGUI.SelectedItems(0).SubItems(3).Text
        Dim output As String = ListViewGUI.SelectedItems(0).SubItems(4).Text

        'Add to listview
        Dim Lvi As ListViewItem
        Lvi = ListViewGUI.Items.Add(Name & " (Clone)")
        With Lvi
            .SubItems.Add(input)
            .SubItems.Add(Path)
            .SubItems.Add(Com)
            .SubItems.Add(output)
            .SubItems.Add("Deactivated")
        End With

        'Create reg key
        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS", True)
        regKey.CreateSubKey(Name & " (Clone)")
        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS\" & Name & " (Clone)", True)
        regKey.SetValue("Command", Com)
        regKey.SetValue("Input", input)
        regKey.SetValue("Output", output)
        regKey.SetValue("Path", Path)
        regKey.SetValue("Status", 0)
    End Sub

    Private Sub DeleteItem()
        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS", True)
        regKey.DeleteSubKey(ListViewGUI.SelectedItems(0).Text)
        ListViewGUI.SelectedItems(0).Remove()
    End Sub



#End Region




End Class