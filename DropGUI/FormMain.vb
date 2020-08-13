Imports Microsoft.Win32
''' <summary>
''' DropGUI 4.0.0.1
''' Copyright Martin Laflamme 2003/2020
''' </summary>

Public Class FormMain

#Region "Declarations"
    'Use for registry
    Private regKey As RegistryKey
    Private OutputPath As String = ""
#End Region

#Region "Methods"
    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "DropGUI 4.0"

        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\OutputPath", True)
        If regKey IsNot Nothing Then
            OutputPath = regKey.GetValue("", "")
        Else
            regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI", True)
            If regKey IsNot Nothing Then
                regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings", True)
                If regKey IsNot Nothing Then
                    regKey.CreateSubKey("OutputPath")
                Else
                    regKey.CreateSubKey("Settings")
                    regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings", True)
                    regKey.CreateSubKey("OutputPath")
                End If
            Else
                regKey.CreateSubKey("DropGUI")
                regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI", True)
                regKey.CreateSubKey("Settings")
                regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings", True)
                regKey.CreateSubKey("OutputPath")
            End If
        End If

        'MessageBox.Show(OutputPath)
    End Sub

    Private Sub FormMain_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick

        If e.Button = Windows.Forms.MouseButtons.Right Then
            ContextMenuStrip1.Show(MousePosition.X, MousePosition.Y)
        End If

    End Sub
#End Region

#Region "ContextMenu"
    Private Sub SelectDestinationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectDestinationToolStripMenuItem.Click

        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\OutputPath", True)
            regKey.SetValue("", FolderBrowserDialog1.SelectedPath)
        End If
    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        Close()
    End Sub
#End Region

End Class
