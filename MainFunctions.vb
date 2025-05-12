
Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Text.RegularExpressions

Public Module MainFunction
    Async Function ScanModFolder(TopModFolderPath As String, SkipDisabledPacks As Boolean) As Task(Of Dictionary(Of String, CustomSong))
        'This function scans all song pack folders and then collects relavant data into a dictionary collection
        Dim SongDict As New Dictionary(Of String, CustomSong)

        Dim ModFolders() As String
        ModFolders = Directory.GetDirectories(TopModFolderPath)

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
                    Dim RegMatch As Match = SongRegex.Match(LineStr)

                    If RegMatch.Success = True Then
                        Dim ID As String = RegMatch.Groups(1).Value 'returns numeric portion of string
                        If CustomSong.OfficialSongCheck(ID) = False Then
                            If SongDict.ContainsKey(ID) = False Then
                                SongDict.Add(ID, New CustomSong(ID))
                                SongDict(ID).ModName = ModName
                                If ModNameExplicit = False Then
                                    SongDict(ID).LogError("Note: Mod name not specified in .toml file.")
                                End If
                                SongDict(ID).FolderPath = ModFolders(i)
                                SongDict(ID).dbPath = modDBpath
                                ParseSongParameter(ID, SongDict, DBarr(j), _CustomSongParameters)
                            Else
                                If SongDict(ID).dbPath <> modDBpath Then
                                    SongDict(ID).dbPath = modDBpath
                                End If
                                ParseSongParameter(ID, SongDict, DBarr(j), _CustomSongParameters)
                            End If
                        End If
                    End If
                Next j
            End If
        Next i

        Dim vanillaDB As String = _ini.EXEfolder & "\vanilla_pv_db.txt"
        If File.Exists(vanillaDB) Then '
            Dim DBarr() As String = File.ReadAllLines(vanillaDB)

            For j As Long = 0 To DBarr.LongLength - 1
                Dim LineStr As String = DBarr(j)

                Dim RegMatch As Match = SongRegex.Match(LineStr)
                Dim ID As String = RegMatch.Groups(1).Value 'returns numeric portion of string

                If SongDict.ContainsKey(ID) = False Then
                    SongDict.Add(ID, New CustomSong(ID))
                    SongDict(ID).ModName = "Vanilla"

                    'SongDict(ID).FolderPath = ModFolders(i)
                    SongDict(ID).dbPath = vanillaDB
                    ParseSongParameter(ID, SongDict, DBarr(j), _CustomSongParameters)
                    ParseSongParameter(ID, SongDict, DBarr(j), _CustomSongParameters)
                Else
                    If SongDict(ID).dbPath <> vanillaDB Then
                        SongDict(ID).dbPath = vanillaDB
                    End If
                    ParseSongParameter(ID, SongDict, DBarr(j), _CustomSongParameters)
                End If

            Next j
        End If

        Dim SongOffsets As String = _ini.EXEfolder & "\suggested_song_offsets.csv"
        If File.Exists(SongOffsets) Then '
            Dim StrArr() As String = File.ReadAllLines(SongOffsets)

            For j As Long = 0 To StrArr.LongLength - 1
                Dim LineStr As String = StrArr(j)
                Dim lineArr() = LineStr.Split(","c)

                Dim ID As String = lineArr(1)
                Dim offset As String = lineArr(2)

                If SongDict.ContainsKey(ID) = True Then
                    SongDict(ID).SuggestedSongOffset = offset
                End If
            Next j
        End If

        Return SongDict
    End Function

    Function ParseSongParameter(ID As String, SongDict As Dictionary(Of String, CustomSong), DBstr As String, CustomParameters As HashSet(Of String)) As String
        Dim ParamAndVal As String = DBstr.Substring(DBstr.IndexOf(".") + 1) 'removes pv_1234.

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
                SongObj.StoreCustomValue(SongParam, ParamVal)
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
            Case Else
                If CustomParameters.Contains(SongParam) Then
                    SongObj.StoreCustomValue(SongParam, ParamVal)
                End If
        End Select

        Return SongParam
    End Function
    Function ReadOverlayTemplate(templateStr As String) As HashSet(Of String)
        Dim paramSet As New HashSet(Of String)
        Dim regStr As String = "\{([^}]+)\}"
        Dim paramMatches As MatchCollection = Regex.Matches(templateStr, regStr)

        For Each match As Match In paramMatches
            paramSet.Add(match.Groups(1).Value)
        Next

        Return paramSet
    End Function
    Function ParseOverlayText(templateStr As String, songID As String) As String
        Dim DisplayStr As String = templateStr
        For Each paramStr As String In _CustomSongParameters
            DisplayStr = DisplayStr.Replace("{" & paramStr & "}", _FullSongDict(songID).GetCustomValue(paramStr))
        Next
        Return DisplayStr
    End Function
    Public Sub StartMainRoutine()
        _MonitorGame = True 'global var
        _RefreshOvlConfig = False

        Dim defaultConfig As New GameOverLay.OverlayConfig
        Dim ErrStr As String

        If _MainForm.GetOverlayConfig(defaultConfig, False) = False Then
            _MainForm.dbBox("Cannot start overlay. Input parameters invalid")
            Exit Sub
        End If

        If _MainForm.VerifySettings(ErrStr) = False Then
            _MainForm.dbBox(ErrStr)
            Exit Sub
        End If

        Dim MainWorkThread As New Task(Sub() MainRoutine(300, defaultConfig))
        MainWorkThread.Start()
    End Sub

    Public Async Sub MainRoutine(pingTime As Integer, defaultConfig As GameOverLay.OverlayConfig)
        Dim songID As Integer, IDstr As String, LastID As Integer
        While _MonitorGame = True 'global var
            If _pipeClient.IsConnected = False Then
                _MainForm.ResetConnection()
                _MainForm.SetStatus("lostConnection")
                If _MainForm.CB_ExitOnConnectionLost.Checked = True Then
                    _MainForm.dbBox("Program will close in 10. Uncheck box to leave it open.")
                    Await WaitAsync(10000)
                    If _MainForm.CB_ExitOnConnectionLost.Checked = True Then
                        Application.Exit()
                    End If
                End If

                Exit Sub
            Else
                Await WaitAsync(pingTime)
                songID = GetSongID()

                If songID <> -1 AndAlso LastID <> songID Then

                    LastID = songID
                    If songID < 1000 Then
                        IDstr = songID.ToString.PadLeft(3, "0"c)
                    Else
                        IDstr = songID
                    End If

                    Try
                        If _RefreshOvlConfig = True Then
                            If _MainForm.GetOverlayConfig(defaultConfig, False) = False Then
                                _MainForm.dbBox("ERROR: Cannot update overlay confing.Input parameters invalid")
                            End If
                            _RefreshOvlConfig = False
                        End If

                        defaultConfig.DisplayStr = ParseOverlayText(defaultConfig.TemplateStr, IDstr)

                        OverLayTextWindow(defaultConfig)
                    Catch ex As KeyNotFoundException
                        _MainForm.dbBox($"Error getting getting info for ID {IDstr}")
                    Catch ex As Exception
                        _MainForm.dbBox($"Unknown error occured when trying to present overlay.")
                    End Try
                ElseIf songID = -1 Then
                    LastID = 0 'this bit of logic makes it so that the song overlay appears if the song is replayed from the menu
                End If
            End If

        End While

    End Sub
    Public Sub OverlayNow(Optional AltBoxConfig As Boolean = False)
        Dim defaultConfig As GameOverLay.OverlayConfig

        If AltBoxConfig = False Then
            If _MainForm.GetOverlayConfig(defaultConfig, False) = False Then
                _MainForm.dbBox("Cannot start overlay. Input parameters invalid")
                Exit Sub
            End If
        Else
            _MainForm.GetOverlayConfig(defaultConfig, False, True)
        End If

        Dim songID As Integer, IDstr As String, LastID As Integer
        If _pipeClient.IsConnected = False Then
            _MainForm.dbBox("Not connected. Running test overaly instead...")
            _MainForm.OverlayTest(AltBoxConfig)
            Exit Sub
        Else
            songID = GetSongID()

            If songID = -1 Then
                _MainForm.dbBox("No song selected. Running overlay test instead.")
                _MainForm.OverlayTest(AltBoxConfig)
                Exit Sub
            ElseIf songID < 1000 Then
                IDstr = songID.ToString.PadLeft(3, "0"c)
            Else
                IDstr = songID
            End If
            Try
                defaultConfig.DisplayStr = ParseOverlayText(defaultConfig.TemplateStr, IDstr)

                OverLayTextWindow(defaultConfig)
            Catch ex As KeyNotFoundException
                _MainForm.dbBox($"Error getting getting info for ID {IDstr}")
            Catch ex As Exception
                _MainForm.dbBox($"Unknown error occured when trying to present overlay.")
            End Try
        End If

    End Sub

    Public Async Sub OverLayTextWindow(config As GameOverLay.OverlayConfig)

        Dim backgroundThread As New Thread(Sub()
                                               Dim overlay As New GameOverLay(config)
                                               If overlay.Initiate() = True Then overlay.Run()
                                           End Sub)
        backgroundThread.IsBackground = True
        backgroundThread.Start()

        Await WaitAsync(config.DisplayTime + 250)

        backgroundThread.Abort()
    End Sub
    Public Function StartPipeConnection() As Boolean
        Try
            _pipeClient.Connect(1000)
            Return True
        Catch ex As Exception
            _MainForm.dbBox("FAILED TO CONNECT.")
            Return False
        End Try

    End Function
    Public Function GetSongID() As Integer
        Try
            Dim message As Byte() = Encoding.ASCII.GetBytes("song-id")
            _pipeClient.Write(message, 0, message.Length)
            _pipeClient.Flush() ' Flush to ensure the message is sent

            ' Read 4 bytes as a response from the server
            Dim response(3) As Byte
            _pipeClient.Read(response, 0, response.Length)

            'Dim responseString As String = Encoding.ASCII.GetString(response)
            Dim result As Integer = BitConverter.ToInt32(response, 0)
            Return result

        Catch ex As Exception
            If _pipeClient.IsConnected = False Then
                _MainForm.dbBox("Communication failed. Pipe connection lost.")
            Else
                _MainForm.dbBox("Unknown pipe communication error." & vbCrLf & ex.Message)
            End If
            Return 0
        End Try
    End Function

    Async Function WaitAsync(milliseconds As Integer) As Task
        Await Task.Delay(milliseconds)
    End Function


End Module

