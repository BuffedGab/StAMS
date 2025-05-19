Imports System.IO
Imports MySql.Data.MySqlClient

Public Class ManageStudent
    Public Sub LoadStudentData(ByVal studentID As String, ByVal lrn As String, ByVal fullName As String, ByVal yearLevel As String, ByVal section As String, ByVal contact As String, ByVal rfid As String)
        lblStudentID.Text = studentID
        txtLRN.Text = lrn
        txtFullName.Text = fullName

        ' This safely sets the Year Level
        If cbYrLevel.Items.Contains(yearLevel) Then
            cbYrLevel.SelectedItem = yearLevel
        Else
            cbYrLevel.SelectedIndex = -1 ' clear selection if not found
        End If

        ' Do the same for Section
        If cbSection.Items.Contains(section) Then
            cbSection.SelectedItem = section
        Else
            cbSection.SelectedIndex = -1
        End If

        txtNumber.Text = contact
        txtRFID.Text = rfid

        btnAdd.Enabled = False
        btnUpdate.Enabled = True
        btnDelete.Enabled = True
    End Sub

    Private openFileDialog As New OpenFileDialog()

    ' Load Data on Form Load
    Private Sub ManageStudent_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        LoadData()
        LoadYearLevels()
    End Sub

    ' Load Year Levels into ComboBox
    Private Sub LoadYearLevels()
        Try
            OpenConnection()
            Dim query As String = "SELECT DISTINCT YearLevel FROM tblAdvisory ORDER BY YearLevel ASC"
            cmd = New MySqlCommand(query, conn)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            cbYrLevel.Items.Clear()
            While reader.Read()
                cbYrLevel.Items.Add(reader("YearLevel").ToString())
            End While
            reader.Close()
        Catch ex As Exception
            MsgBox("Error loading Year Levels: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    ' Add Student Record
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Try
            ' Validate fields before adding
            If String.IsNullOrWhiteSpace(txtLRN.Text) OrElse
               String.IsNullOrWhiteSpace(txtFullName.Text) OrElse
               cbYrLevel.SelectedIndex = -1 OrElse
               cbSection.SelectedIndex = -1 OrElse
               String.IsNullOrWhiteSpace(txtNumber.Text) OrElse
               String.IsNullOrWhiteSpace(txtRFID.Text) Then

                MsgBox("Please fill in all fields before adding (Image is optional).", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            OpenConnection()
            ' Check if LRN already exists
            Dim checkLRNQuery As String = "SELECT COUNT(*) FROM tblStudent WHERE LRN = @LRN"
            Dim checkLRNCmd As New MySqlCommand(checkLRNQuery, conn)
            checkLRNCmd.Parameters.AddWithValue("@LRN", txtLRN.Text)

            If Convert.ToInt32(checkLRNCmd.ExecuteScalar()) > 0 Then
                MsgBox("A student with this LRN already exists.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            ' Check if RFID already exists
            Dim checkRFIDQuery As String = "SELECT COUNT(*) FROM tblStudent WHERE RFID = @RFID"
            Dim checkRFIDCmd As New MySqlCommand(checkRFIDQuery, conn)
            checkRFIDCmd.Parameters.AddWithValue("@RFID", txtRFID.Text)

            If Convert.ToInt32(checkRFIDCmd.ExecuteScalar()) > 0 Then
                MsgBox("A student with this RFID already exists.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            ' Check if FullName already exists
            Dim checkNameQuery As String = "SELECT COUNT(*) FROM tblStudent WHERE FullName = @FullName"
            Dim checkNameCmd As New MySqlCommand(checkNameQuery, conn)
            checkNameCmd.Parameters.AddWithValue("@FullName", txtFullName.Text)

            If Convert.ToInt32(checkNameCmd.ExecuteScalar()) > 0 Then
                MsgBox("A student with this Full Name already exists.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            Dim query As String = "INSERT INTO tblStudent (LRN, FullName, YearLevel, Section, Contact, RFID, Image) VALUES (@LRN, @FullName, @YearLevel, @Section, @Contact, @RFID, @Image)"
            cmd = New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@LRN", txtLRN.Text)
            cmd.Parameters.AddWithValue("@FullName", txtFullName.Text)
            cmd.Parameters.AddWithValue("@YearLevel", cbYrLevel.Text)
            cmd.Parameters.AddWithValue("@Section", cbSection.Text)
            cmd.Parameters.AddWithValue("@Contact", txtNumber.Text)
            cmd.Parameters.AddWithValue("@RFID", txtRFID.Text)

            ' Convert Image to Byte Array and handle NULL
            If PictureBox1.Image IsNot Nothing Then
                Dim imageBytes As Byte() = ImageToByteArray(PictureBox1.Image)
                If imageBytes IsNot Nothing Then
                    cmd.Parameters.Add("@Image", MySqlDbType.Blob).Value = imageBytes
                Else
                    cmd.Parameters.Add("@Image", MySqlDbType.Blob).Value = DBNull.Value
                End If
            Else
                cmd.Parameters.Add("@Image", MySqlDbType.Blob).Value = DBNull.Value
            End If


            cmd.ExecuteNonQuery()
            MsgBox("Record Added Successfully!", MsgBoxStyle.Information)
            LoadData()
            Dashboard.UpdateStudentCount()
            BtnClear_Click(Nothing, Nothing)
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    ' Update Student Record
    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Dim studentID As Integer
        If Not Integer.TryParse(lblStudentID.Text, studentID) Then
            MsgBox("Please select a student from the list first.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Try
            OpenConnection()
            Dim query As String = "UPDATE tblStudent SET LRN=@LRN, FullName=@FullName, YearLevel=@YearLevel, Section=@Section, Contact=@Contact, RFID=@RFID, Image=@Image WHERE StudentID=@StudentID"
            cmd = New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@StudentID", lblStudentID.Text)
            cmd.Parameters.AddWithValue("@LRN", txtLRN.Text)
            cmd.Parameters.AddWithValue("@FullName", txtFullName.Text)
            cmd.Parameters.AddWithValue("@YearLevel", cbYrLevel.Text)
            cmd.Parameters.AddWithValue("@Section", cbSection.Text)
            cmd.Parameters.AddWithValue("@Contact", txtNumber.Text)
            cmd.Parameters.AddWithValue("@RFID", txtRFID.Text)

            ' Convert Image to Byte Array and handle NULL
            If PictureBox1.Image IsNot Nothing Then
                Dim imageBytes As Byte() = ImageToByteArray(PictureBox1.Image)
                If imageBytes IsNot Nothing Then
                    cmd.Parameters.Add("@Image", MySqlDbType.Blob).Value = imageBytes
                Else
                    cmd.Parameters.Add("@Image", MySqlDbType.Blob).Value = DBNull.Value
                End If
            Else
                cmd.Parameters.Add("@Image", MySqlDbType.Blob).Value = DBNull.Value
            End If


            cmd.ExecuteNonQuery()
            MsgBox("Record Updated Successfully!", MsgBoxStyle.Information)
            LoadData()
            BtnClear_Click(Nothing, Nothing)
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    ' Delete Student Record
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Dim studentID As Integer
        If Not Integer.TryParse(lblStudentID.Text, studentID) Then
            MsgBox("Please select a student from the list first.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then

            Try
                OpenConnection()
                ' Changed from DELETE to soft delete using IsDeleted
                Dim query As String = "UPDATE tblStudent SET IsDeleted = 1 WHERE StudentID=@StudentID"
                cmd = New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@StudentID", lblStudentID.Text)

                cmd.ExecuteNonQuery()
                MsgBox("Record Moved to Recycle Bin Successfully!", MessageBoxIcon.Information)
                LoadData()
                Dashboard.UpdateStudentCount()
                BtnClear_Click(Nothing, Nothing)
            Catch ex As Exception
                MsgBox("Error: " & ex.Message)
            Finally
                CloseConnection()
            End Try
        End If
    End Sub


    Private Sub LoadData()
        Try
            OpenConnection()
            Dim query As String = "SELECT * FROM tblStudent WHERE IsDeleted = 0"  ' Only show records that are not deleted
            adapter = New MySqlDataAdapter(query, conn)
            dt = New DataTable()
            adapter.Fill(dt)
            DataGridView1.DataSource = dt

            ' Make the DataGridView non-editable but selectable
            DataGridView1.ReadOnly = True
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DataGridView1.AllowUserToAddRows = False

            ' Hide the Image column
            If DataGridView1.Columns.Contains("Image") Then
                DataGridView1.Columns("Image").Visible = False
            End If

            ' Hide the StudentID column
            If DataGridView1.Columns.Contains("StudentID") Then
                DataGridView1.Columns("StudentID").Visible = False
            End If

            ' Hide the IsDeleted column
            If DataGridView1.Columns.Contains("IsDeleted") Then
                DataGridView1.Columns("IsDeleted").Visible = False
            End If

        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub




    ' Browse and Load Image
    Private Sub btnBrowse_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowse.Click
        openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
        openFileDialog.Title = "Select an Image"

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Try
                ' Load image into memory to prevent file lock issues
                Dim tempImage As Image
                Using fs As New FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read)
                    tempImage = Image.FromStream(fs)
                End Using

                ' Assign a fresh copy
                PictureBox1.Image = New Bitmap(tempImage)
                PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
            Catch ex As Exception
                MsgBox("Error loading image: " & ex.Message)
            End Try
        End If
    End Sub

    ' Convert Image to Byte Array
    Private Function ImageToByteArray(ByVal img As Image) As Byte()
        Try
            Using ms As New MemoryStream()
                ' Save the image to memory as PNG
                img.Save(ms, Imaging.ImageFormat.Png)
                Return ms.ToArray() ' Return the byte array
            End Using
        Catch ex As Exception
            MsgBox("Error converting image: " & ex.Message)
            Return Nothing
        End Try
    End Function

    ' Convert Byte Array to Image
    Private Function ByteArrayToImage(ByVal data As Byte()) As Image
        Try
            Using ms As New MemoryStream(data)
                Return New Bitmap(Image.FromStream(ms))
            End Using
        Catch ex As Exception
            MsgBox("Error loading image: " & ex.Message)
            Return Nothing
        End Try
    End Function

    ' Clear Fields
    Private Sub BtnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        txtLRN.Clear()
        txtFullName.Clear()
        cbYrLevel.SelectedIndex = -1
        cbSection.SelectedIndex = -1
        txtNumber.Clear()
        txtRFID.Clear()
        UpdateButtonStates()

        If PictureBox1.Image IsNot Nothing Then
            PictureBox1.Image.Dispose()
        End If
        PictureBox1.Image = Nothing
    End Sub

    Private Sub LoadSections(ByVal yearLevel As String)
        Try
            OpenConnection()
            Dim query As String = "SELECT DISTINCT Section FROM tblAdvisory WHERE YearLevel = @YearLevel ORDER BY Section ASC"
            cmd = New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@YearLevel", yearLevel)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            cbSection.Items.Clear()
            While reader.Read()
                cbSection.Items.Add(reader("Section").ToString())
            End While
            reader.Close()
        Catch ex As Exception
            MsgBox("Error loading Sections: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub cbYrLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbYrLevel.SelectedIndexChanged
        If cbYrLevel.SelectedIndex <> -1 Then
            Dim selectedYearLevel As String = cbYrLevel.SelectedItem.ToString()
            LoadSections(selectedYearLevel)
        Else
            cbSection.Items.Clear()
        End If
    End Sub


    ' Handle Cell Click Event to Edit Data
    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            lblStudentID.Text = row.Cells("StudentID").Value.ToString()
            txtLRN.Text = row.Cells("LRN").Value.ToString()
            txtFullName.Text = row.Cells("FullName").Value.ToString()

            ' Safely select YearLevel from ComboBox
            Dim yearLevelValue As String = row.Cells("YearLevel").Value.ToString().Trim()
            Dim foundIndex As Integer = -1

            For i As Integer = 0 To cbYrLevel.Items.Count - 1
                If cbYrLevel.Items(i).ToString().Trim().ToLower() = yearLevelValue.ToLower() Then
                    foundIndex = i
                    Exit For
                End If
            Next

            If foundIndex <> -1 Then
                cbYrLevel.SelectedIndex = foundIndex
            Else
                cbYrLevel.SelectedIndex = -1 ' fallback
            End If

            ' Safely select Section from ComboBox
            Dim sectionValue As String = row.Cells("Section").Value.ToString().Trim()
            If cbSection.Items.Contains(sectionValue) Then
                cbSection.SelectedItem = sectionValue
            Else
                cbSection.SelectedIndex = -1 ' fallback
            End If

            txtNumber.Text = row.Cells("Contact").Value.ToString()
            txtRFID.Text = row.Cells("RFID").Value.ToString()

            ' Load Image from Database
            If Not IsDBNull(row.Cells("Image").Value) Then
                PictureBox1.Image = ByteArrayToImage(DirectCast(row.Cells("Image").Value, Byte()))
                PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
            Else
                PictureBox1.Image = Nothing
            End If

            ' Update button states after selecting a row
            UpdateButtonStates(isRowSelected:=True)
        End If
    End Sub




    Private Sub UpdateButtonStates(Optional ByVal isRowSelected As Boolean = False)
        Dim allFieldsEmpty As Boolean =
            String.IsNullOrWhiteSpace(txtLRN.Text) AndAlso
            String.IsNullOrWhiteSpace(txtFullName.Text) AndAlso
            cbYrLevel.SelectedIndex = -1 AndAlso
            cbSection.SelectedIndex = -1 AndAlso
            String.IsNullOrWhiteSpace(txtNumber.Text) AndAlso
            String.IsNullOrWhiteSpace(txtRFID.Text)

        If isRowSelected Then
            ' If a row is selected, only allow update/delete/clear
            btnAdd.Enabled = False
            btnUpdate.Enabled = True
            btnDelete.Enabled = True
            btnClear.Enabled = True
        ElseIf allFieldsEmpty Then
            ' When all fields are empty, only Add and Clear are enabled
            btnAdd.Enabled = True
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnClear.Enabled = True
        Else
            ' Fields are filled but no row selected — Add and Clear are available
            btnAdd.Enabled = True
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnClear.Enabled = True
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
        Dashboard.Show()
    End Sub

    Private Sub txtLRN_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtLRN.KeyPress
        ' Allow only digits and control keys (like Backspace)
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If

        ' Limit to 12 digits
        If Char.IsDigit(e.KeyChar) AndAlso txtLRN.Text.Length >= 12 Then
            e.Handled = True
        End If
    End Sub


    Private Sub txtRFID_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtRFID.KeyPress
        ' Allow only digits and control keys
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If

        ' Limit to 11 digits
        If Char.IsDigit(e.KeyChar) AndAlso txtRFID.Text.Length >= 11 Then
            e.Handled = True
        End If
    End Sub


    Private Sub txtNumber_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtNumber.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If

        ' Limit to 11 digits
        If Char.IsDigit(e.KeyChar) AndAlso txtNumber.Text.Length >= 11 Then
            e.Handled = True
        End If
    End Sub


    Private Sub txtFullName_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtFullName.KeyPress
        ' Allow letters, control keys (like Backspace), and space
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsLetter(e.KeyChar) AndAlso e.KeyChar <> " "c Then
            e.Handled = True
        End If
    End Sub

    Public Sub HighlightStudentRow(ByVal studentID As String)
        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.Cells("StudentID").Value.ToString() = studentID Then
                row.Selected = True
                DataGridView1.FirstDisplayedScrollingRowIndex = row.Index ' Scroll to it
                Exit For
            End If
        Next
    End Sub

    ' Handle TextChanged event for txtSearch
    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtSearch.TextChanged
        Dim searchQuery As String = txtSearch.Text.Trim()

        If String.IsNullOrEmpty(searchQuery) Then
            ' If no search query is entered, load all students
            LoadData()
        Else
            ' If there's a search query, filter the data
            Try
                OpenConnection()

                ' Modify the query to search by LRN or FullName
                Dim query As String = "SELECT * FROM tblStudent WHERE LRN LIKE @SearchQuery OR FullName LIKE @SearchQuery"
                cmd = New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@SearchQuery", "%" & searchQuery & "%")

                ' Load the filtered data into DataGridView
                adapter = New MySqlDataAdapter(cmd)
                dt = New DataTable()
                adapter.Fill(dt)
                DataGridView1.DataSource = dt

                ' Make the DataGridView non-editable but selectable
                DataGridView1.ReadOnly = True
                DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                DataGridView1.AllowUserToAddRows = False

                ' Hide the Image column if necessary
                If DataGridView1.Columns.Contains("Image") Then
                    DataGridView1.Columns("Image").Visible = False
                End If
                If DataGridView1.Columns.Contains("StudentID") Then
                    DataGridView1.Columns("StudentID").Visible = False
                End If

            Catch ex As Exception
                MsgBox("Error: " & ex.Message)
            Finally
                CloseConnection()
            End Try
        End If
    End Sub


    Private Sub lblStudentID_Click(sender As System.Object, e As System.EventArgs) Handles lblStudentID.Click

    End Sub

    Private Sub txtNumber_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtNumber.TextChanged

    End Sub

    Private Sub btnRecycle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecycle.Click
        Me.Hide()
        RecycleBin.Show()
    End Sub
    Private Sub btnAdd_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.MouseEnter
        btnAdd.BackColor = Color.LightGreen
        btnAdd.ForeColor = Color.White
        btnAdd.FlatStyle = FlatStyle.Flat

    End Sub

    ' Reset the button appearance when mouse leaves
    Private Sub btnAdd_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.MouseLeave
        btnAdd.BackColor = Color.DarkGreen
        btnAdd.ForeColor = Color.White
        btnAdd.FlatStyle = FlatStyle.Standard

    End Sub

    Private Sub btnUpdate_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.MouseEnter
        btnUpdate.BackColor = Color.LightBlue
        btnUpdate.ForeColor = Color.White
        btnUpdate.FlatStyle = FlatStyle.Flat

    End Sub

    ' Reset the button appearance when mouse leaves
    Private Sub btnUpdate_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.MouseLeave
        btnUpdate.BackColor = Color.DarkBlue
        btnUpdate.ForeColor = Color.White
        btnUpdate.FlatStyle = FlatStyle.Standard

    End Sub

    Private Sub btnDelete_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.MouseEnter
        btnDelete.BackColor = Color.Firebrick
        btnDelete.ForeColor = Color.White
        btnDelete.FlatStyle = FlatStyle.Flat

    End Sub

    ' Reset the button appearance when mouse leaves
    Private Sub btnDelete_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.MouseLeave
        btnDelete.BackColor = Color.DarkRed
        btnDelete.ForeColor = Color.White
        btnDelete.FlatStyle = FlatStyle.Standard

    End Sub

    Private Sub btnClear_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.MouseEnter
        btnClear.BackColor = Color.Gray
        btnClear.ForeColor = Color.White
        btnClear.FlatStyle = FlatStyle.Flat

    End Sub

    ' Reset the button appearance when mouse leaves
    Private Sub btnClear_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.MouseLeave
        btnClear.BackColor = Color.DarkGray
        btnClear.ForeColor = Color.White
        btnClear.FlatStyle = FlatStyle.Standard

    End Sub

    Private Sub btnBrowse_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowse.MouseLeave
        btnBrowse.BackColor = Color.LightBlue
        btnBrowse.ForeColor = Color.White
        btnBrowse.FlatStyle = FlatStyle.Standard

    End Sub

    Private Sub btnBrowse_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowse.MouseEnter
        btnBrowse.BackColor = Color.DarkBlue
        btnBrowse.ForeColor = Color.White
        btnBrowse.FlatStyle = FlatStyle.Flat

    End Sub

    Private Sub btnAcademicYear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAcademicYear.Click
        Me.Hide()
        Academic.Show()
    End Sub

    Private Sub btnAttendanceReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttendanceReport.Click
        Me.Hide()
        Report.Show()
    End Sub

    Private Sub btnAttendanceRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttendanceRecord.Click
        Me.Hide()
        DTR.Show()
    End Sub

    Private Sub btnManageAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnManageAccount.Click
        Me.Hide()
        Account.Show()
    End Sub

    Private Sub btnVerification_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVerification.Click
        Me.Hide()
        AttendanceVerify.Show()
    End Sub

    Private Sub btnLogOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogOut.Click
        Me.Hide()
        Login.Show()
    End Sub

    Private Sub btnAdvisoryClass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdvisoryClass.Click
        Me.Hide()
        AdvisoryClass.Show()
    End Sub

    Private Sub Button14_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button14.Click
        Me.Show()
    End Sub
End Class
