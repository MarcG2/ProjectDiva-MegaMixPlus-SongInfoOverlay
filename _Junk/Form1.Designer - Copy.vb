<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.But_TestOverlay = New System.Windows.Forms.Button()
        Me.TB_DebugTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.But_Test2 = New System.Windows.Forms.Button()
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
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TB_OverlayDisplayTime = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.But_SaveSettings = New System.Windows.Forms.Button()
        Me.But_SendTest = New System.Windows.Forms.Button()
        Me.TB_FontSize = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.But_StartOveralyThread = New System.Windows.Forms.Button()
        Me.TB_ProcessName = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TB_OverlayTest = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'But_TestOverlay
        '
        Me.But_TestOverlay.Location = New System.Drawing.Point(12, 367)
        Me.But_TestOverlay.Name = "But_TestOverlay"
        Me.But_TestOverlay.Size = New System.Drawing.Size(132, 36)
        Me.But_TestOverlay.TabIndex = 0
        Me.But_TestOverlay.Text = "Test Overlay"
        Me.But_TestOverlay.UseVisualStyleBackColor = True
        '
        'TB_DebugTextBox
        '
        Me.TB_DebugTextBox.Location = New System.Drawing.Point(15, 130)
        Me.TB_DebugTextBox.Multiline = True
        Me.TB_DebugTextBox.Name = "TB_DebugTextBox"
        Me.TB_DebugTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TB_DebugTextBox.Size = New System.Drawing.Size(305, 79)
        Me.TB_DebugTextBox.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 108)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Debug Display"
        '
        'But_Test2
        '
        Me.But_Test2.Location = New System.Drawing.Point(507, 332)
        Me.But_Test2.Name = "But_Test2"
        Me.But_Test2.Size = New System.Drawing.Size(132, 36)
        Me.But_Test2.TabIndex = 0
        Me.But_Test2.Text = "Test 2"
        Me.But_Test2.UseVisualStyleBackColor = True
        '
        'But_OpenINI
        '
        Me.But_OpenINI.Location = New System.Drawing.Point(632, 221)
        Me.But_OpenINI.Name = "But_OpenINI"
        Me.But_OpenINI.Size = New System.Drawing.Size(98, 40)
        Me.But_OpenINI.TabIndex = 4
        Me.But_OpenINI.Text = "Open Settings"
        Me.But_OpenINI.UseVisualStyleBackColor = True
        '
        'But_SelectFolder
        '
        Me.But_SelectFolder.Location = New System.Drawing.Point(529, 24)
        Me.But_SelectFolder.Name = "But_SelectFolder"
        Me.But_SelectFolder.Size = New System.Drawing.Size(90, 23)
        Me.But_SelectFolder.TabIndex = 6
        Me.But_SelectFolder.Text = "Select Folder"
        Me.But_SelectFolder.UseVisualStyleBackColor = True
        '
        'But_OpenEXEfolder
        '
        Me.But_OpenEXEfolder.Location = New System.Drawing.Point(632, 267)
        Me.But_OpenEXEfolder.Name = "But_OpenEXEfolder"
        Me.But_OpenEXEfolder.Size = New System.Drawing.Size(98, 40)
        Me.But_OpenEXEfolder.TabIndex = 4
        Me.But_OpenEXEfolder.Text = "Open Folder"
        Me.But_OpenEXEfolder.UseVisualStyleBackColor = True
        '
        'TB_GameFolder
        '
        Me.TB_GameFolder.Location = New System.Drawing.Point(12, 26)
        Me.TB_GameFolder.Name = "TB_GameFolder"
        Me.TB_GameFolder.Size = New System.Drawing.Size(495, 20)
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
        Me.TB_OverlayHeight.Location = New System.Drawing.Point(565, 156)
        Me.TB_OverlayHeight.Name = "TB_OverlayHeight"
        Me.TB_OverlayHeight.Size = New System.Drawing.Size(54, 20)
        Me.TB_OverlayHeight.TabIndex = 9
        '
        'TB_OverlayWidth
        '
        Me.TB_OverlayWidth.Location = New System.Drawing.Point(480, 156)
        Me.TB_OverlayWidth.Name = "TB_OverlayWidth"
        Me.TB_OverlayWidth.Size = New System.Drawing.Size(54, 20)
        Me.TB_OverlayWidth.TabIndex = 10
        '
        'TB_OverlayYpos
        '
        Me.TB_OverlayYpos.Location = New System.Drawing.Point(565, 108)
        Me.TB_OverlayYpos.Name = "TB_OverlayYpos"
        Me.TB_OverlayYpos.Size = New System.Drawing.Size(54, 20)
        Me.TB_OverlayYpos.TabIndex = 11
        '
        'TB_OverlayXpos
        '
        Me.TB_OverlayXpos.Location = New System.Drawing.Point(480, 108)
        Me.TB_OverlayXpos.Name = "TB_OverlayXpos"
        Me.TB_OverlayXpos.Size = New System.Drawing.Size(54, 20)
        Me.TB_OverlayXpos.TabIndex = 12
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(562, 140)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Height (px)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(477, 140)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Width (px)"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(562, 92)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 13)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "y Pos (px)"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(477, 92)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(53, 13)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "x Pos (px)"
        '
        'TB_OverlayDisplayTime
        '
        Me.TB_OverlayDisplayTime.Location = New System.Drawing.Point(644, 108)
        Me.TB_OverlayDisplayTime.Name = "TB_OverlayDisplayTime"
        Me.TB_OverlayDisplayTime.Size = New System.Drawing.Size(54, 20)
        Me.TB_OverlayDisplayTime.TabIndex = 9
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(641, 92)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(89, 13)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "Display Time (ms)"
        '
        'But_SaveSettings
        '
        Me.But_SaveSettings.Location = New System.Drawing.Point(687, 15)
        Me.But_SaveSettings.Name = "But_SaveSettings"
        Me.But_SaveSettings.Size = New System.Drawing.Size(103, 40)
        Me.But_SaveSettings.TabIndex = 17
        Me.But_SaveSettings.Text = "Save Settings"
        Me.But_SaveSettings.UseVisualStyleBackColor = True
        '
        'But_SendTest
        '
        Me.But_SendTest.Location = New System.Drawing.Point(507, 374)
        Me.But_SendTest.Name = "But_SendTest"
        Me.But_SendTest.Size = New System.Drawing.Size(132, 30)
        Me.But_SendTest.TabIndex = 18
        Me.But_SendTest.Text = "Send Test"
        Me.But_SendTest.UseVisualStyleBackColor = True
        '
        'TB_FontSize
        '
        Me.TB_FontSize.Location = New System.Drawing.Point(644, 156)
        Me.TB_FontSize.Name = "TB_FontSize"
        Me.TB_FontSize.Size = New System.Drawing.Size(54, 20)
        Me.TB_FontSize.TabIndex = 19
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(641, 140)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(69, 13)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "Font Size (pt)"
        '
        'But_StartOveralyThread
        '
        Me.But_StartOveralyThread.Location = New System.Drawing.Point(354, 130)
        Me.But_StartOveralyThread.Name = "But_StartOveralyThread"
        Me.But_StartOveralyThread.Size = New System.Drawing.Size(90, 37)
        Me.But_StartOveralyThread.TabIndex = 21
        Me.But_StartOveralyThread.Text = "Start Overlay"
        Me.But_StartOveralyThread.UseVisualStyleBackColor = True
        '
        'TB_ProcessName
        '
        Me.TB_ProcessName.Location = New System.Drawing.Point(116, 52)
        Me.TB_ProcessName.Name = "TB_ProcessName"
        Me.TB_ProcessName.Size = New System.Drawing.Size(391, 20)
        Me.TB_ProcessName.TabIndex = 7
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(21, 55)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(76, 13)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Process Name"
        '
        'TB_OverlayTest
        '
        Me.TB_OverlayTest.Location = New System.Drawing.Point(12, 270)
        Me.TB_OverlayTest.Multiline = True
        Me.TB_OverlayTest.Name = "TB_OverlayTest"
        Me.TB_OverlayTest.Size = New System.Drawing.Size(305, 85)
        Me.TB_OverlayTest.TabIndex = 22
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(12, 248)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(67, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Overlay Test"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(795, 415)
        Me.Controls.Add(Me.TB_OverlayTest)
        Me.Controls.Add(Me.But_StartOveralyThread)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.TB_FontSize)
        Me.Controls.Add(Me.But_SendTest)
        Me.Controls.Add(Me.But_SaveSettings)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TB_OverlayXpos)
        Me.Controls.Add(Me.TB_OverlayYpos)
        Me.Controls.Add(Me.TB_OverlayWidth)
        Me.Controls.Add(Me.TB_OverlayDisplayTime)
        Me.Controls.Add(Me.TB_OverlayHeight)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TB_ProcessName)
        Me.Controls.Add(Me.TB_GameFolder)
        Me.Controls.Add(Me.But_SelectFolder)
        Me.Controls.Add(Me.But_OpenEXEfolder)
        Me.Controls.Add(Me.But_OpenINI)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TB_DebugTextBox)
        Me.Controls.Add(Me.But_Test2)
        Me.Controls.Add(Me.But_TestOverlay)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents But_TestOverlay As Button
    Friend WithEvents TB_DebugTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents But_Test2 As Button
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
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents TB_OverlayDisplayTime As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents But_SaveSettings As Button
    Friend WithEvents But_SendTest As Button
    Friend WithEvents TB_FontSize As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents But_StartOveralyThread As Button
    Friend WithEvents TB_ProcessName As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents TB_OverlayTest As TextBox
    Friend WithEvents Label10 As Label
End Class
