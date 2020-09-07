Imports Microsoft.Win32
''' <summary>
''' DropGUI 4.0.1.10
''' 12 Aout 2020 to 7 septembre 2020
''' Copyright Martin Laflamme 2004/2020
''' 
''' Fix bug with destination folder
''' </summary>

Public Class FormMain

#Region "Declarations"
    'Use for registry
    Private regKey As RegistryKey
    Private OutputPath As String = ""
    Private Debug As Integer = 0
#End Region

#Region "Methods"
    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "DropGUI 4.0"
        MinimumSize = New Size(300, 300)

        'Is Starting size and location exist in reg
        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\WinSize", True)
        If regKey IsNot Nothing Then
            Me.Size = New Size(CInt(regKey.GetValue("Width", "300")), CInt(regKey.GetValue("Height", "300")))
            Location = New Point(regKey.GetValue("X"), regKey.GetValue("Y"))
        Else
            Me.Size = New Size(300, 300)
            CenterToScreen()
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

        regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\Debug", True)
        If regKey IsNot Nothing Then
            Debug = regKey.GetValue("")
            If Debug = 1 Then
                ShowDebugMessageToolStripMenuItem.Checked = True
            Else
                ShowDebugMessageToolStripMenuItem.Checked = False
            End If
        Else
            Debug = 0
        End If
    End Sub

    Private Sub FormMain_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick

        If e.Button = Windows.Forms.MouseButtons.Right Then
            ContextMenuStrip1.Show(MousePosition.X, MousePosition.Y)
        End If

    End Sub

    Private Sub FormMain_FormClosing(ByVal sender As System.Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        SaveWinSize()
    End Sub

    Private Sub FormMain_DragDrop(sender As System.Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        'Lancer dans un nouveau thread
        Dim Tasks As New DoDrop(files, OutputPath, Debug)
        Dim Thread1 As New Threading.Thread(AddressOf Tasks.Drop)
        Thread1.Name = "DropGUIDoDrop"
        Thread1.Priority = Threading.ThreadPriority.Normal
        Thread1.Start() ' Start the new thread.
    End Sub

    Private Sub FormMain_DragEnter(sender As System.Object, e As DragEventArgs) Handles MyBase.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

#End Region

#Region "ContextMenu"
    Private Sub SelectDestinationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectDestinationToolStripMenuItem.Click

        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\OutputPath", True)
            regKey.SetValue("", FolderBrowserDialog1.SelectedPath)
            OutputPath = FolderBrowserDialog1.SelectedPath
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

    Private Sub GUIManagerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GUIManagerToolStripMenuItem.Click
        FormGUImanager.Show()
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
        regKey.SetValue("X", Location.X)
        regKey.SetValue("Y", Location.Y)
    End Sub

    Private Sub ShowDebugMessageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowDebugMessageToolStripMenuItem.Click
        If ShowDebugMessageToolStripMenuItem.Checked = False Then
            ShowDebugMessageToolStripMenuItem.Checked = True
            regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\Debug", True)
            If regKey Is Nothing Then
                regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings", True)
                regKey.CreateSubKey("Debug")
            End If
            regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\Debug", True)
            regKey.SetValue("", 1)
            Debug = 1
        Else
            ShowDebugMessageToolStripMenuItem.Checked = False
            regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\Debug", True)
            If regKey Is Nothing Then
                regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings", True)
                regKey.CreateSubKey("Debug")
            End If
            regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\Settings\Debug", True)
            regKey.SetValue("", 0)
            Debug = 0
        End If
    End Sub

#End Region

End Class
