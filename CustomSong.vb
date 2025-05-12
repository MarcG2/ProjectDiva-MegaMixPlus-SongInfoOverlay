
Public Class CustomSong
    Private SongErrorLogVal As String = ""
    Private SongNameVal As String = ""
    Private ArtistVal As String = ""
    Private dbPathVal As String = "" ' mod_pv_db.txt
    Private FolderPathVal As String = ""
    Private ModNameVal As String = ""
    Private IDval As String = ""
    Private HiddenVal As Boolean = False
    Private DifficultiesArrVal(0 To 4) As String
    Private NumDataBasesVal As Integer = 0
    Private ErrorStateVal As ErrorStruct
    Private CustomParamDict As New Dictionary(Of String, String)
    Public SuggestedSongOffset As String

    Public Enum Difficulties
        Easy
        Normal
        Hard
        Extreme
        ExEx
    End Enum

    Public Structure ErrorStruct
        Public ContainsContinuityError As Boolean

    End Structure
    Public Sub New(ID As String)
        IDval = ID
        ErrorStateVal.ContainsContinuityError = False

    End Sub

    Public ReadOnly Property ErrorStates As ErrorStruct
        Get
            Return ErrorStateVal
        End Get
    End Property

    Public ReadOnly Property DifficultyArr As String()
        Get
            Dim DifReformattedArr(4) As String
            For i As Integer = 0 To 4
                If DifficultiesArrVal(i) <> "" Then
                    DifReformattedArr(i) = Replace(Right(DifficultiesArrVal(i), 4), "_", ".", 1).TrimStart({"0"c})
                End If
            Next
            Return DifReformattedArr
        End Get
    End Property

    Public Sub AddDifficulty(val As String, dif As Difficulties)
        Select Case dif
            Case Difficulties.ExEx
                DifficultiesArrVal(4) = CheckEmptyProp(DifficultiesArrVal(4), val, dif)
            Case Difficulties.Extreme
                DifficultiesArrVal(3) = CheckEmptyProp(DifficultiesArrVal(3), val, dif)
            Case Difficulties.Hard
                DifficultiesArrVal(2) = CheckEmptyProp(DifficultiesArrVal(2), val, dif)
            Case Difficulties.Normal
                DifficultiesArrVal(1) = CheckEmptyProp(DifficultiesArrVal(1), val, dif)
            Case Difficulties.Easy
                DifficultiesArrVal(0) = CheckEmptyProp(DifficultiesArrVal(0), val, dif)

        End Select

    End Sub

    Public ReadOnly Property NumDataBases As Integer
        Get
            Return NumDataBasesVal
        End Get
    End Property
    Public ReadOnly Property SongErrorLog As String
        Get
            Return SongErrorLogVal
        End Get
    End Property

    Public ReadOnly Property BaseGameSong As Boolean
        Get
            Return OfficialSongCheck(IDval)
        End Get
    End Property

    Public ReadOnly Property ID As String
        Get
            Return IDval
        End Get
    End Property
    Public Property Hidden() As Boolean
        Get
            Return HiddenVal
        End Get
        Set(value As Boolean)
            HiddenVal = value
        End Set
    End Property

    Public Property SongName() As String
        Get
            Return SongNameVal
        End Get
        Set(value As String)
            SongNameVal = CheckEmptyProp(SongNameVal, value, "EnglishSongName")
        End Set
    End Property
    Public Property Artist() As String
        Get
            Return ArtistVal
        End Get
        Set(value As String)
            ArtistVal = value
        End Set
    End Property
    Public Property dbPath() As String
        Get
            Return dbPathVal
        End Get
        Set(value As String)
            If dbPathVal = "" Then
                dbPathVal = value
                NumDataBasesVal = NumDataBasesVal + 1
            Else
                Dim DBarr() As String = Split(dbPathVal, vbCrLf)
                Dim IsContained As Boolean = False
                For i As Integer = 0 To DBarr.Length - 1
                    If value = DBarr(i) Then
                        IsContained = True
                        Exit For
                    End If
                Next i
                If IsContained = False Then
                    dbPathVal = dbPathVal & vbCrLf & value
                    NumDataBasesVal = NumDataBasesVal + 1
                End If

            End If
        End Set

    End Property
    Public Property FolderPath() As String
        Get
            Return FolderPathVal
        End Get
        Set(value As String)
            FolderPathVal = value
        End Set

    End Property
    Public Property ModName() As String
        Get
            Return ModNameVal
        End Get
        Set(value As String)
            ModNameVal = value
        End Set

    End Property

    Private Function CheckEmptyProp(CurrentVal As String, NewVal As String, PropName As Object) As String
        Dim PropStr As String
        If PropName.GetType.IsEnum = True Then
            PropStr = NameOf(PropName)
        Else
            PropStr = PropName
        End If

        If CurrentVal <> "" Then
            Dim ErrStr As String
            If CurrentVal = NewVal Then
                ErrStr = $"{PropName} has duplicate entries. Values are the same."
            Else
                ErrStr = $"{PropName} has duplicate entries. Val1 = {CurrentVal}. Val2 = {NewVal}"
            End If

            LogError(ErrStr)
        End If
        Return NewVal
    End Function

    Public Sub LogError(str As String)
        If SongErrorLogVal = "" Then
            SongErrorLogVal = str
        ElseIf Left(SongErrorLogVal, 1) <> "▼" Then
            SongErrorLogVal = "▼" & SongErrorLog & "   " & vbCrLf & str
        Else
            SongErrorLogVal = SongErrorLog & vbCrLf & str
        End If
    End Sub

    Public Shared Function OfficialSongCheck(ID As Integer) As Boolean
        If ID > 832 Then
            Return False
        End If

        Dim OfficialIDs() As String = {"1-25", "27-32", "37 - 68", "79", "81-97", "101-104",
                                            "201-216", "218-228", "231-236", "238-244", "246-251", "253-255", "257", "259-263", "265-281",
                                            "401-405", "407-443", "600-605", "607-631", "637-642", "710", "722-734", "736-740", "832"}

        For i As Integer = 0 To OfficialIDs.Length - 1
            Dim LowerNum As Integer, UpperNum As Integer

            If OfficialIDs(i).Contains("-") Then
                LowerNum = Split(OfficialIDs(i), "-")(0)
                UpperNum = Split(OfficialIDs(i), "-")(1)

                If (ID >= LowerNum AndAlso ID <= UpperNum) Then
                    Return True
                End If

            Else
                LowerNum = OfficialIDs(i)

                If ID = LowerNum Then
                    Return True
                End If
            End If

        Next i

        OfficialSongCheck = False
    End Function

    Public Function StoreCustomValue(param As String, value As String) As Boolean

        If CustomParamDict.ContainsKey(param) = False Then
            CustomParamDict.Add(param, value)
            Return True
        Else
            Return False
        End If

    End Function
    Public Function GetCustomValue(param As String) As String
        If param = "id" Then
            Return ID
        ElseIf param = "mod" Then
            Return ModName
        ElseIf param = "suggested_offset" Then
            If SuggestedSongOffset <> "" Then
                Return "Suggested Offset: " & SuggestedSongOffset
            Else
                Return ""
            End If

        ElseIf CustomParamDict.ContainsKey(param) Then
            Return CustomParamDict(param)
        Else
            Return ""
        End If

    End Function
End Class
