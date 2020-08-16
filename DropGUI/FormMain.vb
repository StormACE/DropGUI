Imports Microsoft.Win32
''' <summary>
''' DropGUI 4.0.0.4
''' 12 Aout 2020 to 16 Aout 2020
''' Copyright Martin Laflamme 2003/2020
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

    Private Sub FormMain_DragDrop(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        Dim match As Boolean = False
        Dim GUIName As String = ""
        Dim Outputfile As String = ""
        For Each path As String In files
            Dim ext As String = IO.Path.GetExtension(path)
            Dim Filename As String = IO.Path.GetFileNameWithoutExtension(path)
            regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS", True)
            If regKey IsNot Nothing Then
                For Each Name As String In regKey.GetSubKeyNames
                    regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS\" & Name, True)
                    Dim Input As String = regKey.GetValue("Input")
                    Dim Activate As Integer = regKey.GetValue("Status")
                    If "." & Input = ext And Activate = 1 Then
                        match = True
                        GUIName = Name
                    End If
                Next

                'We got a Match !!!
                If match = True Then
                    regKey = Registry.CurrentUser.OpenSubKey("Software\DropGUI\GUIS\" & GUIName, True)
                    Dim Output As String = regKey.GetValue("Output")
                    Dim ProgramPath As String = regKey.GetValue("Path")
                    Dim Command As String = regKey.GetValue("Command")

                    Dim Pos As Integer = InStr(1, Command, "/@in", CompareMethod.Text)
                    If Pos <> 0 Then
                        Do
                            Command = Command.Replace("/@in", """" & path & """")
                            Pos = InStr(1, Command, "/@in", CompareMethod.Text)
                        Loop Until Pos = 0

                        Pos = InStr(1, Command, "/@out", CompareMethod.Text)
                        If OutputPath <> "" Then
                            Outputfile = OutputPath & Filename & "." & Output
                            Do
                                Command = Command.Replace("/@out", """" & Outputfile & """")
                                Pos = InStr(1, Command, "/@out", CompareMethod.Text)
                            Loop Until Pos = 0
                        Else
                            MsgBox("Output Folder not selected")
                        End If


                    Else
                        MsgBox("Pointer /@in must be use in your command")
                    End If

                    If Debug = 1 Then
                        MsgBox(Command, MsgBoxStyle.Information, ProgramPath)
                    End If

                    LaunchApp(ProgramPath, Command)

                Else
                    MsgBox("GUI doesnt exist")
                End If

            End If
        Next
    End Sub

    Private Sub FormMain_DragEnter(sender As System.Object, e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragEnter
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
    End Sub

    Private Sub LaunchApp(ProgramPath As String, Command As String)
        Dim info As New ProcessStartInfo
        info.FileName = ProgramPath
        info.Arguments = Command
        info.CreateNoWindow = False
        info.WindowStyle = ProcessWindowStyle.Normal

        Dim App As Process = Process.Start(info)
        App.WaitForExit()
        MsgBox("Job Completed")
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
