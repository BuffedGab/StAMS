Imports MySql.Data.MySqlClient

Public Class AdvisoryClass
    Private conn As MySqlConnection
    Private cmd As MySqlCommand
    Private adapter As MySqlDataAdapter
    Private dt As DataTable


    Private Sub AdvisoryClass_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        LoadAdvisory()
        LoadYearLevels()

        cbYrLevel.Items.Clear()
        cbYrLevel.Items.Add("Grade 11")
        cbYrLevel.Items.Add("Grade 12")

        ToggleButtons()

        DataGridView1.ReadOnly = True
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.AllowUserToResizeRows = False
        DataGridView1.MultiSelect = False

        DataGridView2.ReadOnly = True
        DataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView2.AllowUserToAddRows = False
        DataGridView2.AllowUserToDeleteRows = False
        DataGridView2.AllowUserToResizeRows = False
        DataGridView2.MultiSelect = False
    End Sub

    Private Sub ToggleButtons()
        If String.IsNullOrWhiteSpace(txtSection.Text) Or cbYrLevel.SelectedIndex = -1 Then
            btnAdd.Enabled = True
            btnClear.Enabled = True
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
        Else
            btnAdd.Enabled = True
            btnClear.Enabled = True
            btnUpdate.Enabled = True
            btnDelete.Enabled = True
        End If
    End Sub

    Private Sub OpenConnection()
        Try
            Dim connectionString As String = "Server=localhost;Database=attendance;Uid=root;Pwd=;"
            conn = New MySqlConnection(connectionString)
            conn.Open()
        Catch ex As Exception
            MsgBox("Connection Failed: " & ex.Message)
        End Try
    End Sub

    Private Sub CloseConnection()
        If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
            conn.Close()
        End If
    End Sub

    Private Sub LoadAdvisory()
        Try
            OpenConnection()
            Dim query As String = "SELECT AdvisoryID, YearLevel, Section FROM tblAdvisory"
            adapter = New MySqlDataAdapter(query, conn)
            dt = New DataTable()
            adapter.Fill(dt)
            DataGridView1.DataSource = dt

            DataGridView1.Columns("AdvisoryID").Visible = False
        Catch ex As Exception
            MsgBox("Error Loading Data: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub


    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        If String.IsNullOrEmpty(cbYrLevel.Text) Or String.IsNullOrEmpty(txtSection.Text) Then
            MsgBox("Please fill in all fields (Year Level and Section).", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Try
            OpenConnection()

            ' Check if any advisory already exists
            Dim totalCheckQuery As String = "SELECT COUNT(*) FROM tblAdvisory"
            cmd = New MySqlCommand(totalCheckQuery, conn)
            Dim totalCount As Integer = Convert.ToInt32(cmd.ExecuteScalar())

            If totalCount > 0 Then
                MsgBox("Only one advisory (YearLevel + Section) is allowed. Please update or delete the current one.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            ' Insert new advisory
            Dim insertQuery As String = "INSERT INTO tblAdvisory (YearLevel, Section) VALUES (@YearLevel, @Section)"
            cmd = New MySqlCommand(insertQuery, conn)
            cmd.Parameters.AddWithValue("@YearLevel", cbYrLevel.Text)
            cmd.Parameters.AddWithValue("@Section", txtSection.Text)
            cmd.ExecuteNonQuery()

            MsgBox("Advisory Added Successfully!", MessageBoxIcon.Information)
            LoadAdvisory()
            Dashboard.UpdateSectionCount()
            btnClear.PerformClick()
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub



    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Try
            OpenConnection()

            Dim checkQuery As String = "SELECT COUNT(*) FROM tblAdvisory WHERE Section = @Section AND AdvisoryID <> @AdvisoryID"
            cmd = New MySqlCommand(checkQuery, conn)
            cmd.Parameters.AddWithValue("@Section", txtSection.Text)
            cmd.Parameters.AddWithValue("@AdvisoryID", lblAdvisoryID.Text)
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

            If count > 0 Then
                MsgBox("Another advisory already uses this section name.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            Dim updateQuery As String = "UPDATE tblAdvisory SET YearLevel=@YearLevel, Section=@Section WHERE AdvisoryID=@AdvisoryID"
            cmd = New MySqlCommand(updateQuery, conn)
            cmd.Parameters.AddWithValue("@AdvisoryID", lblAdvisoryID.Text)
            cmd.Parameters.AddWithValue("@YearLevel", cbYrLevel.Text)
            cmd.Parameters.AddWithValue("@Section", txtSection.Text)
            cmd.ExecuteNonQuery()
            MsgBox("Advisory Updated Successfully!", MessageBoxIcon.Information)
            LoadAdvisory()
            btnClear.PerformClick()
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        If MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                OpenConnection()
                Dim query As String = "DELETE FROM tblAdvisory WHERE AdvisoryID=@AdvisoryID"
                cmd = New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@AdvisoryID", lblAdvisoryID.Text)
                cmd.ExecuteNonQuery()
                MsgBox("Advisory Deleted Successfully!", MessageBoxIcon.Information)
                LoadAdvisory()
                Dashboard.UpdateSectionCount()
                btnClear.PerformClick()
            Catch ex As Exception
                MsgBox("Error: " & ex.Message)
            Finally
                CloseConnection()
            End Try
        End If
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            lblAdvisoryID.Text = row.Cells("AdvisoryID").Value.ToString()
            cbYrLevel.Text = row.Cells("YearLevel").Value.ToString()
            txtSection.Text = row.Cells("Section").Value.ToString()

            LoadStudentsBySection(txtSection.Text)

            btnAdd.Enabled = False
            btnUpdate.Enabled = True
            btnDelete.Enabled = True
            btnClear.Enabled = True
        End If
    End Sub


    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        cbYrLevel.SelectedIndex = -1
        txtSection.Clear()
        lblAdvisoryID.Text = ""

        ToggleButtons()
    End Sub

    Private Sub LoadYearLevels()
        Try
            OpenConnection()
            Dim query As String = "SELECT DISTINCT YearLevel FROM tblAdvisory ORDER BY YearLevel ASC"
            cmd = New MySqlCommand(query, conn)
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            cbGradeLvl.Items.Clear()
            cbGradeLvl.Items.Add("All")

            While reader.Read()
                cbGradeLvl.Items.Add(reader("YearLevel").ToString())
            End While

            reader.Close()
            cbGradeLvl.SelectedIndex = 0
        Catch ex As Exception
            MsgBox("Error Loading Year Levels: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub cbGradeLvl_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbGradeLvl.SelectedIndexChanged
        Try
            OpenConnection()
            Dim query As String

            If cbGradeLvl.Text = "All" Then
                query = "SELECT AdvisoryID, YearLevel, Section FROM tblAdvisory"
            Else
                query = "SELECT AdvisoryID, YearLevel, Section FROM tblAdvisory WHERE YearLevel = @YearLevel"
            End If

            adapter = New MySqlDataAdapter(query, conn)
            adapter.SelectCommand.Parameters.AddWithValue("@YearLevel", cbGradeLvl.Text)

            dt = New DataTable()
            adapter.Fill(dt)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MsgBox("Error Filtering Data: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub LoadStudentsBySection(ByVal section As String)
        Try
            OpenConnection()
            Dim query As String = "SELECT StudentID, LRN, FullName, YearLevel, Section, Contact, RFID FROM tblStudent WHERE Section = @Section"
            adapter = New MySqlDataAdapter(query, conn)
            adapter.SelectCommand.Parameters.AddWithValue("@Section", section)

            dt = New DataTable()
            adapter.Fill(dt)
            DataGridView2.DataSource = dt

            DataGridView2.Columns("StudentID").Visible = False
        Catch ex As Exception
            MsgBox("Error Loading Students: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub


    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
        Dashboard.Show()
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtSearch.TextChanged
        Dim keyword As String = txtSearch.Text.Trim()

        If keyword = "" AndAlso Not String.IsNullOrWhiteSpace(txtSection.Text) Then
            LoadStudentsBySection(txtSection.Text)
            Return
        End If

        Try
            OpenConnection()
            Dim query As String = "SELECT StudentID, LRN, FullName, YearLevel, Section, Contact, RFID " &
                                  "FROM tblStudent WHERE FullName LIKE @Keyword OR LRN LIKE @Keyword"
            adapter = New MySqlDataAdapter(query, conn)
            adapter.SelectCommand.Parameters.AddWithValue("@Keyword", "%" & keyword & "%")

            dt = New DataTable()
            adapter.Fill(dt)
            DataGridView2.DataSource = dt

            If DataGridView2.Columns.Contains("StudentID") Then
                DataGridView2.Columns("StudentID").Visible = False
            End If
        Catch ex As Exception
            MsgBox("Error searching students: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub


    Private Sub DataGridView2_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles DataGridView2.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridView2.Rows(e.RowIndex)

            Dim studentID As String = row.Cells("StudentID").Value.ToString()
            Dim lrn As String = row.Cells("LRN").Value.ToString()
            Dim fullName As String = row.Cells("FullName").Value.ToString()
            Dim yearLevel As String = row.Cells("YearLevel").Value.ToString()
            Dim section As String = row.Cells("Section").Value.ToString()
            Dim contact As String = row.Cells("Contact").Value.ToString()
            Dim rfid As String = row.Cells("RFID").Value.ToString()

            ManageStudent.LoadStudentData(studentID, lrn, fullName, yearLevel, section, contact, rfid)
            ManageStudent.Show()
            ManageStudent.BringToFront()

            ManageStudent.HighlightStudentRow(studentID)
        End If
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

    Private Sub btnStudents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStudents.Click
        Me.Hide()
        ManageStudent.Show()
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
End Class
