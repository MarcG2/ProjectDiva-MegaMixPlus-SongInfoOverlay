Imports System.IO


Imports System
Imports System.Collections.Generic
Imports System.Text

Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Diagnostics

Imports GameOverlay.Drawing
Imports GameOverlay.Windows
Imports WindowsAPICodePack.Dialogs
Imports System.IO.Pipes
Imports System.Text.RegularExpressions
Imports System.Net.Http.Headers
Imports System.Runtime.Remoting.Channels
Imports System.Windows.Forms.VisualStyles
Imports System.ComponentModel



Public Module GlobalVariables
    Public iniObj As iniSettings
    Public pipeClient As NamedPipeClientStream
    Public MainForm As Form1
    Public MonitorGame As Boolean

    Public FullSongDict As New Dictionary(Of String, CustomSong) 'Key str is Song ID

    Public Structure OverlayConfig
        Public DisplayStr As String
        Public DisplayTime As Integer
        Public xPos As Integer
        Public yPos As Integer
        Public width As Integer
        Public height As Integer
        Public FontSize As Double
        Public ProcessName As String
    End Structure

End Module

Public Module MainFunction

    Async Function ScanModFolder(TopModFolderPath As String, SkipDisabledPacks As Boolean) As Task(Of Dictionary(Of String, CustomSong))
        Dim SongDict As New Dictionary(Of String, CustomSong)
        'ReDim SongArr(0 To 0)

        Dim ModFolders() As String
        ModFolders = Directory.GetDirectories(TopModFolderPath)

        'Dim SongRegex As New Regex("^#?pv_(\d{3,4})\.")
        Dim SongRegex As New Regex("^pv_(\d{3,4})\.")

        For i As Integer = 0 To ModFolders.Length - 1
            Dim ModName As String, ModNameExplicit As Boolean, ModEnabled As Boolean

            Dim ConfigPath As String = ModFolders(i) & "\config.toml"

            Try 'Get mod name from config.toml
                ModName = ""
                Dim ConfigArr() As String = File.ReadAllLines(ConfigPath)

                For Each str As String In ConfigArr
                    Dim param As String = Trim(Split(str, "=")(0))

                    If param = "enabled" Then
                        ModEnabled = Split(str, "=")(1).Trim({" "c, """"c})
                        If ModEnabled = False AndAlso SkipDisabledPacks = True Then
                            Exit For
                        End If
                    ElseIf param = "name" Then
                        ModName = Split(str, "=")(1).Trim({" "c, """"c})
                        Exit For
                    End If

                Next
                If SkipDisabledPacks = True AndAlso ModEnabled = False Then
                    Continue For
                End If

                If ModName = "" Then
                    ModName = Path.GetFileName(ModFolders(i))
                    ModNameExplicit = False
                Else
                    ModNameExplicit = True
                End If

            Catch ex As FileNotFoundException
                Continue For 'If this file is missing, then folder is not a proper mod folder
            Catch ex As Exception
                MsgBox("Error reading config file:" & vbCrLf & ConfigPath)
                Exit Function
            End Try

            Dim modDBpath As String = ModFolders(i) & "\rom\mod_pv_db.txt"

            If File.Exists(modDBpath) Then '
                Dim DBarr() As String = File.ReadAllLines(modDBpath)

                For j As Long = 0 To DBarr.LongLength - 1
                    Dim LineStr As String = DBarr(j)
                    Dim SongParam As String
                    Dim RegMatch As Match = SongRegex.Match(LineStr)

                    If RegMatch.Success = True Then
                        Dim ID As String = RegMatch.Groups(1).Value 'returns numeric portion of string

                        If SongDict.ContainsKey(ID) = False Then
                            SongDict.Add(ID, New CustomSong(ID))
                            SongDict(ID).ModName = ModName
                            If ModNameExplicit = False Then
                                SongDict(ID).LogError("Note: Mod name not specified in .toml file.")
                            End If
                            SongDict(ID).FolderPath = ModFolders(i)
                            SongDict(ID).dbPath = modDBpath
                            ParseSongParameter(ID, SongDict, DBarr(j))
                        Else
                            If SongDict(ID).dbPath <> modDBpath Then
                                SongDict(ID).dbPath = modDBpath
                            End If
                            ParseSongParameter(ID, SongDict, DBarr(j))
                        End If
                    End If

                Next j
            End If
        Next i

        Return SongDict
    End Function

    Function ParseSongParameter(ID As String, SongDict As Dictionary(Of String, CustomSong), DBstr As String) As String

        Dim ParamAndVal As String = TruncateStr(DBstr, "l", 1, ".")

        Dim SongParam As String = Split(ParamAndVal, "=")(0)
        Dim ParamVal As String
        Try
            ParamVal = Split(ParamAndVal, "=")(1)
        Catch ex As Exception
            ParamVal = ""
        End Try

        Dim SongObj As CustomSong = SongDict(ID)

        Select Case SongParam
            Case "song_name_en"
                SongObj.SongName = ParamVal
                'If Left(DBstr, 1) = "#" Then
                '    SongObj.Hidden = True
                'End If
            Case "difficulty.extreme.1.level"
                SongObj.AddDifficulty(ParamVal, CustomSong.Difficulties.ExEx)
            Case "difficulty.extreme.0.level"
                SongObj.AddDifficulty(ParamVal, CustomSong.Difficulties.Extreme)
            Case "difficulty.hard.0.level"
                SongObj.AddDifficulty(ParamVal, CustomSong.Difficulties.Hard)
            Case "difficulty.normal.0.level"
                SongObj.AddDifficulty(ParamVal, CustomSong.Difficulties.Normal)
            Case "difficulty.easy.0.level"
                SongObj.AddDifficulty(ParamVal, CustomSong.Difficulties.Easy)
        End Select

        Return SongParam

    End Function

    Public Sub StartMainRoutine()
        MonitorGame = True 'global var
        If pipeClient.IsConnected = False Then
            StartPipeConnection()
        End If

        Dim defaultConfig As New OverlayConfig With {
            .DisplayStr = MainForm.TB_DebugTextBox.Text,
            .height = MainForm.TB_OverlayHeight.Text,
            .width = MainForm.TB_OverlayWidth.Text,
            .xPos = MainForm.TB_OverlayXpos.Text,
            .yPos = MainForm.TB_OverlayYpos.Text,
            .DisplayTime = MainForm.TB_OverlayDisplayTime.Text,
            .FontSize = MainForm.TB_FontSize.Text,
            .ProcessName = MainForm.TB_ProcessName.Text
        }

        Dim MainWorkThread As New Task(Sub() MainRoutine(300, defaultConfig))
        MainWorkThread.Start()
    End Sub

    Public Async Sub MainRoutine(pingTime As Integer, defaultConfig As OverlayConfig)

        Dim songID As Integer, IDstr As String, SongName As String, ModName As String, LastID As Integer, DisplayText As String
        While MonitorGame = True 'global var
            If pipeClient.IsConnected = False Then Exit Sub
            Await WaitAsync(pingTime)

            songID = GetSongID()

            If songID <> -1 AndAlso LastID <> songID Then

                LastID = songID
                If songID < 1000 Then
                    IDstr = songID.ToString.PadLeft(3, "0"c)
                Else
                    IDstr = songID.ToString.PadLeft(4, "0"c)
                End If

                Try
                    SongName = FullSongDict(IDstr).SongName
                    ModName = FullSongDict(IDstr).ModName

                    DisplayText = $"Song {IDstr}: {SongName} {vbCrLf}From {ModName}"
                    defaultConfig.DisplayStr = DisplayText

                    OverLayTextWindow(defaultConfig)
                Catch ex As Exception

                End Try

            End If

        End While

    End Sub

    Public Async Sub OverLayTextWindow(config As OverlayConfig)

        Dim backgroundThread As New Thread(Sub()
                                               Dim overlay As New GameOverLay(config)
                                               If overlay.Initiate() = True Then overlay.Run()
                                           End Sub)
        backgroundThread.IsBackground = True
        backgroundThread.Start()

        Await WaitAsync(config.DisplayTime + 250)

        backgroundThread.Abort()
    End Sub
    Public Sub StartPipeConnection()
        Try
            pipeClient.Connect(1000)
        Catch ex As Exception
            WriteToDebugBox("FAILED TO CONNECT.")
            Exit Sub
        End Try

    End Sub
    Public Function GetSongID() As Integer
        Try
            Dim message As Byte() = Encoding.ASCII.GetBytes("song-id")
            pipeClient.Write(message, 0, message.Length)
            pipeClient.Flush() ' Flush to ensure the message is sent

            ' Read 4 bytes as a response from the server
            Dim response(3) As Byte
            pipeClient.Read(response, 0, response.Length)

            'Dim responseString As String = Encoding.ASCII.GetString(response)
            Dim result As Integer = BitConverter.ToInt32(response, 0)
            Return result

        Catch ex As Exception
            If pipeClient.IsConnected = False Then
                WriteToDebugBox("Communication failed. Pipe connection lost.")
            Else
                WriteToDebugBox("Unknown pipe communication error." & vbCrLf & ex.Message)
            End If
            Return 0
        End Try
    End Function

    Async Function WaitAsync(milliseconds As Integer) As Task
        Await Task.Delay(milliseconds)
    End Function

    Public Sub WriteToDebugBox(str As String)
        If MainForm.InvokeRequired = True Then
            MainForm.Invoke(Sub() MainForm.TB_DebugTextBox.Text = str)
        Else
            MainForm.TB_DebugTextBox.Text = str
        End If
    End Sub

End Module


Public Class Form1
    Private hook As New KeyboardHook
    Private OverlayObj As GameOverLay
    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MainForm = Me

        iniObj = New iniSettings
        TB_GameFolder.Text = iniObj.GetSetting(iniSettings.StandardParams.Main_GameFolder)
        TB_OverlayHeight.Text = iniObj.GetSetting(iniSettings.StandardParams.Main_OverlayHeight)
        TB_OverlayWidth.Text = iniObj.GetSetting(iniSettings.StandardParams.Main_OverlayWidth)
        TB_OverlayXpos.Text = iniObj.GetSetting(iniSettings.StandardParams.Main_Xpos)
        TB_OverlayYpos.Text = iniObj.GetSetting(iniSettings.StandardParams.Main_Ypos)
        TB_OverlayDisplayTime.Text = iniObj.GetSetting(iniSettings.StandardParams.Main_DisplayTime)
        TB_FontSize.Text = iniObj.GetSetting(iniSettings.StandardParams.Main_FontSize)
        TB_ProcessName.Text = iniObj.GetSetting(iniSettings.StandardParams.Main_ProcessName)

        TB_OverlayTest.Text = $"Song 4538: THE POWER OF TERRY {vbCrLf}From Nitori's Song Pack"

        pipeClient = New NamedPipeClientStream(".", "marcgii", PipeDirection.InOut)

        'AddGlobalHotkeySupport()
        'HotkeyTest()

        Await Task.Run(Function() ScanStart())

    End Sub

    Private Async Function ScanStart() As Task

        Dim GameFolderPath As String = TB_GameFolder.Text
        Dim ModFolderPath As String = GameFolderPath & "\mods"

        If Directory.Exists(ModFolderPath) = True Then
            FullSongDict = ScanModFolder(ModFolderPath, True).Result
        Else
            MsgBox("Game folder path given does not exisit.", MsgBoxStyle.OkOnly, "Game Folder Error")
            Exit Function
        End If

    End Function
    Private Sub But_TestOverlay_Click(sender As Object, e As EventArgs) Handles But_TestOverlay.Click
        If VerifySettings() = False Then Exit Sub

        Dim config As New OverlayConfig With {
            .DisplayStr = TB_OverlayTest.Text,
            .height = TB_OverlayHeight.Text,
            .width = TB_OverlayWidth.Text,
            .xPos = TB_OverlayXpos.Text,
            .yPos = TB_OverlayYpos.Text,
            .DisplayTime = TB_OverlayDisplayTime.Text,
            .FontSize = TB_FontSize.Text,
            .ProcessName = TB_ProcessName.Text
        }


        OverLayTextWindow(config)
    End Sub


    Private Sub But_Test2_Click(sender As Object, e As EventArgs) Handles But_Test2.Click
        'TB_test2.Text = GetWindowHandleByProcessName("DivaMegaMix")

        StartPipeConnection()
    End Sub

    Private Sub But_OpenINI_Click(sender As Object, e As EventArgs) Handles But_OpenINI.Click
        iniObj.OpenINIinNotepad()
    End Sub

    Private Sub But_OpenEXEfolder_Click(sender As Object, e As EventArgs) Handles But_OpenEXEfolder.Click
        iniObj.OpenINIinNotepad(True)
    End Sub

    Private Sub But_SelectFolder_Click(sender As Object, e As EventArgs) Handles But_SelectFolder.Click

        Using dialog As New CommonOpenFileDialog
            dialog.IsFolderPicker = True
            dialog.Title = "Select a Folder"

            If dialog.ShowDialog() = CommonFileDialogResult.Ok Then
                TB_GameFolder.Text = dialog.FileName

            End If
        End Using
    End Sub

    Private Sub But_SaveSettings_Click(sender As Object, e As EventArgs) Handles But_SaveSettings.Click

        If VerifySettings() = False Then Exit Sub

        Dim DisplayTime As Integer = Integer.Parse(TB_OverlayDisplayTime.Text, DisplayTime)
        Dim Height As Integer = Integer.Parse(TB_OverlayHeight.Text, Height)
        Dim Width As Integer = Integer.Parse(TB_OverlayWidth.Text, Width)
        Dim xPos As Integer = Integer.Parse(TB_OverlayXpos.Text, xPos)
        Dim yPos As Integer = Integer.Parse(TB_OverlayYpos.Text, yPos)
        Dim FontSize As Double = Double.Parse(TB_FontSize.Text, FontSize)
        Dim ProcessName As String = TB_ProcessName.Text

        With iniObj
            .SetSetting(TB_GameFolder.Text, iniSettings.StandardParams.Main_GameFolder)
            .SetSetting(DisplayTime, iniSettings.StandardParams.Main_DisplayTime)
            .SetSetting(Height, iniSettings.StandardParams.Main_OverlayHeight)
            .SetSetting(Width, iniSettings.StandardParams.Main_OverlayWidth)
            .SetSetting(xPos, iniSettings.StandardParams.Main_Xpos)
            .SetSetting(yPos, iniSettings.StandardParams.Main_Ypos)
            .SetSetting(FontSize, iniSettings.StandardParams.Main_FontSize)
            .SetSetting(ProcessName, iniSettings.StandardParams.Main_ProcessName)
        End With
    End Sub

    Public Function VerifySettings() As Boolean
        VerifySettings = True
        Dim res As Boolean
        Dim DisplayTime As Integer : res = Integer.TryParse(TB_OverlayDisplayTime.Text, DisplayTime)
        If res = False Or DisplayTime < 200 Or DisplayTime > 10000 Then
            MsgBox("Display time is measured in ms. Valid range is 200 to 10000 ms. Correct before saving.", MsgBoxStyle.OkOnly)
            Return False
        End If

        If res = False Or Height < 30 Or Height > 600 Then
            MsgBox("Overlay height is measured in pixels. Valid range is 30 to 600. Correct before saving.", MsgBoxStyle.OkOnly)
            Return False
        End If
        Dim Width As Integer : res = Integer.TryParse(TB_OverlayWidth.Text, Width)
        If res = False Or Width < 50 Or Width > 800 Then
            MsgBox("Overlay Width is measured in pixels. Valid range is 50 to 800. Correct before saving.", MsgBoxStyle.OkOnly)
            Return False
        End If

        Dim xPos As Integer : res = Integer.TryParse(TB_OverlayXpos.Text, xPos)
        If res = False Then
            MsgBox("Overlay x position is measured in from the left of the main monitor and is measured in pixels. Correct before saving.", MsgBoxStyle.OkOnly)
            Return False
        End If
        Dim yPos As Integer : res = Integer.TryParse(TB_OverlayYpos.Text, yPos)
        If res = False Then
            MsgBox("Overlay y position is measured in from the top of the main monitor and is measured in pixels. Correct before saving.", MsgBoxStyle.OkOnly)
            Return False
        End If
        Dim FontSize As Integer : res = Integer.TryParse(TB_FontSize.Text, FontSize)
        If res = False Or FontSize < 8 Or FontSize > 64 Then
            MsgBox("Font size is measured in points. Valid range is 8 to 64 pts. Correct before saving.", MsgBoxStyle.OkOnly)
            Return False
        End If


    End Function

    Private Sub But_SendTest_Click(sender As Object, e As EventArgs) Handles But_SendTest.Click
        dbBox("Current Song: " & GetSongID())

    End Sub
    Private Sub But_StartOveralyThread_Click(sender As Object, e As EventArgs) Handles But_StartOveralyThread.Click
        StartMainRoutine()
    End Sub

    Public Sub AddGlobalHotkeySupport()  'TODO: call this at initialization of the application

        ' register the event that is fired after the key press.
        AddHandler hook.KeyPressed, AddressOf hook_KeyPressed

        ' register the control + alt + F12 combination as hot key.
        hook.RegisterHotKey(MMplus_Song_Info_Overlay.ModifierKeys.Control, Keys.F12)
        'MMplus_Song_Info_Overlay.ModifierKeys.Control
    End Sub
    Public Sub RemoveGlobalHotkeySupport()  'TODO: call this at finalization of the application
        ' unregister all registered hot keys.
        hook.Dispose()
    End Sub

    Private Sub hook_KeyPressed(sender As Object, e As KeyPressedEventArgs)
        ' show the keys pressed in a label.
        AudioNotification()
        But_TestOverlay_Click(New Object, New EventArgs)
    End Sub

    Public Sub dbBox(str As String, Optional appendTxt As Boolean = True)
        If appendTxt = True Then
            str = TB_DebugTextBox.Text & vbCrLf & str
        End If

        If Me.InvokeRequired = True Then
            Me.Invoke(Sub() TB_DebugTextBox.Text = str)
        Else
            TB_DebugTextBox.Text = str
        End If
    End Sub

End Class




Public Class GameOverLay
    Implements IDisposable

    Private DisplayedText As String
    Private DisplayTime As Integer
    Private OverlayXpos As Integer
    Private OverlayYpos As Integer
    Private OverlayWidth As Integer
    Private OverlayHeight As Integer
    Private FontSize As Double

    Private SceneCleared As Boolean
    Private divaHwnd As IntPtr

    Private vStopWatch As New Stopwatch

    Private _window As StickyWindow
    Private _windowSticky As StickyWindow
    Private ReadOnly _brushes As Dictionary(Of String, SolidBrush)
    Private ReadOnly _fonts As Dictionary(Of String, Font)

    Private _gridGeometry As Geometry
    Private _gridBounds As Rectangle

    Private gfx As Graphics

    Public Sub New(config As OverlayConfig)
        divaHwnd = GetWindowHandleByProcessName(config.ProcessName)
        WindowHelper.EnableBlurBehind(divaHwnd)

        DisplayedText = config.DisplayStr
        DisplayTime = config.DisplayTime
        OverlayHeight = config.height
        OverlayWidth = config.width
        OverlayXpos = config.xPos
        OverlayYpos = config.yPos
        FontSize = config.FontSize

        SceneCleared = False
        _brushes = New Dictionary(Of String, SolidBrush)()
        _fonts = New Dictionary(Of String, Font)()


    End Sub

    Public Function Initiate() As Boolean

        gfx = New Graphics() With {
            .MeasureFPS = False,
            .VSync = True,
            .PerPrimitiveAntiAliasing = True,
            .Height = 500,
            .Width = 500,
            .TextAntiAliasing = True
            }
        '    .WindowHandle = divaHwnd
        '}
        If divaHwnd = 0 Then
            MainForm.dbBox("Window handle not found. Cannot display overlay.")
            Finalize()
            Return False
            'Else
            '    _window = New GraphicsWindow(OverlayXpos, OverlayYpos, OverlayWidth, OverlayHeight, gfx)
            '    divaHwnd = IntPtr.Zero
        End If
        '_window = New GraphicsWindow(OverlayXpos, OverlayYpos, OverlayWidth, OverlayHeight, gfx)
        _window = New StickyWindow(100, 100, 100, 100, divaHwnd, gfx) 'Window size parameters do nothing when creating sticky windows
        With _window
            .FPS = 30
            .IsTopmost = True
            .IsVisible = True
            '_window.AttachToClientArea = True

        End With
        AddHandler _window.DestroyGraphics, AddressOf _window_DestroyGraphics
        AddHandler _window.DrawGraphics, AddressOf _window_DrawGraphics
        AddHandler _window.SetupGraphics, AddressOf _window_SetupGraphics

        vStopWatch.Start()
        Return True
    End Function

    Private Sub _window_SetupGraphics(sender As Object, e As SetupGraphicsEventArgs)
        Dim gfx = e.Graphics


        If e.RecreateResources Then
            For Each pair In _brushes
                pair.Value.Dispose()
            Next
        End If

        _brushes("background") = gfx.CreateSolidBrush(0, 0, 0, 120)
        _brushes("clear") = gfx.CreateSolidBrush(0, 0, 0, 0)

        _brushes("black") = gfx.CreateSolidBrush(0, 0, 0)
        _brushes("white") = gfx.CreateSolidBrush(255, 255, 255)
        _brushes("red") = gfx.CreateSolidBrush(255, 0, 0)
        _brushes("green") = gfx.CreateSolidBrush(0, 255, 0)
        _brushes("blue") = gfx.CreateSolidBrush(0, 0, 255)


        If e.RecreateResources Then Return

        '_fonts("arial") = gfx.CreateFont("Arial", 12)
        _fonts("consolas") = gfx.CreateFont("Consolas", FontSize)

        '_gridBounds = New Rectangle(20, 60, gfx.Width - 20, gfx.Height - 20)
        '_gridGeometry = gfx.CreateGeometry()

        'For x As Single = _gridBounds.Left To _gridBounds.Right Step 20
        '    Dim line = New Line(x, _gridBounds.Top, x, _gridBounds.Bottom)
        '    _gridGeometry.BeginFigure(line)
        '    _gridGeometry.EndFigure(False)
        'Next

        'For y As Single = _gridBounds.Top To _gridBounds.Bottom Step 20
        '    Dim line = New Line(_gridBounds.Left, y, _gridBounds.Right, y)
        '    _gridGeometry.BeginFigure(line)
        '    _gridGeometry.EndFigure(False)
        'Next

        '_gridGeometry.Close()

    End Sub

    Private Sub _window_DestroyGraphics(sender As Object, e As DestroyGraphicsEventArgs)
        For Each pair In _brushes
            pair.Value.Dispose()
        Next
        For Each pair In _fonts
            pair.Value.Dispose()
        Next
    End Sub

    Private Sub _window_DrawGraphics(sender As Object, e As DrawGraphicsEventArgs)
        '_window.Recreate()
        'WindowHelper.EnableBlurBehind(divaHwnd)

        Dim gfx = e.Graphics



        If vStopWatch.ElapsedMilliseconds > DisplayTime And SceneCleared = False Then
            gfx.ClearScene()
            SceneCleared = True
            Exit Sub
        ElseIf SceneCleared = True Then
            Dispose()
        End If


        Dim infotext As String = DisplayedText

        gfx.ClearScene(_brushes("clear"))
        'gfx.DrawTextWithBackground(_fonts("consolas"), _brushes("white"), _brushes("background"), 20, 320, infotext)
        'gfx.DrawRectangle(_brushes("background"), OverlayXpos, OverlayYpos, OverlayXpos + OverlayWidth, OverlayYpos + OverlayHeight, 1)
        gfx.FillRectangle(_brushes("background"), OverlayXpos, OverlayYpos, OverlayXpos + OverlayWidth, OverlayYpos + OverlayHeight)
        gfx.DrawText(_fonts("consolas"), FontSize, _brushes("white"), OverlayXpos + 24, OverlayYpos + 12, infotext)


        'For row As Single = _gridBounds.Top + 12 To _gridBounds.Bottom - 120 Step 120
        '    For column As Single = _gridBounds.Left + 12 To _gridBounds.Right - 120 Step 120
        '        DrawRandomFigure(gfx, column, row)
        '    Next
        'Next
    End Sub

    Public Sub Run()
        _window.Create()


        _window.Join()
    End Sub


#Region "IDisposable Support"

    Public Sub Dispose() Implements IDisposable.Dispose
        _window.Dispose()
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class

