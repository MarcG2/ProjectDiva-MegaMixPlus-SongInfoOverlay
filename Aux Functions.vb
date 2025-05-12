Imports System.Drawing.Imaging
Imports System.Media
Imports System.Runtime.InteropServices
Imports System.Threading

Public Module Aux_Functions

    Public Delegate Function EnumWindowsProc(ByVal hWnd As IntPtr, ByVal lParam As IntPtr) As Boolean

    <DllImport("user32.dll", SetLastError:=True)>
    Public Function EnumWindows(ByVal lpEnumFunc As EnumWindowsProc, ByVal lParam As IntPtr) As Boolean
    End Function

    ' Import GetWindowThreadProcessId function from user32.dll
    <DllImport("user32.dll", SetLastError:=True)>
    Private Function GetWindowThreadProcessId(ByVal hWnd As IntPtr, ByRef lpdwProcessId As UInteger) As UInteger
    End Function

    ' Function to get the main window handle by process name
    Public Function GetWindowHandleByProcessName(processName As String) As IntPtr
        Dim targetHwnd As IntPtr = IntPtr.Zero
        Dim targetProcess As Process() = Process.GetProcessesByName(processName)

        If targetProcess.Length = 0 Then
            Console.WriteLine("Process not found.")
            Return IntPtr.Zero
        End If

        Dim processId As Integer = targetProcess(0).Id

        ' EnumWindows callback function to check each window
        Dim enumFunc As EnumWindowsProc =
            Function(hWnd As IntPtr, lParam As IntPtr) As Boolean
                Dim windowProcessId As UInteger = 0
                GetWindowThreadProcessId(hWnd, windowProcessId)
                If windowProcessId = processId Then
                    targetHwnd = hWnd
                    Return False ' Stop enumeration once we find the handle
                End If
                Return True ' Continue enumeration
            End Function

        ' Enumerate all windows to find the one matching the process ID
        EnumWindows(enumFunc, IntPtr.Zero)

        Return targetHwnd
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Public Function GetWindowRect(hWnd As IntPtr, ByRef lpRect As RECT) As Boolean
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Public Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure


    Sub AudioNotification(Optional SoundOrPath As String = "[error, success, LightError, thunk]", Optional waitForAudioCompletion As Boolean = False)
        'v=2025-4-27

        Dim SoundFile As String
        Select Case SoundOrPath
            Case "error", "[error, success, LightError, thunk]"
                SoundFile = "C:\Windows\media\chord.wav"
            Case "success"
                SoundFile = "C:\Windows\media\Windows Print complete.wav"
            Case "LightError"
                SoundFile = "C:\Windows\Media\Windows Error.wav"
            Case "thunk"
                SoundFile = "C:\Windows\Media\Windows Information Bar.wav"
            Case Else
                SoundFile = SoundOrPath
        End Select


        Dim player As New Media.SoundPlayer()
        player.SoundLocation = SoundFile

        Try
            player.Load()
            If waitForAudioCompletion = True Then
                player.PlaySync() ' PlaySync() waits until the file is done playing
            Else
                player.Play() ' Play() for asynchronous playback
            End If
        Catch ex As Exception
            Console.WriteLine("Error playing sound: " & ex.Message)
        End Try
    End Sub

    Sub ScreenShotByRectangle(bounds As Rectangle, savePath As String, Optional mode As String = "[JPEG,PNG]")
        '2025-5-10
        Using bmp As New Bitmap(bounds.Width, bounds.Height)
            ' Create graphics from bitmap
            Using g As Graphics = Graphics.FromImage(bmp)
                ' Capture screen area into the bitmap
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size)
            End Using

            Select Case mode
                Case "[JPEG,PNG]", "JPEG"
                    Dim encoderParams As New EncoderParameters(1)
                    encoderParams.Param(0) = New EncoderParameter(Encoder.Quality, 90L)

                    bmp.Save(savePath, GetEncoder(ImageFormat.Jpeg), encoderParams)
                Case "PNG"
                    bmp.Save(savePath, ImageFormat.Png)
            End Select

        End Using

    End Sub

    Private Function GetEncoder(format As ImageFormat) As ImageCodecInfo
        For Each codec As ImageCodecInfo In ImageCodecInfo.GetImageDecoders()
            If codec.FormatID = format.Guid Then
                Return codec
            End If
        Next
        Return Nothing
    End Function

End Module


Public NotInheritable Class KeyboardHook
    Implements IDisposable

    ' Registers a hot key with Windows.
    <DllImport("user32.dll")>
    Private Shared Function RegisterHotKey(hWnd As IntPtr, id As Integer, fsModifiers As UInteger, vk As UInteger) As Boolean
    End Function

    ' Unregisters the hot key with Windows.
    <DllImport("user32.dll")>
    Private Shared Function UnregisterHotKey(hWnd As IntPtr, id As Integer) As Boolean
    End Function

    ''' <summary>
    ''' Represents the window that is used internally to get the messages.
    ''' </summary>
    Private Class Window
        Inherits NativeWindow
        Implements IDisposable
        Private Shared WM_HOTKEY As Integer = &H312

        Public Sub New()
            ' create the handle for the window.
            Me.CreateHandle(New CreateParams())
        End Sub

        Public Event KeyPressed As EventHandler(Of KeyPressedEventArgs)

        ''' <summary>
        ''' Overridden to get the notifications.
        ''' </summary>
        ''' <param name="m"></param>
        Protected Overrides Sub WndProc(ByRef m As Message)
            MyBase.WndProc(m)

            ' check if we got a hot key pressed.
            If m.Msg = WM_HOTKEY Then
                ' get the keys.
                Dim key As Keys = DirectCast((CInt(m.LParam) >> 16) And &HFFFF, Keys)
                Dim modifier As ModifierKeys = DirectCast(CUInt(CInt(m.LParam) And &HFFFF), ModifierKeys)

                ' invoke the event to notify the parent.
                RaiseEvent KeyPressed(Me, New KeyPressedEventArgs(modifier, key))
            End If
        End Sub

#Region " IDisposable Members"

        Public Sub Dispose() Implements IDisposable.Dispose
            Me.DestroyHandle()
        End Sub

#End Region
    End Class

    Private _window As New Window()
    Private _currentId As Integer

    Public Sub New()
        ' register the event of the inner native window.
        AddHandler _window.KeyPressed, Sub(sender As Object, args As KeyPressedEventArgs)
                                           RaiseEvent KeyPressed(Me, args)
                                       End Sub
    End Sub

    ''' <summary>
    ''' Registers a hot key in the system.
    ''' </summary>
    ''' <param name="modifier">The modifiers that are associated with the hot key.</param>
    ''' <param name="key">The key itself that is associated with the hot key.</param>
    Public Sub RegisterHotKey(modifier As ModifierKeys, key As Keys)
        ' increment the counter.
        _currentId = _currentId + 1

        ' register the hot key.
        If Not RegisterHotKey(_window.Handle, _currentId, DirectCast(modifier, UInteger), CUInt(key)) Then
            Throw New InvalidOperationException("Couldn’t register the hot key.")
            'or use MsgBox("Couldn’t register the hot key.")
        End If
    End Sub

    ''' <summary>
    ''' A hot key has been pressed.
    ''' </summary>
    Public Event KeyPressed As EventHandler(Of KeyPressedEventArgs)

#Region " IDisposable Members"

    Public Sub Dispose() Implements IDisposable.Dispose
        ' unregister all the registered hot keys.
        Dim i As Integer = _currentId
        While i > 0
            UnregisterHotKey(_window.Handle, i)
            System.Math.Max(System.Threading.Interlocked.Decrement(i), i + 1)
        End While

        ' dispose the inner native window.
        _window.Dispose()
    End Sub

#End Region
End Class

''' <summary>
''' Event Args for the event that is fired after the hot key has been pressed.
''' </summary>
Public Class KeyPressedEventArgs
    Inherits EventArgs
    Private _modifier As ModifierKeys
    Private _key As Keys

    Friend Sub New(modifier As ModifierKeys, key As Keys)
        _modifier = modifier
        _key = key
    End Sub

    Public ReadOnly Property Modifier() As ModifierKeys
        Get
            Return _modifier
        End Get
    End Property

    Public ReadOnly Property Key() As Keys
        Get
            Return _key
        End Get
    End Property
End Class

''' <summary>
''' The enumeration of possible modifiers.
''' </summary>
<Flags>
Public Enum ModifierKeys As UInteger
    Alt = 1
    Control = 2
    Shift = 4
    Win = 8
End Enum
