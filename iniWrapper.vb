'2025-3-15
Imports System.IO
Imports System.Diagnostics
Imports IniParser
Public Class iniWrapper
    Public iniDict As Model.IniData
    Private iniParserObj As New FileIniDataParser
    Private _iniPath As String

    Public ReadOnly EXEfolder As String = IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)

    Public Sub New(Optional iniPath As String = "[EXE directory]", Optional AutoGenerateIfMissing As Boolean = False)
        If iniPath = "[EXE directory]" Then
            iniPath = EXEfolder & "\" & "UserOptions.ini"
        End If
        _iniPath = iniPath
        If IO.File.Exists(iniPath) = True Then 'global vars
            iniDict = iniParserObj.ReadFile(iniPath) 'global vars
        Else
            If AutoGenerateIfMissing = True OrElse vbYes = MsgBox("Settings ini file was not found. Generate a new one?", MsgBoxStyle.YesNo) Then
                GenerateNewINIfile(iniPath)
            Else
                End
            End If
        End If

    End Sub
    Public Function GenerateNewINIfile(iniPath As String) As Boolean
        'running this will also overwrite the existing file and reset settings to default

        If File.Exists(iniPath) = True Then
            Return False
        End If

        Dim defaultIni As String = EXEfolder & "\defaults.dat"
        If File.Exists(defaultIni) = False Then
            Throw New FileNotFoundException("default ini config definition file is missing.")
        End If

        File.Copy(defaultIni, iniPath, True)
        iniDict = iniParserObj.ReadFile(iniPath) 'global vars

        Return True
    End Function

    Public Property Entry(keyName As String, Optional section As String = "Main") As String
        Get
            If iniDict(section).ContainsKey(keyName) = False Then
                Throw New Collections.Generic.KeyNotFoundException("Given key does not exist in ini file." & vbCrLf & "Section: " & section & ", Key: " & keyName)
            End If
            Dim keyVal As String = iniDict(section)(keyName)
            Return keyVal
        End Get
        Set(keyValue As String)
            If iniDict(section).ContainsKey(keyName) = False Then
                Throw New Collections.Generic.KeyNotFoundException("Given key does not exist in ini file." & vbCrLf & "Section: " & section & ", Key: " & keyName)
            End If

            iniDict(section)(keyName) = keyValue
            iniParserObj.WriteFile(_iniPath, iniDict)
        End Set
    End Property
    Public Sub SaveFile()
        'Note this should only be called after editing the iniDict directly
        iniParserObj.WriteFile(_iniPath, iniDict)
    End Sub
    Public Sub OpenINI(Optional OpenFolder As Boolean = False, Optional CloseFirst As Boolean = False)

        If OpenFolder = False Then
            OpenTextFile(_iniPath,, True, "CloseIfOpen")
        Else
            Process.Start("explorer.exe", IO.Path.GetDirectoryName(_iniPath))
        End If
    End Sub

    Private Function OpenTextFile(FilePath As String, Optional ByRef ExceptionStr As String = "", Optional ShowErrorMessageWindow As Boolean = True, Optional CloseMode As String = "[Off,CloseIfOpen,ForceClose]") As Boolean
        '2025-3-15  

        If CloseMode = "[Off,CloseIfOpen,ForceClose]" Then
            CloseMode = "Off"
        ElseIf CloseMode <> "CloseIfOpen" And CloseMode <> "ForceClose" And CloseMode <> "Off" Then
            Throw New Exception
        End If

        Try
            FilePath = Strings.Split(FilePath, vbCrLf)(0)
            If IO.File.Exists(FilePath) = False Then
                Throw New IO.FileNotFoundException("File not found." & vbCrLf & FilePath)
            End If

            If CloseMode <> "Off" Then
                For Each proc As Process In Process.GetProcessesByName("notepad")
                    If Not String.IsNullOrEmpty(proc.MainWindowTitle) AndAlso proc.MainWindowTitle.Contains(Path.GetFileName(FilePath)) Then
                        Try
                            proc.CloseMainWindow() ' Attempt graceful closure

                            proc.WaitForExit(1000)
                            If CloseMode = "ForceClose" AndAlso proc.HasExited = False Then
                                proc.Kill() ' Force close if still running
                            Else
                                '
                            End If

                        Catch ex As Exception
                            ExceptionStr = ex.Message
                            If ShowErrorMessageWindow = True Then
                                MsgBox("Error closing Notepad:" & vbCrLf & ExceptionStr)
                            End If
                            Return False
                        End Try
                    End If
                Next
            End If

            Process.Start("notepad.exe", FilePath)
            Return True
        Catch ex As Exception
            ExceptionStr = ex.Message
            If ShowErrorMessageWindow = True Then
                MsgBox("Error opening text file:" & vbCrLf & ExceptionStr)
            End If
            Return False
        End Try
    End Function

End Class
