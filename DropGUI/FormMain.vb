Imports Microsoft.Win32
''' <summary>
''' DropGUI 4.0.0.2
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
        MinimumSize = New Drawing.Size(300, 300)
        CenterToScreen()


        'Is Starting size exist in reg
        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\WinSize", True)
        If regKey IsNot Nothing Then
            Me.Size = New Size(CInt(regKey.GetValue("Width", "300")), CInt(regKey.GetValue("Height", "300")))
        Else
            Me.Size = New Size(300, 300)
        End If

        'Check Output Path and create reg key
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
                regKey = Registry.CurrentUser.OpenSubKey("Software", True)
                regKey.CreateSubKey("DropGUI")
                regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI", True)
                regKey.CreateSubKey("Settings")
                regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings", True)
                regKey.CreateSubKey("OutputPath")
            End If
        End If

        'Set Background Image
        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\BackgroundImage", True)
        If regKey IsNot Nothing Then
            BackgroundImage = Image.FromFile(regKey.GetValue(""))
        End If

        'MessageBox.Show(OutputPath)
    End Sub

    Private Sub FormMain_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick

        If e.Button = Windows.Forms.MouseButtons.Right Then
            ContextMenuStrip1.Show(MousePosition.X, MousePosition.Y)
        End If

    End Sub

    Private Sub FormMain_FormClosing(ByVal sender As System.Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        SaveWinSize()
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
        SaveWinSize()
        Close()
    End Sub

    Private Sub SelectBackImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectBackImageToolStripMenuItem.Click
        OpenFileDialog1.DefaultExt = "jpg"
        OpenFileDialog1.Filter = "Jpeg Pictures|*.jpg|PNG Pictures|*.png|All files|*.*"
        OpenFileDialog1.Title = "Open Picture File ..."
        OpenFileDialog1.FileName = ""

        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            BackgroundImage = Image.FromFile(OpenFileDialog1.FileName)
            regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\BackgroundImage", True)
            If regKey Is Nothing Then
                regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings", True)
                regKey.CreateSubKey("BackgroundImage")
                regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\BackgroundImage", True)
            End If
            regKey.SetValue("", OpenFileDialog1.FileName)
        End If
    End Sub
#End Region

#Region "Subroutines"
    Private Sub SaveWinSize()
        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\WinSize", True)
        If regKey Is Nothing Then
            regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\", True)
            regKey = regKey.CreateSubKey("WinSize")
        End If
        regKey.SetValue("Height", Height)
        regKey.SetValue("Width", Width)
    End Sub

    Private Sub GUIManagerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GUIManagerToolStripMenuItem.Click
        FormGUImanager.Show()
    End Sub
#End Region

End Class
