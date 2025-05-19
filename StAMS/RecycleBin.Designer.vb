<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RecycleBin
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
        Me.dgvDeletedStudents = New System.Windows.Forms.DataGridView()
        Me.btnRestore = New System.Windows.Forms.Button()
        Me.btnLoadDeleted = New System.Windows.Forms.Button()
        Me.btnSelectAll = New System.Windows.Forms.Button()
        Me.btnDeselectAll = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnBack = New System.Windows.Forms.Button()
        CType(Me.dgvDeletedStudents, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvDeletedStudents
        '
        Me.dgvDeletedStudents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvDeletedStudents.BackgroundColor = System.Drawing.Color.White
        Me.dgvDeletedStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDeletedStudents.Location = New System.Drawing.Point(12, 222)
        Me.dgvDeletedStudents.Name = "dgvDeletedStudents"
        Me.dgvDeletedStudents.Size = New System.Drawing.Size(620, 257)
        Me.dgvDeletedStudents.TabIndex = 0
        '
        'btnRestore
        '
        Me.btnRestore.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRestore.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRestore.ForeColor = System.Drawing.Color.White
        Me.btnRestore.Location = New System.Drawing.Point(319, 495)
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.Size = New System.Drawing.Size(105, 41)
        Me.btnRestore.TabIndex = 1
        Me.btnRestore.Text = "Restore"
        Me.btnRestore.UseVisualStyleBackColor = False
        '
        'btnLoadDeleted
        '
        Me.btnLoadDeleted.BackColor = System.Drawing.Color.FromArgb(CType(CType(23, Byte), Integer), CType(CType(162, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.btnLoadDeleted.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoadDeleted.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadDeleted.ForeColor = System.Drawing.Color.White
        Me.btnLoadDeleted.Location = New System.Drawing.Point(504, 182)
        Me.btnLoadDeleted.Name = "btnLoadDeleted"
        Me.btnLoadDeleted.Size = New System.Drawing.Size(128, 34)
        Me.btnLoadDeleted.TabIndex = 2
        Me.btnLoadDeleted.Text = "Refresh"
        Me.btnLoadDeleted.UseVisualStyleBackColor = False
        '
        'btnSelectAll
        '
        Me.btnSelectAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(123, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelectAll.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectAll.ForeColor = System.Drawing.Color.White
        Me.btnSelectAll.Location = New System.Drawing.Point(14, 182)
        Me.btnSelectAll.Name = "btnSelectAll"
        Me.btnSelectAll.Size = New System.Drawing.Size(89, 34)
        Me.btnSelectAll.TabIndex = 3
        Me.btnSelectAll.Text = "Select All"
        Me.btnSelectAll.UseVisualStyleBackColor = False
        '
        'btnDeselectAll
        '
        Me.btnDeselectAll.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnDeselectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDeselectAll.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeselectAll.ForeColor = System.Drawing.Color.White
        Me.btnDeselectAll.Location = New System.Drawing.Point(109, 182)
        Me.btnDeselectAll.Name = "btnDeselectAll"
        Me.btnDeselectAll.Size = New System.Drawing.Size(101, 34)
        Me.btnDeselectAll.TabIndex = 4
        Me.btnDeselectAll.Text = "Deselect All"
        Me.btnDeselectAll.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ForeColor = System.Drawing.Color.White
        Me.btnDelete.Location = New System.Drawing.Point(499, 495)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(116, 41)
        Me.btnDelete.TabIndex = 5
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Location = New System.Drawing.Point(-7, -2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(678, 133)
        Me.Panel1.TabIndex = 31
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label8.Font = New System.Drawing.Font("Calibri", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(175, 77)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(349, 39)
        Me.Label8.TabIndex = 31
        Me.Label8.Text = "NATIONAL HIGH SCHOOL"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.PictureBox1.BackgroundImage = Global.StAMS.My.Resources.Resources.ca1aaecc_6baa_4153_b083_b5525ada6bd1_removebg_preview
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(21, 14)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(116, 111)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 30
        Me.PictureBox1.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label6.Font = New System.Drawing.Font("Calibri", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(206, 18)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(280, 59)
        Me.Label6.TabIndex = 28
        Me.Label6.Text = "VALENZUELA"
        '
        'btnBack
        '
        Me.btnBack.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBack.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBack.ForeColor = System.Drawing.Color.White
        Me.btnBack.Location = New System.Drawing.Point(12, 513)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(75, 23)
        Me.btnBack.TabIndex = 32
        Me.btnBack.Text = "Back"
        Me.btnBack.UseVisualStyleBackColor = False
        '
        'RecycleBin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(44, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(662, 548)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnDeselectAll)
        Me.Controls.Add(Me.btnSelectAll)
        Me.Controls.Add(Me.btnLoadDeleted)
        Me.Controls.Add(Me.btnRestore)
        Me.Controls.Add(Me.dgvDeletedStudents)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "RecycleBin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "RecycleBin"
        CType(Me.dgvDeletedStudents, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvDeletedStudents As System.Windows.Forms.DataGridView
    Friend WithEvents btnRestore As System.Windows.Forms.Button
    Friend WithEvents btnLoadDeleted As System.Windows.Forms.Button
    Friend WithEvents btnSelectAll As System.Windows.Forms.Button
    Friend WithEvents btnDeselectAll As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnBack As System.Windows.Forms.Button
End Class
