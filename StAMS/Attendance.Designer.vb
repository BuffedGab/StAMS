<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Attendance
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Attendance))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.pbCapture = New System.Windows.Forms.PictureBox()
        Me.pbImage = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblNumber = New System.Windows.Forms.Label()
        Me.lblSection = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblYrLevel = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblFullName = New System.Windows.Forms.Label()
        Me.lblLRN = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtRFID = New System.Windows.Forms.TextBox()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.lblStudentID = New System.Windows.Forms.Label()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.lblAcadYear = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.TimerClear = New System.Windows.Forms.Timer(Me.components)
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.pbCapture, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1087, 663)
        Me.Panel1.TabIndex = 0
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Panel3.Controls.Add(Me.Panel4)
        Me.Panel3.Controls.Add(Me.Label6)
        Me.Panel3.Controls.Add(Me.Label8)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Controls.Add(Me.Label10)
        Me.Panel3.Controls.Add(Me.Label11)
        Me.Panel3.Controls.Add(Me.lblNumber)
        Me.Panel3.Controls.Add(Me.lblSection)
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Controls.Add(Me.lblYrLevel)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.lblFullName)
        Me.Panel3.Controls.Add(Me.lblLRN)
        Me.Panel3.Controls.Add(Me.Label7)
        Me.Panel3.Controls.Add(Me.txtRFID)
        Me.Panel3.Controls.Add(Me.lblDate)
        Me.Panel3.Controls.Add(Me.lblStudentID)
        Me.Panel3.Controls.Add(Me.lblTime)
        Me.Panel3.Controls.Add(Me.lblAcadYear)
        Me.Panel3.Controls.Add(Me.Panel2)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1087, 663)
        Me.Panel3.TabIndex = 1
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Panel4.Controls.Add(Me.btnLogout)
        Me.Panel4.Controls.Add(Me.pbCapture)
        Me.Panel4.Controls.Add(Me.pbImage)
        Me.Panel4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Panel4.Location = New System.Drawing.Point(0, 195)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(377, 468)
        Me.Panel4.TabIndex = 25
        '
        'btnLogout
        '
        Me.btnLogout.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLogout.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogout.ForeColor = System.Drawing.Color.White
        Me.btnLogout.Location = New System.Drawing.Point(12, 431)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(87, 34)
        Me.btnLogout.TabIndex = 24
        Me.btnLogout.Text = "Logout"
        Me.btnLogout.UseVisualStyleBackColor = False
        '
        'pbCapture
        '
        Me.pbCapture.BackColor = System.Drawing.Color.White
        Me.pbCapture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pbCapture.Location = New System.Drawing.Point(38, 81)
        Me.pbCapture.Name = "pbCapture"
        Me.pbCapture.Size = New System.Drawing.Size(294, 263)
        Me.pbCapture.TabIndex = 23
        Me.pbCapture.TabStop = False
        '
        'pbImage
        '
        Me.pbImage.BackColor = System.Drawing.Color.Green
        Me.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbImage.Location = New System.Drawing.Point(50, 94)
        Me.pbImage.Name = "pbImage"
        Me.pbImage.Size = New System.Drawing.Size(267, 238)
        Me.pbImage.TabIndex = 9
        Me.pbImage.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Calibri", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(418, 511)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(243, 39)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "Contact Number:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Calibri", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(418, 463)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(123, 39)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "Section:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Calibri", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(418, 415)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(181, 39)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Grade Level:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Calibri", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(418, 365)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(160, 39)
        Me.Label10.TabIndex = 19
        Me.Label10.Text = "Full Name:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Calibri", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(418, 313)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(79, 39)
        Me.Label11.TabIndex = 18
        Me.Label11.Text = "LRN:"
        '
        'lblNumber
        '
        Me.lblNumber.AutoSize = True
        Me.lblNumber.BackColor = System.Drawing.Color.Transparent
        Me.lblNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumber.ForeColor = System.Drawing.Color.Black
        Me.lblNumber.Location = New System.Drawing.Point(862, 317)
        Me.lblNumber.Name = "lblNumber"
        Me.lblNumber.Size = New System.Drawing.Size(0, 31)
        Me.lblNumber.TabIndex = 14
        '
        'lblSection
        '
        Me.lblSection.AutoSize = True
        Me.lblSection.BackColor = System.Drawing.Color.Transparent
        Me.lblSection.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSection.ForeColor = System.Drawing.Color.Black
        Me.lblSection.Location = New System.Drawing.Point(862, 510)
        Me.lblSection.Name = "lblSection"
        Me.lblSection.Size = New System.Drawing.Size(0, 31)
        Me.lblSection.TabIndex = 13
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Calibri", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(779, 206)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(108, 45)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "TIME:"
        '
        'lblYrLevel
        '
        Me.lblYrLevel.AutoSize = True
        Me.lblYrLevel.BackColor = System.Drawing.Color.Transparent
        Me.lblYrLevel.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYrLevel.ForeColor = System.Drawing.Color.Black
        Me.lblYrLevel.Location = New System.Drawing.Point(862, 461)
        Me.lblYrLevel.Name = "lblYrLevel"
        Me.lblYrLevel.Size = New System.Drawing.Size(0, 31)
        Me.lblYrLevel.TabIndex = 12
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Calibri", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(407, 206)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(107, 45)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "DATE:"
        '
        'lblFullName
        '
        Me.lblFullName.AutoSize = True
        Me.lblFullName.BackColor = System.Drawing.Color.Transparent
        Me.lblFullName.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFullName.ForeColor = System.Drawing.Color.Black
        Me.lblFullName.Location = New System.Drawing.Point(862, 409)
        Me.lblFullName.Name = "lblFullName"
        Me.lblFullName.Size = New System.Drawing.Size(0, 31)
        Me.lblFullName.TabIndex = 11
        '
        'lblLRN
        '
        Me.lblLRN.AutoSize = True
        Me.lblLRN.BackColor = System.Drawing.Color.Transparent
        Me.lblLRN.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLRN.ForeColor = System.Drawing.Color.Black
        Me.lblLRN.Location = New System.Drawing.Point(862, 359)
        Me.lblLRN.Name = "lblLRN"
        Me.lblLRN.Size = New System.Drawing.Size(0, 31)
        Me.lblLRN.TabIndex = 10
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Calibri", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(385, 622)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(376, 36)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Enter or Tap RFID card Reader"
        '
        'txtRFID
        '
        Me.txtRFID.BackColor = System.Drawing.Color.White
        Me.txtRFID.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRFID.ForeColor = System.Drawing.Color.Black
        Me.txtRFID.Location = New System.Drawing.Point(767, 624)
        Me.txtRFID.Multiline = True
        Me.txtRFID.Name = "txtRFID"
        Me.txtRFID.Size = New System.Drawing.Size(308, 34)
        Me.txtRFID.TabIndex = 7
        Me.txtRFID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.BackColor = System.Drawing.Color.Transparent
        Me.lblDate.Font = New System.Drawing.Font("Calibri", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDate.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.lblDate.Location = New System.Drawing.Point(511, 206)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(93, 45)
        Me.lblDate.TabIndex = 5
        Me.lblDate.Text = "Date"
        '
        'lblStudentID
        '
        Me.lblStudentID.AutoSize = True
        Me.lblStudentID.Location = New System.Drawing.Point(502, 365)
        Me.lblStudentID.Name = "lblStudentID"
        Me.lblStudentID.Size = New System.Drawing.Size(39, 13)
        Me.lblStudentID.TabIndex = 15
        Me.lblStudentID.Text = "Label4"
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.BackColor = System.Drawing.Color.Transparent
        Me.lblTime.Font = New System.Drawing.Font("Calibri", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTime.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.lblTime.Location = New System.Drawing.Point(884, 206)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(96, 45)
        Me.lblTime.TabIndex = 4
        Me.lblTime.Text = "Time"
        '
        'lblAcadYear
        '
        Me.lblAcadYear.AutoSize = True
        Me.lblAcadYear.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.lblAcadYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAcadYear.ForeColor = System.Drawing.Color.White
        Me.lblAcadYear.Location = New System.Drawing.Point(749, 154)
        Me.lblAcadYear.Name = "lblAcadYear"
        Me.lblAcadYear.Size = New System.Drawing.Size(0, 25)
        Me.lblAcadYear.TabIndex = 3
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Panel2.Controls.Add(Me.Label14)
        Me.Panel2.Controls.Add(Me.PictureBox1)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1168, 196)
        Me.Panel2.TabIndex = 24
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(-10, 188)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(398, 14)
        Me.Label14.TabIndex = 35
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(50, 19)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(177, 160)
        Me.PictureBox1.TabIndex = 6
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Calibri", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(349, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(670, 59)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Valenzuela National High Shcool"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Calibri", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(259, 88)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(825, 39)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "4 A. Fernando Street, Marulas, Valenzuela City, Metro Manila" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Calibri", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(470, 139)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(260, 45)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Academic Year :"
        '
        'Timer1
        '
        '
        'Timer2
        '
        '
        'Timer3
        '
        '
        'TimerClear
        '
        Me.TimerClear.Interval = 3000
        '
        'Attendance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1087, 663)
        Me.Controls.Add(Me.Panel1)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Attendance"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Panel1.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        CType(Me.pbCapture, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblAcadYear As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblTime As System.Windows.Forms.Label
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtRFID As System.Windows.Forms.TextBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblNumber As System.Windows.Forms.Label
    Friend WithEvents lblSection As System.Windows.Forms.Label
    Friend WithEvents lblYrLevel As System.Windows.Forms.Label
    Friend WithEvents lblFullName As System.Windows.Forms.Label
    Friend WithEvents lblLRN As System.Windows.Forms.Label
    Friend WithEvents pbImage As System.Windows.Forms.PictureBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents Timer3 As System.Windows.Forms.Timer
    Friend WithEvents lblStudentID As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents pbCapture As System.Windows.Forms.PictureBox
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnLogout As System.Windows.Forms.Button
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TimerClear As System.Windows.Forms.Timer
End Class
