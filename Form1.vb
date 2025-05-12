Option Explicit On

Imports System.IO
Imports System.Threading
Imports WindowsAPICodePack.Dialogs
Imports System.IO.Pipes
Imports System.Text.RegularExpressions
Imports IniParser


Public Module GlobalVariables
    Public _ini As iniWrapper
    Public _pipeClient As NamedPipeClientStream
    Public _MainForm As formOverlayMenu
    Public _MonitorGame As Boolean
    Public _RefreshOvlConfig As Boolean

    Public _FullSongDict As New Dictionary(Of String, CustomSong) 'Key str is Song ID
    Public _CustomSongParameters As HashSet(Of String)
    'Public _DivaGameHwnd As IntPtr = 0

End Module


Public Class formOverlayMenu
    Private hook1 As New KeyboardHook
    Private hook2 As New KeyboardHook
    Private OverlayObj As GameOverLay
    Private CTS As CancellationTokenSource
    Private finishCheck As Boolean
    Private modFolderScanned As Boolean
    Private DivaProcessName As String
    Private OverlayHelpStr As String

    Private Async Sub formOverlayMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '_____Reading in setting from .ini file_______
        _ini = New iniWrapper(, True) 'looks .ini in EXE directory. Generates a new file from template if missing
        TB_GameFolder.Text = _ini.Entry("GameFolder")
        TB_OverlayHeight.Text = _ini.Entry("OverlayHeight")

        TB_OverlayWidth.Text = _ini.Entry("OverlayWidth")
        TB_OverlayXpos.Text = _ini.Entry("Xpos")
        TB_OverlayYpos.Text = _ini.Entry("Ypos")
        TB_OverlayDisplayTime.Text = _ini.Entry("DisplayTime")
        TB_FontSize.Text = _ini.Entry("FontSize")
        TB_OverlayTemplate.Text = _ini.Entry("OverlayTemplate").Replace("`n", vbCrLf)
        TB_FontColor.Text = _ini.Entry("FontARGB")
        TB_BoxColor.Text = _ini.Entry("BoxARGB")
        CB_ScreenShot.Checked = _ini.Entry("ScreenshotOnCtrl+2")
        DivaProcessName = _ini.Entry("ProcessName")
        CB_ExitOnConnectionLost.Checked = True

        _MainForm = Me
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.Icon = New Icon(_ini.EXEfolder & "\assets\MM+OVLv3.ico")
        ToolTip1.SetToolTip(lblXpos, "Measured from the left of the screen in pixels. Use the test overlay button to verify placement.")
        ToolTip1.SetToolTip(lblYpos, "Measured from the top of the screen in pixels. Use the test overlay button to verify placement.")
        ToolTip1.SetToolTip(lblFontColor, "Go to rgbcolorpicker.com to get help with selecting appropriate RGB values. The alpha channel should remain at 255 to be fully opaque.")
        ToolTip1.SetToolTip(lblBoxColor, "Go to rgbcolorpicker.com to get help with selecting appropriate RGB values. The 4th value is the alpha channel which controls transparency.")

        ToolTip1.AutoPopDelay = 10000 'how long tooltip is visible in ms
        ToolTip1.InitialDelay = 100
        ToolTip1.ReshowDelay = 100

        _CustomSongParameters = ReadOverlayTemplate(TB_OverlayTemplate.Text)

        _pipeClient = New NamedPipeClientStream(".", "MM+ OVL", PipeDirection.InOut)

        AddGlobalHotkeySupport() 'enables hot key functionality

        CTS = New CancellationTokenSource()
        CTS.Cancel()
        finishCheck = True
        modFolderScanned = Await Task.Run(Function() ScanStart())
        If modFolderScanned = True Then
            If _ini.Entry("AttemptConnectAtLaunch") = True Then
                But_StartOveralyThread_Click(New Object, New EventArgs)
            End If
        Else
            But_TestOverlay.Enabled = False
            But_StartOveralyThread.Enabled = False
            But_SendTest.Enabled = False
        End If

    End Sub

    Private Sub formOverlayMenu_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = New Icon(_ini.EXEfolder & "\assets\MM+OVLv3.ico")
            NotifyIcon1.Text = "MM+ Overlay"
            NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
            NotifyIcon1.BalloonTipTitle = "MM+ Overlay"
            ShowInTaskbar = False
        End If
    End Sub
    Private Sub NotifyIcon1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotifyIcon1.DoubleClick
        ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
        NotifyIcon1.Visible = False
    End Sub

    Private Async Function ScanStart() As Task(Of Boolean)
        Dim GameFolderPath As String = TB_GameFolder.Text
        Dim ModFolderPath As String = GameFolderPath & "\mods"

        If Directory.Exists(ModFolderPath) = True Then
            _FullSongDict = ScanModFolder(ModFolderPath, True).Result
            Return True
        Else

            Dim ModEXEfolder As String = _ini.EXEfolder
            Dim Modsfolder As String = Path.GetDirectoryName(Path.GetDirectoryName(ModEXEfolder)) 'If the mod is installed the correctly, the 
            If Strings.Right(Modsfolder, 4) = "mods" Then
                TB_GameFolder.Invoke(Sub() TB_GameFolder.Text = Path.GetDirectoryName(Modsfolder))
                _ini.Entry("GameFolder") = TB_GameFolder.Text
                Return ScanStart().Result
            Else
                dbBox("ERROR. Cannnot locate game folder automatically. Set it manually.")
                Return False
            End If
        End If

    End Function

    Public Function GetOverlayConfig(ByRef config As GameOverLay.OverlayConfig, showMsgBox As Boolean, Optional GetAltConfig As Boolean = False) As Boolean
        'This function parses the current text box values in the menu and assigns them to structure OverlayConfig

        Try
            If GetAltConfig = False Then
                With config
                    .TemplateStr = TB_OverlayTemplate.Text
                    .height = TB_OverlayHeight.Text
                    .width = TB_OverlayWidth.Text
                    .xPos = TB_OverlayXpos.Text
                    .yPos = TB_OverlayYpos.Text
                    .DisplayTime = TB_OverlayDisplayTime.Text
                    .FontSize = TB_FontSize.Text
                    '.ProcessHwnd = _DivaGameHwnd
                    .ProcessName = DivaProcessName
                    .boxRGBA = Array.ConvertAll(TB_BoxColor.Text.Split(","c), Function(s) Integer.Parse(s))
                    .fontRGBA = Array.ConvertAll(TB_FontColor.Text.Split(","c), Function(s) Integer.Parse(s))
                End With
            Else
                With config
                    .TemplateStr = _ini.Entry("OverlayTemplate", "AltBoxConfig").Replace("`n", vbCrLf)
                    .height = _ini.Entry("OverlayHeight", "AltBoxConfig")
                    .width = _ini.Entry("OverlayWidth", "AltBoxConfig")
                    .xPos = _ini.Entry("Xpos", "AltBoxConfig")
                    .yPos = _ini.Entry("Ypos", "AltBoxConfig")
                    .DisplayTime = _ini.Entry("DisplayTime", "AltBoxConfig")
                    .FontSize = _ini.Entry("FontSize", "AltBoxConfig")
                    '.ProcessHwnd = _DivaGameHwnd
                    .ProcessName = DivaProcessName
                    .boxRGBA = Array.ConvertAll(_ini.Entry("BoxARGB", "AltBoxConfig").Split(","c), Function(s) Integer.Parse(s))
                    .fontRGBA = Array.ConvertAll(_ini.Entry("FontARGB", "AltBoxConfig").Split(","c), Function(s) Integer.Parse(s))
                End With
            End If

            Return True
        Catch ex As Exception
            If showMsgBox = True Then
                MsgBox("Invalid overlay parameters given.", vbCritical)
            Else
                dbBox("Overlay input config ERROR.")
            End If

            Return False
        End Try
    End Function
    Private Sub But_TestOverlay_Click(sender As Object, e As EventArgs) Handles But_TestOverlay.Click
        OverlayTest(False)
    End Sub

    Private Sub But_OpenINI_Click(sender As Object, e As EventArgs) Handles But_OpenINI.Click
        _ini.OpenINI()
    End Sub

    Private Sub But_OpenEXEfolder_Click(sender As Object, e As EventArgs) Handles But_OpenEXEfolder.Click
        _ini.OpenINI(OpenFolder:=True)
    End Sub

    Private Sub But_SelectFolder_Click(sender As Object, e As EventArgs) Handles But_SelectFolder.Click

        Using dialog As New CommonOpenFileDialog
            dialog.IsFolderPicker = True
            dialog.Title = "Select a Folder"

            If dialog.ShowDialog() = CommonFileDialogResult.Ok Then
                TB_GameFolder.Text = dialog.FileName
                _ini.Entry("GameFolder") = TB_GameFolder.Text
            End If
        End Using
    End Sub

    Private Sub But_SaveSettings_Click(sender As Object, e As EventArgs) Handles But_SaveSettings.Click
        Dim ErrStr As String
        If VerifySettings(ErrStr) = False Then
            MsgBox(ErrStr)
            Exit Sub
        End If

        Dim DisplayTime As Integer = Integer.Parse(TB_OverlayDisplayTime.Text, DisplayTime)
        Dim Height As Integer = Integer.Parse(TB_OverlayHeight.Text, Height)
        Dim Width As Integer = Integer.Parse(TB_OverlayWidth.Text, Width)
        Dim xPos As Integer = Integer.Parse(TB_OverlayXpos.Text, xPos)
        Dim yPos As Integer = Integer.Parse(TB_OverlayYpos.Text, yPos)
        Dim FontSize As Double = Double.Parse(TB_FontSize.Text, FontSize)
        Dim OverlayTemplate As String = TB_OverlayTemplate.Text.Replace(vbCrLf, "`n")
        Dim FontColor As String = TB_FontColor.Text
        Dim BoxColor As String = TB_BoxColor.Text
        Dim ScreenshotEnable As Boolean = CB_ScreenShot.Checked

        Dim ini As Model.IniData = _ini.iniDict
        ini("Main")("GameFolder") = TB_GameFolder.Text
        ini("Main")("DisplayTime") = DisplayTime
        ini("Main")("OverlayHeight") = Height
        ini("Main")("OverlayWidth") = Width
        ini("Main")("Xpos") = xPos
        ini("Main")("Ypos") = yPos
        ini("Main")("FontSize") = FontSize
        ini("Main")("OverlayTemplate") = OverlayTemplate
        ini("Main")("FontARGB") = FontColor
        ini("Main")("BoxARGB") = BoxColor
        ini("Main")("ScreenshotOnCtrl+2") = ScreenshotEnable
        _ini.SaveFile()

        _RefreshOvlConfig = True 'Trigger to reload overlay config currently set in the menu
        dbBox("Settings Saved")
    End Sub

    Public Sub OverlayTest(Optional AltTextBox As Boolean = False)
        Dim ErrStr As String
        If VerifySettings(ErrStr) = False Then
            ErrStr = "Invalid input parameters:" & vbCrLf & ErrStr
            MsgBox(ErrStr, vbCritical)

            Exit Sub
        End If

        Dim DisplayStr As String
        If AltTextBox = False Then
            DisplayStr = TB_OverlayTemplate.Text
        ElseIf AltTextBox = True Then
            DisplayStr = _ini.Entry("OverlayTemplate", "AltBoxConfig").Replace("`n", vbCrLf)
        End If

        For Each paramStr As String In _CustomSongParameters
            DisplayStr = DisplayStr.Replace("{" & paramStr & "}", _FullSongDict("002").GetCustomValue(paramStr))
        Next

        Dim cfg As GameOverLay.OverlayConfig
        If GetOverlayConfig(cfg, True, AltTextBox) = True Then
            cfg.DisplayStr = DisplayStr
            OverLayTextWindow(cfg)
        End If

    End Sub
    Public Function VerifySettings(ByRef ErrStr As String) As Boolean

        ErrStr = ""
        VerifySettings = True
        Dim res As Boolean
        Dim DisplayTime As Integer : res = Integer.TryParse(TB_OverlayDisplayTime.Text, DisplayTime)
        If res = False Or DisplayTime < 200 Or DisplayTime > 10000 Then
            ErrStr = "Display time is measured in ms. Valid range is 200 to 10000 ms." & vbCrLf
        End If

        Dim Height As Integer : res = Integer.TryParse(TB_OverlayHeight.Text, Height)
        If res = False Or Height < 10 Or Height > 1000 Then
            ErrStr = ErrStr & "Overlay height is measured in pixels. Valid range is 30 to 600." & vbCrLf
        End If
        Dim Width As Integer : res = Integer.TryParse(TB_OverlayWidth.Text, Width)
        If res = False Or Width < 50 Or Width > 1500 Then
            ErrStr = ErrStr & "Overlay Width is measured in pixels. Valid range is 50 to 800." & vbCrLf

        End If

        Dim xPos As Integer : res = Integer.TryParse(TB_OverlayXpos.Text, xPos)
        If res = False Then
            ErrStr = ErrStr & "Overlay x position is measured in from the left of the main monitor and is measured in pixels." & vbCrLf

        End If
        Dim yPos As Integer : res = Integer.TryParse(TB_OverlayYpos.Text, yPos)
        If res = False Then
            ErrStr = ErrStr & "Overlay y position is measured in from the top of the main monitor and is measured in pixels." & vbCrLf

        End If
        Dim FontSize As Integer : res = Integer.TryParse(TB_FontSize.Text, FontSize)
        If res = False Or FontSize < 6 Or FontSize > 128 Then
            ErrStr = ErrStr & "Font size is measured in points. Valid range is 8 to 64 pts." & vbCrLf
        End If


        Dim regex As New Regex("^\d{1,3},\d{1,3},\d{1,3},\d{1,3}$")

        TB_BoxColor.Text = TB_BoxColor.Text.Replace(" "c, "")
        Dim BoxColor As String = TB_BoxColor.Text

        If regex.IsMatch(BoxColor) = True Then
            Dim rgba() As String = BoxColor.Split(",")
            For Each val As String In rgba
                If val > 255 Then
                    res = False
                    Exit For
                End If
            Next
        Else
            res = False
        End If
        If res = False Then
            ErrStr = ErrStr & "Text Box Color RGBA input invalid. Input must consist of 4 integers from 0-255." & vbCrLf
        End If

        TB_FontColor.Text = TB_FontColor.Text.Replace(" "c, "")
        Dim FontColor As String = TB_FontColor.Text
        If regex.IsMatch(FontColor) = True Then
            Dim rgba() As String = FontColor.Split(",")
            For Each val As String In rgba
                If val > 255 Then
                    res = False
                    Exit For
                End If
            Next
        Else
            res = False
        End If
        If res = False Then
            ErrStr = ErrStr & "Font Color RGBA input invalid. Input must consist of 4 integers from 0-255." & vbCrLf
        End If

        If ErrStr <> "" Then Return False

    End Function

    Private Sub But_SendTest_Click(sender As Object, e As EventArgs) Handles But_SendTest.Click
        dbBox("Current Song: " & GetSongID())

    End Sub

    Public Sub ResetConnection()
        CTS.Cancel()
        If But_StartOveralyThread.InvokeRequired = True Then
            Me.Invoke(Sub() But_StartOveralyThread.Text = "Connect and Start Overlay")
        Else
            But_StartOveralyThread.Text = "Connect and Start Overlay"
        End If

    End Sub
    Private Async Sub But_StartOveralyThread_Click(sender As Object, e As EventArgs) Handles But_StartOveralyThread.Click
        If CTS.IsCancellationRequested = True And finishCheck = True Then
            But_StartOveralyThread.Text = "Cancel Connection Wait"
            CTS = New CancellationTokenSource()
            Await Task.Run(Function() WaitForConnection(CTS.Token))
            If CTS.IsCancellationRequested = False Then
                StartMainRoutine()
            End If
        Else
            finishCheck = False
            ResetConnection()
            SetStatus("unconnected")
        End If
    End Sub
    Private Async Function WaitForConnection(ct As CancellationToken) As Task
        Dim ConnectAttempts As Integer = 0
        'SetStatus("waiting")
        While True
            If ct.IsCancellationRequested = True Then
                'But_Indicator.BackColor = Drawing.Color.White
                finishCheck = True
                Exit Function
            End If
            Dim timeOutPeriod As Integer = 4000
            Try
                If _pipeClient.IsConnected = False Then
                    SetStatus("attemptingConnect")
                    _pipeClient.Connect(timeOutPeriod)
                End If
                SetStatus("connected")
                '_DivaGameHwnd = GetWindowHandleByProcessName(DivaProcessName)
                Exit Function
            Catch ex As IOException
                'SetStatus("waiting") 'no reason to indicate a "waiting state" if only one connect attempt will be made.
                'a timeout occurred. will retry after wait
                dbBox($"Connection timeout occurred ({timeOutPeriod} ms)")
            Catch ex As Exception
                Stop
            End Try
            ConnectAttempts += 1
            'Await WaitAsync(4000)

            If ConnectAttempts = 1 Then 'For some reason starting the game while attempting a connection tends to fail and prevent connection entirely. So for now there's no reason to repeatedly retry connections.
                But_StartOveralyThread_Click(New Object, New EventArgs)
                Await WaitAsync(100)
            End If
        End While

    End Function
    Public Sub AddGlobalHotkeySupport()
        ' register the event that is fired after the key press.
        AddHandler hook1.KeyPressed, AddressOf keyPress_OverlayNow
        AddHandler hook2.KeyPressed, AddressOf keyPress_OverlayNow

        hook1.RegisterHotKey(MMplus_Song_Info_Overlay.ModifierKeys.Control, Keys.D1)
        hook2.RegisterHotKey(MMplus_Song_Info_Overlay.ModifierKeys.Control, Keys.D2)
    End Sub
    Public Sub RemoveGlobalHotkeySupport()
        ' unregister all registered hot keys.
        hook1.Dispose()
        hook2.Dispose()
    End Sub

    Private Sub keyPress_OverlayNow(sender As Object, e As KeyPressedEventArgs)
        If e.Key = Keys.D1 Then
            OverlayNow(False)
        ElseIf e.Key = Keys.D2 Then
            OverlayNow(True)
            If CB_ScreenShot.Checked = True Then
                Thread.Sleep(300)
                TakeScreenShot()
            End If
        End If
    End Sub


    Public Sub dbBox(str As String, Optional mode As String = "[prepend,append,replace]")
        Select Case mode
            Case "[prepend,append,replace]", "prepend"
                str = str & vbCrLf & TB_DebugTextBox.Text
            Case "append"
                If TB_DebugTextBox.Text <> "" Then
                    str = TB_DebugTextBox.Text & vbCrLf & str
                End If
            Case "replace"
                'leave as is
        End Select

        If Me.InvokeRequired = True Then
            Me.Invoke(Sub() TB_DebugTextBox.Text = str)
        Else
            TB_DebugTextBox.Text = str
        End If
    End Sub

    Public Sub SetStatus(Optional status As String = "[unconnected,waiting,connected,lostConnection,attemptingConnect]")
        Dim color As Drawing.Color, labelStr As String
        Select Case status
            Case "unconnected"
                color = Drawing.Color.White
                labelStr = "Not connected"
            Case "waiting"
                color = Drawing.Color.Orange
                labelStr = "Awaiting connection"
            Case "connected"
                color = Drawing.Color.Green
                labelStr = "Connected"
            Case "lostConnection"
                color = Drawing.Color.Red
                labelStr = "LOST connection. Restart Game"
            Case "attemptingConnect"
                color = Drawing.Color.Orange
                labelStr = "attempting connection now..."
            Case Else
                Stop
        End Select

        Me.Invoke(Sub()
                          But_Indicator.BackColor = color
                          Lab_Status.Text = labelStr
                          Lab_Status.Refresh()
                      If status = "connected" Then
                          But_StartOveralyThread.Enabled = False
                          If _ini.Entry("HideWhenConnected") = True Then
                              Me.WindowState = FormWindowState.Minimized
                          End If
                      End If
                  End Sub)

    End Sub

    Private Sub formOverlayMenu_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        RemoveGlobalHotkeySupport()
    End Sub

    Private Sub lblOverlayHelp_Click(sender As Object, e As EventArgs) Handles lblOverlayHelp.Click
        Dim helpFile As String = _ini.EXEfolder & "\help.dat"
        Try

            Dim helpText As String = File.ReadAllText(helpFile)
            MsgBox(helpText, MsgBoxStyle.OkOnly, "Help")
        Catch ex As Exception
            AudioNotification("error")
            dbBox("HELP FILE MISSING")
        End Try

    End Sub

    Private Sub TakeScreenShot()
        Dim r As RECT
        Dim DivaHwnd As IntPtr = GetWindowHandleByProcessName(DivaProcessName)
        If GetWindowRect(DivaHwnd, r) = True Then
            Dim bounds As New Rectangle(r.Left, r.Top, r.Right - r.Left, r.Bottom - r.Top)
            Dim currentDateTime As String = Now.ToString("yyyy-MM-dd, HH-mm-ss")
            Dim saveFolder As String = _ini.Entry("ScreenShotSaveLocation")
            If saveFolder.Contains("%") Then
                Dim var As String = Regex.Match(saveFolder, "%(.*?)%").Groups(1).Value
                Dim val As String = Environment.GetEnvironmentVariable(var)
                saveFolder = saveFolder.Replace("%" & var & "%", val)
            End If

            ScreenShotByRectangle(bounds, saveFolder & "\" & currentDateTime & "_DivaCaputure.jpg")
        End If
    End Sub
End Class



