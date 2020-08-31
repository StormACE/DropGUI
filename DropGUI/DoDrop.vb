Imports Microsoft.Win32

Public Class DoDrop

    Dim Files() As String
    Dim OutputPath As String
    Dim Debug As Integer = 0
    Private regKey As RegistryKey

    'Constructor
    Public Sub New(ByVal File() As String, Outpath As String, Debg As Integer)
        Files = File
        OutputPath = Outpath
        Debug = Debg
    End Sub

    Public Sub Drop()
        Dim match As Boolean = False
        Dim GUIName As String = ""
        Dim Outputfile As String = ""
        Dim Fcount As Integer = Files.Count
        Dim x As Integer = 0

        For Each path As String In Files
            x += 1
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

                    If x = Fcount Then
                        MsgBox("Job Completed")
                    End If

                Else
                    MsgBox("GUI doesnt exist")
                End If

            End If
        Next
    End Sub

    Private Sub LaunchApp(ProgramPath As String, Command As String)
        Dim info As New ProcessStartInfo
        info.FileName = ProgramPath
        info.Arguments = Command
        info.CreateNoWindow = False
        info.WindowStyle = ProcessWindowStyle.Normal

        Dim App As Process = Process.Start(info)
        App.WaitForExit()
    End Sub
End Class
