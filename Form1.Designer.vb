<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class formOverlayMenu
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.But_TestOverlay = New System.Windows.Forms.Button()
        Me.TB_DebugTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.But_OpenINI = New System.Windows.Forms.Button()
        Me.But_SelectFolder = New System.Windows.Forms.Button()
        Me.But_OpenEXEfolder = New System.Windows.Forms.Button()
        Me.TB_GameFolder = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TB_OverlayHeight = New System.Windows.Forms.TextBox()
        Me.TB_OverlayWidth = New System.Windows.Forms.TextBox()
        Me.TB_OverlayYpos = New System.Windows.Forms.TextBox()
        Me.TB_OverlayXpos = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblYpos = New System.Windows.Forms.Label()
        Me.lblXpos = New System.Windows.Forms.Label()
        Me.TB_OverlayDisplayTime = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.But_SaveSettings = New System.Windows.Forms.Button()
        Me.But_SendTest = New System.Windows.Forms.Button()
        Me.TB_FontSize = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.But_StartOveralyThread = New System.Windows.Forms.Button()
        Me.TB_OverlayTemplate = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.But_Indicator = New System.Windows.Forms.Button()
        Me.Lab_Status = New System.Windows.Forms.Label()
        Me.TB_FontColor = New System.Windows.Forms.TextBox()
        Me.lblFontColor = New System.Windows.Forms.Label()
        Me.lblBoxColor = New System.Windows.Forms.Label()
        Me.TB_BoxColor = New System.Windows.Forms.TextBox()
        Me.CB_ExitOnConnectionLost = New System.Windows.Forms.CheckBox()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblOverlayHelp = New System.Windows.Forms.Label()
        Me.CB_ScreenShot = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'But_TestOverlay
        '
        Me.But_TestOverlay.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.But_TestOverlay.Location = New System.Drawing.Point(15, 194)
        Me.But_TestOverlay.Name = "But_TestOverlay"
        Me.But_TestOverlay.Size = New System.Drawing.Size(120, 36)
        Me.But_TestOverlay.TabIndex = 0
        Me.But_TestOverlay.Text = "Test Overlay"
        Me.But_TestOverlay.UseVisualStyleBackColor = False
        '
        'TB_DebugTextBox
        '
        Me.TB_DebugTextBox.Location = New System.Drawing.Point(279, 95)
        Me.TB_DebugTextBox.Multiline = True
        Me.TB_DebugTextBox.Name = "TB_DebugTextBox"
        Me.TB_DebugTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TB_DebugTextBox.Size = New System.Drawing.Size(284, 85)
        Me.TB_DebugTextBox.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(280, 73)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Debug Display"
        '
        'But_OpenINI
        '
        Me.But_OpenINI.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.But_OpenINI.Location = New System.Drawing.Point(605, 95)
        Me.But_OpenINI.Name = "But_OpenINI"
        Me.But_OpenINI.Size = New System.Drawing.Size(89, 35)
        Me.But_OpenINI.TabIndex = 4
        Me.But_OpenINI.Text = "Open Settings File"
        Me.But_OpenINI.UseVisualStyleBackColor = False
        '
        'But_SelectFolder
        '
        Me.But_SelectFolder.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.But_SelectFolder.Location = New System.Drawing.Point(581, 18)
        Me.But_SelectFolder.Name = "But_SelectFolder"
        Me.But_SelectFolder.Size = New System.Drawing.Size(90, 34)
        Me.But_SelectFolder.TabIndex = 6
        Me.But_SelectFolder.Text = "Select Folder"
        Me.But_SelectFolder.UseVisualStyleBackColor = False
        '
        'But_OpenEXEfolder
        '
        Me.But_OpenEXEfolder.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.But_OpenEXEfolder.Location = New System.Drawing.Point(605, 140)
        Me.But_OpenEXEfolder.Name = "But_OpenEXEfolder"
        Me.But_OpenEXEfolder.Size = New System.Drawing.Size(89, 35)
        Me.But_OpenEXEfolder.TabIndex = 4
        Me.But_OpenEXEfolder.Text = "Open EXE Folder"
        Me.But_OpenEXEfolder.UseVisualStyleBackColor = False
        '
        'TB_GameFolder
        '
        Me.TB_GameFolder.Location = New System.Drawing.Point(12, 26)
        Me.TB_GameFolder.Name = "TB_GameFolder"
        Me.TB_GameFolder.Size = New System.Drawing.Size(551, 20)
        Me.TB_GameFolder.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(92, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Diva Game Folder"
        '
        'TB_OverlayHeight
        '
        Me.TB_OverlayHeight.Location = New System.Drawing.Point(100, 317)
        Me.TB_OverlayHeight.Name = "TB_OverlayHeight"
        Me.TB_OverlayHeight.Size = New System.Drawing.Size(54, 20)
        Me.TB_OverlayHeight.TabIndex = 9
        '
        'TB_OverlayWidth
        '
        Me.TB_OverlayWidth.Location = New System.Drawing.Point(15, 317)
        Me.TB_OverlayWidth.Name = "TB_OverlayWidth"
        Me.TB_OverlayWidth.Size = New System.Drawing.Size(54, 20)
        Me.TB_OverlayWidth.TabIndex = 10
        '
        'TB_OverlayYpos
        '
        Me.TB_OverlayYpos.Location = New System.Drawing.Point(100, 269)
        Me.TB_OverlayYpos.Name = "TB_OverlayYpos"
        Me.TB_OverlayYpos.Size = New System.Drawing.Size(54, 20)
        Me.TB_OverlayYpos.TabIndex = 11
        '
        'TB_OverlayXpos
        '
        Me.TB_OverlayXpos.Location = New System.Drawing.Point(15, 269)
        Me.TB_OverlayXpos.Name = "TB_OverlayXpos"
        Me.TB_OverlayXpos.Size = New System.Drawing.Size(54, 20)
        Me.TB_OverlayXpos.TabIndex = 12
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(97, 301)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Height (px)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 301)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Width (px)"
        '
        'lblYpos
        '
        Me.lblYpos.AutoSize = True
        Me.lblYpos.Location = New System.Drawing.Point(97, 251)
        Me.lblYpos.Name = "lblYpos"
        Me.lblYpos.Size = New System.Drawing.Size(53, 13)
        Me.lblYpos.TabIndex = 15
        Me.lblYpos.Text = "y Pos (px)"
        '
        'lblXpos
        '
        Me.lblXpos.AutoSize = True
        Me.lblXpos.Location = New System.Drawing.Point(12, 251)
        Me.lblXpos.Name = "lblXpos"
        Me.lblXpos.Size = New System.Drawing.Size(53, 13)
        Me.lblXpos.TabIndex = 16
        Me.lblXpos.Text = "x Pos (px)"
        '
        'TB_OverlayDisplayTime
        '
        Me.TB_OverlayDisplayTime.Location = New System.Drawing.Point(179, 269)
        Me.TB_OverlayDisplayTime.Name = "TB_OverlayDisplayTime"
        Me.TB_OverlayDisplayTime.Size = New System.Drawing.Size(54, 20)
        Me.TB_OverlayDisplayTime.TabIndex = 9
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(176, 251)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(89, 13)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Display Time (ms)"
        '
        'But_SaveSettings
        '
        Me.But_SaveSettings.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.But_SaveSettings.Location = New System.Drawing.Point(141, 194)
        Me.But_SaveSettings.Name = "But_SaveSettings"
        Me.But_SaveSettings.Size = New System.Drawing.Size(120, 36)
        Me.But_SaveSettings.TabIndex = 17
        Me.But_SaveSettings.Text = "Save Settings"
        Me.But_SaveSettings.UseVisualStyleBackColor = False
        '
        'But_SendTest
        '
        Me.But_SendTest.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.But_SendTest.Location = New System.Drawing.Point(465, 194)
        Me.But_SendTest.Name = "But_SendTest"
        Me.But_SendTest.Size = New System.Drawing.Size(98, 36)
        Me.But_SendTest.TabIndex = 18
        Me.But_SendTest.Text = "ID Check Test"
        Me.But_SendTest.UseVisualStyleBackColor = False
        '
        'TB_FontSize
        '
        Me.TB_FontSize.Location = New System.Drawing.Point(179, 317)
        Me.TB_FontSize.Name = "TB_FontSize"
        Me.TB_FontSize.Size = New System.Drawing.Size(54, 20)
        Me.TB_FontSize.TabIndex = 19
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(176, 301)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(69, 13)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "Font Size (pt)"
        '
        'But_StartOveralyThread
        '
        Me.But_StartOveralyThread.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.But_StartOveralyThread.Location = New System.Drawing.Point(581, 317)
        Me.But_StartOveralyThread.Name = "But_StartOveralyThread"
        Me.But_StartOveralyThread.Size = New System.Drawing.Size(103, 37)
        Me.But_StartOveralyThread.TabIndex = 21
        Me.But_StartOveralyThread.Text = "Start Overlay"
        Me.But_StartOveralyThread.UseVisualStyleBackColor = False
        '
        'TB_OverlayTemplate
        '
        Me.TB_OverlayTemplate.Location = New System.Drawing.Point(12, 95)
        Me.TB_OverlayTemplate.Multiline = True
        Me.TB_OverlayTemplate.Name = "TB_OverlayTemplate"
        Me.TB_OverlayTemplate.Size = New System.Drawing.Size(261, 85)
        Me.TB_OverlayTemplate.TabIndex = 22
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(12, 73)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(67, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Overlay Text"
        '
        'But_Indicator
        '
        Me.But_Indicator.BackColor = System.Drawing.Color.White
        Me.But_Indicator.Enabled = False
        Me.But_Indicator.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.But_Indicator.ForeColor = System.Drawing.Color.Black
        Me.But_Indicator.Location = New System.Drawing.Point(537, 318)
        Me.But_Indicator.Name = "But_Indicator"
        Me.But_Indicator.Size = New System.Drawing.Size(38, 36)
        Me.But_Indicator.TabIndex = 23
        Me.But_Indicator.UseVisualStyleBackColor = False
        '
        'Lab_Status
        '
        Me.Lab_Status.AutoSize = True
        Me.Lab_Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lab_Status.Location = New System.Drawing.Point(534, 299)
        Me.Lab_Status.Name = "Lab_Status"
        Me.Lab_Status.Size = New System.Drawing.Size(53, 15)
        Me.Lab_Status.TabIndex = 2
        Me.Lab_Status.Text = "STATUS"
        '
        'TB_FontColor
        '
        Me.TB_FontColor.Location = New System.Drawing.Point(15, 370)
        Me.TB_FontColor.Name = "TB_FontColor"
        Me.TB_FontColor.Size = New System.Drawing.Size(114, 20)
        Me.TB_FontColor.TabIndex = 12
        '
        'lblFontColor
        '
        Me.lblFontColor.AutoSize = True
        Me.lblFontColor.Location = New System.Drawing.Point(12, 354)
        Me.lblFontColor.Name = "lblFontColor"
        Me.lblFontColor.Size = New System.Drawing.Size(94, 13)
        Me.lblFontColor.TabIndex = 16
        Me.lblFontColor.Text = "Font Color (RGBA)"
        '
        'lblBoxColor
        '
        Me.lblBoxColor.AutoSize = True
        Me.lblBoxColor.Location = New System.Drawing.Point(132, 354)
        Me.lblBoxColor.Name = "lblBoxColor"
        Me.lblBoxColor.Size = New System.Drawing.Size(91, 13)
        Me.lblBoxColor.TabIndex = 25
        Me.lblBoxColor.Text = "Box Color (RGBA)"
        '
        'TB_BoxColor
        '
        Me.TB_BoxColor.Location = New System.Drawing.Point(135, 370)
        Me.TB_BoxColor.Name = "TB_BoxColor"
        Me.TB_BoxColor.Size = New System.Drawing.Size(114, 20)
        Me.TB_BoxColor.TabIndex = 24
        '
        'CB_ExitOnConnectionLost
        '
        Me.CB_ExitOnConnectionLost.AutoSize = True
        Me.CB_ExitOnConnectionLost.Location = New System.Drawing.Point(585, 360)
        Me.CB_ExitOnConnectionLost.Name = "CB_ExitOnConnectionLost"
        Me.CB_ExitOnConnectionLost.Size = New System.Drawing.Size(109, 30)
        Me.CB_ExitOnConnectionLost.TabIndex = 26
        Me.CB_ExitOnConnectionLost.Text = "Close When " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "MegaMix+ Closes"
        Me.CB_ExitOnConnectionLost.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Text = "NotifyIcon1"
        Me.NotifyIcon1.Visible = True
        '
        'lblOverlayHelp
        '
        Me.lblOverlayHelp.AutoSize = True
        Me.lblOverlayHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOverlayHelp.Location = New System.Drawing.Point(258, 70)
        Me.lblOverlayHelp.Name = "lblOverlayHelp"
        Me.lblOverlayHelp.Size = New System.Drawing.Size(17, 18)
        Me.lblOverlayHelp.TabIndex = 2
        Me.lblOverlayHelp.Text = "?"
        '
        'CB_ScreenShot
        '
        Me.CB_ScreenShot.AutoSize = True
        Me.CB_ScreenShot.Location = New System.Drawing.Point(270, 371)
        Me.CB_ScreenShot.Name = "CB_ScreenShot"
        Me.CB_ScreenShot.Size = New System.Drawing.Size(110, 17)
        Me.CB_ScreenShot.TabIndex = 28
        Me.CB_ScreenShot.Text = "Ctrl+2 Screenshot"
        Me.CB_ScreenShot.UseVisualStyleBackColor = True
        '
        'formOverlayMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(704, 400)
        Me.Controls.Add(Me.CB_ScreenShot)
        Me.Controls.Add(Me.CB_ExitOnConnectionLost)
        Me.Controls.Add(Me.lblBoxColor)
        Me.Controls.Add(Me.TB_BoxColor)
        Me.Controls.Add(Me.But_Indicator)
        Me.Controls.Add(Me.TB_OverlayTemplate)
        Me.Controls.Add(Me.But_StartOveralyThread)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.TB_FontSize)
        Me.Controls.Add(Me.But_SendTest)
        Me.Controls.Add(Me.But_SaveSettings)
        Me.Controls.Add(Me.lblFontColor)
        Me.Controls.Add(Me.lblXpos)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.lblYpos)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TB_FontColor)
        Me.Controls.Add(Me.TB_OverlayXpos)
        Me.Controls.Add(Me.TB_OverlayYpos)
        Me.Controls.Add(Me.TB_OverlayWidth)
        Me.Controls.Add(Me.TB_OverlayDisplayTime)
        Me.Controls.Add(Me.TB_OverlayHeight)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TB_GameFolder)
        Me.Controls.Add(Me.But_SelectFolder)
        Me.Controls.Add(Me.But_OpenEXEfolder)
        Me.Controls.Add(Me.But_OpenINI)
        Me.Controls.Add(Me.Lab_Status)
        Me.Controls.Add(Me.lblOverlayHelp)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TB_DebugTextBox)
        Me.Controls.Add(Me.But_TestOverlay)
        Me.Name = "formOverlayMenu"
        Me.Text = "Overlay Menu"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents But_TestOverlay As Button
    Friend WithEvents TB_DebugTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents But_OpenINI As Button
    Friend WithEvents But_SelectFolder As Button
    Friend WithEvents But_OpenEXEfolder As Button
    Friend WithEvents TB_GameFolder As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TB_OverlayHeight As TextBox
    Friend WithEvents TB_OverlayWidth As TextBox
    Friend WithEvents TB_OverlayYpos As TextBox
    Friend WithEvents TB_OverlayXpos As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lblYpos As Label
    Friend WithEvents lblXpos As Label
    Friend WithEvents TB_OverlayDisplayTime As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents But_SaveSettings As Button
    Friend WithEvents But_SendTest As Button
    Friend WithEvents TB_FontSize As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents But_StartOveralyThread As Button
    Friend WithEvents TB_OverlayTemplate As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents But_Indicator As Button
    Friend WithEvents Lab_Status As Label
    Friend WithEvents TB_FontColor As TextBox
    Friend WithEvents lblFontColor As Label
    Friend WithEvents lblBoxColor As Label
    Friend WithEvents TB_BoxColor As TextBox
    Friend WithEvents CB_ExitOnConnectionLost As CheckBox
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents lblOverlayHelp As Label
    Friend WithEvents CB_ScreenShot As CheckBox
End Class
