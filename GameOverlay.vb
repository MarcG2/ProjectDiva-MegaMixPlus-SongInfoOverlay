Imports GameOverlay.Windows
Imports GameOverlay.Drawing

Public Class GameOverLay
        Implements IDisposable

    Public Structure OverlayConfig
        Public DisplayStr As String
        Public TemplateStr As String
        Public DisplayTime As Integer
        Public xPos As Integer
        Public yPos As Integer
        Public width As Integer
        Public height As Integer
        Public FontSize As Double
        Public ProcessHwnd As String
        Public ProcessName As String
        Public boxRGBA() As Integer
        Public fontRGBA() As Integer
    End Structure


    Private _cfg As OverlayConfig
        Private _SceneCleared As Boolean
        Private _divaHwnd As IntPtr
        Private _vStopWatch As New Stopwatch

        Private _windowStandard As GraphicsWindow
        Private _windowSticky As StickyWindow

        Private ReadOnly _brushes As Dictionary(Of String, SolidBrush)
        Private ReadOnly _fonts As Dictionary(Of String, Font)

        Private _gfx As Graphics

    Public Sub New(config As OverlayConfig)
        '_divaHwnd = config.ProcessHwnd 'it may be worth limiting how many times the GetWindowHandle function runs for speed.  However the window handle can change while the game is running.
        _divaHwnd = GetWindowHandleByProcessName(config.ProcessName) 'config.ProcessHwnd
        _cfg = config

        _SceneCleared = False
        _brushes = New Dictionary(Of String, SolidBrush)()
        _fonts = New Dictionary(Of String, Font)()
    End Sub

    Public Function Initiate() As Boolean

        _gfx = New Graphics() With {
                .MeasureFPS = False,
                .VSync = False,
                .PerPrimitiveAntiAliasing = True,
                .TextAntiAliasing = True
                }

        If _divaHwnd = 0 Then
                _windowStandard = New GraphicsWindow(0, 0, 2000, 4000, _gfx) 'bounds given need to cover the area were graphics are being layed. Providing the exact bounds don't work for some reason.
                With _windowStandard
                    .FPS = 30
                    .IsTopmost = True
                    .IsVisible = True

                End With
                AddHandler _windowStandard.DestroyGraphics, AddressOf _window_DestroyGraphics
                AddHandler _windowStandard.DrawGraphics, AddressOf _window_DrawGraphics
                AddHandler _windowStandard.SetupGraphics, AddressOf _window_SetupGraphics

            Else
                _windowSticky = New StickyWindow(100, 100, 100, 100, _divaHwnd, _gfx) 'Window size parameters do nothing when creating sticky windows
                With _windowSticky
                    .FPS = 30
                    .IsTopmost = True
                    .IsVisible = True
                    '_window.AttachToClientArea = True
                End With

                AddHandler _windowSticky.DestroyGraphics, AddressOf _window_DestroyGraphics
                AddHandler _windowSticky.DrawGraphics, AddressOf _window_DrawGraphics
                AddHandler _windowSticky.SetupGraphics, AddressOf _window_SetupGraphics
            End If

            _vStopWatch.Start()
            Return True
        End Function

        Private Sub _window_SetupGraphics(sender As Object, e As SetupGraphicsEventArgs)
            Dim gfx = e.Graphics

            If e.RecreateResources Then
                For Each pair In _brushes
                    pair.Value.Dispose()
                Next
            End If

            With _cfg
                _brushes("textbox") = gfx.CreateSolidBrush(.boxRGBA(0), .boxRGBA(1), .boxRGBA(2), .boxRGBA(3))
                _brushes("font") = gfx.CreateSolidBrush(.fontRGBA(0), .fontRGBA(1), .fontRGBA(2), .fontRGBA(3))
                _brushes("clear") = gfx.CreateSolidBrush(0, 0, 0, 0)
                _brushes("black") = gfx.CreateSolidBrush(0, 0, 0)

            '_brushes("red") = gfx.CreateSolidBrush(255, 0, 0)
            '_brushes("green") = gfx.CreateSolidBrush(0, 255, 0)
            '_brushes("blue") = gfx.CreateSolidBrush(0, 0, 255)

            If e.RecreateResources Then Return

            _fonts("consolas") = gfx.CreateFont("Consolas", _cfg.FontSize)

        End With
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

            Dim gfx = e.Graphics

            If _vStopWatch.ElapsedMilliseconds > _cfg.DisplayTime And _SceneCleared = False Then
                gfx.ClearScene()
                _SceneCleared = True
                Exit Sub
            ElseIf _SceneCleared = True Then
                Dispose()
            End If

            Dim infotext As String = _cfg.DisplayStr

            gfx.ClearScene(_brushes("clear"))

            gfx.FillRectangle(_brushes("textbox"), _cfg.xPos, _cfg.yPos, _cfg.xPos + _cfg.width, _cfg.yPos + _cfg.height)
            gfx.DrawText(_fonts("consolas"), _cfg.FontSize, _brushes("font"), _cfg.xPos + 24, _cfg.yPos + 12, infotext)
        End Sub

        Public Sub Run()
            If _divaHwnd = 0 Then
                _windowStandard.Create()
                _windowStandard.Join()
            Else
                _windowSticky.Create()
                _windowSticky.Join()
            End If
        End Sub


#Region "IDisposable Support"
        Public Sub Dispose() Implements IDisposable.Dispose
            If _divaHwnd = 0 Then
                _windowStandard.Dispose()
            Else
                _windowSticky.Dispose()
            End If
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class

