Imports MySql.Data.MySqlClient
Imports System.IO

Public Class Academic
    Private conn As MySqlConnection
    Private cmd As MySqlCommand
    Private adapter As MySqlDataAdapter
    Private dt As DataTable

    ' Form Load - Load Data into DataGridView
    Private Sub Academic_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        With DataGridView1
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToOrderColumns = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
        End With
        lblAcadID.Text = ""
        LoadData()
        UpdateButtonStates()
    End Sub

    ' Function to Open Database Connection
    Private Sub OpenConnection()
        Try
            ' Change the connection string to your XAMPP MySQL settings
            Dim connectionString As String = "Server=localhost;Database=attendance;Uid=root;Pwd=;"
            conn = New MySqlConnection(connectionString)
            conn.Open()
        Catch ex As Exception
            MsgBox("Connection Failed: " & ex.Message)
        End Try
    End Sub

    ' Function to Close Database Connection
    Private Sub CloseConnection()
        If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
            conn.Close()
        End If
    End Sub

    Private Sub LoadData()
        Try
            OpenConnection()
            Dim query As String = "SELECT AcademicID, Year FROM tblAcademic"
            adapter = New MySqlDataAdapter(query, conn)
            dt = New DataTable()
            adapter.Fill(dt)
            DataGridView1.DataSource = dt

            ' Hide the AcademicID column
            If DataGridView1.Columns.Contains("AcademicID") Then
                DataGridView1.Columns("AcademicID").Visible = False
            End If

        Catch ex As Exception
            MsgBox("Error Loading Data: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        If String.IsNullOrWhiteSpace(txtYear.Text) Then
            MsgBox("Please enter an academic year.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Try
            OpenConnection()

            ' Check for duplicates
            Dim checkQuery As String = "SELECT COUNT(*) FROM tblAcademic WHERE Year = @Year"
            cmd = New MySqlCommand(checkQuery, conn)
            cmd.Parameters.AddWithValue("@Year", txtYear.Text)
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

            If count > 0 Then
                MsgBox("This academic year already exists.", MsgBoxStyle.Information)
                Exit Sub
            End If

            ' Add new year if no duplicate
            Dim query As String = "INSERT INTO tblAcademic (Year) VALUES (@Year)"
            cmd = New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Year", txtYear.Text)
            cmd.ExecuteNonQuery()

            MsgBox("Academic Year Added Successfully!", MessageBoxIcon.Information)
            LoadData()
            txtYear.Clear()
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        Dim acadID As Integer
        If Not Integer.TryParse(lblAcadID.Text, acadID) Then
            MsgBox("Please select a valid academic year to update.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Try
            OpenConnection()
            Dim query As String = "UPDATE tblAcademic SET Year = @Year WHERE AcademicID = @AcademicID"
            cmd = New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Year", txtYear.Text)
            cmd.Parameters.AddWithValue("@AcademicID", acadID)
            cmd.ExecuteNonQuery()
            MsgBox("Academic Year Updated Successfully!", MessageBoxIcon.Information)
            LoadData()
            BtnClear_Click(Nothing, Nothing) ' Optional: clear after update
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Dim acadID As Integer
        If Not Integer.TryParse(lblAcadID.Text, acadID) Then
            MsgBox("Please select a valid academic year to delete.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                OpenConnection()
                Dim query As String = "DELETE FROM tblAcademic WHERE AcademicID = @AcademicID"
                cmd = New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@AcademicID", acadID)
                cmd.ExecuteNonQuery()
                MsgBox("Academic Year Deleted Successfully!", MessageBoxIcon.Information)
                LoadData()
                BtnClear_Click(Nothing, Nothing) ' Optional: clear after delete
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
            lblAcadID.Text = row.Cells("AcademicID").Value.ToString()
            txtYear.Text = row.Cells("Year").Value.ToString()

            ' Enable appropriate buttons
            UpdateButtonStates("GridClick")
        End If
    End Sub

    Private Sub BtnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        txtYear.Clear()
        lblAcadID.Text = ""
        UpdateButtonStates()
    End Sub

    Private Sub btnDefault_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDefault.Click
        If String.IsNullOrWhiteSpace(txtYear.Text) OrElse String.IsNullOrWhiteSpace(lblAcadID.Text) Then
            MsgBox("Please select an academic year to set as default.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Try
            OpenConnection()

            ' Unset previous default
            Dim unsetQuery As String = "UPDATE tblAcademic SET IsDefault = 0 WHERE IsDefault = 1"
            cmd = New MySqlCommand(unsetQuery, conn)
            cmd.ExecuteNonQuery()

            ' Set new default
            Dim setQuery As String = "UPDATE tblAcademic SET IsDefault = 1 WHERE AcademicID = @AcademicID"
            cmd = New MySqlCommand(setQuery, conn)
            cmd.Parameters.AddWithValue("@AcademicID", lblAcadID.Text)
            cmd.ExecuteNonQuery()

            ' Update Attendance form
            Attendance.AcademicYear = txtYear.Text

            MsgBox("Default academic year set successfully.", MsgBoxStyle.Information)

            ' Clear the fields after successfully setting the default
            BtnClear_Click(Nothing, Nothing) ' This will clear the text boxes and reset the button states

        Catch ex As Exception
            MsgBox("Error setting default: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
        Dashboard.Show()
    End Sub

    Private Sub UpdateButtonStates(Optional ByVal source As String = "")
        If source = "GridClick" Then
            btnAdd.Enabled = False
            btnUpdate.Enabled = True
            btnDelete.Enabled = True
            btnClear.Enabled = True
            btnDefault.Enabled = True

        ElseIf String.IsNullOrWhiteSpace(txtYear.Text) AndAlso String.IsNullOrWhiteSpace(lblAcadID.Text) Then
            btnAdd.Enabled = False
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnClear.Enabled = True
            btnDefault.Enabled = False

        ElseIf Not String.IsNullOrWhiteSpace(txtYear.Text) AndAlso String.IsNullOrWhiteSpace(lblAcadID.Text) Then
            btnAdd.Enabled = True
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnClear.Enabled = True
            btnDefault.Enabled = False

        ElseIf Not String.IsNullOrWhiteSpace(txtYear.Text) AndAlso Not String.IsNullOrWhiteSpace(lblAcadID.Text) Then
            btnAdd.Enabled = False
            btnUpdate.Enabled = True
            btnDelete.Enabled = True
            btnClear.Enabled = True
            btnDefault.Enabled = True

        Else
            btnAdd.Enabled = False
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnClear.Enabled = True
            btnDefault.Enabled = False
        End If
    End Sub

    Private Sub txtYear_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtYear.TextChanged
        UpdateButtonStates()
    End Sub

    Private Sub txtYear_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtYear.KeyPress
        ' Allow only digits, space, dash, and control keys like Backspace
        If Not Char.IsDigit(e.KeyChar) AndAlso
           Not Char.IsControl(e.KeyChar) AndAlso
           e.KeyChar <> " "c AndAlso
           e.KeyChar <> "-"c Then
            e.Handled = True ' Cancel the key press
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

    Private Sub btnDefault_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles btnDefault.MouseEnter
        btnDefault.BackColor = Color.LightGreen
        btnDefault.ForeColor = Color.White
        btnDefault.FlatStyle = FlatStyle.Flat

    End Sub

    ' Reset the button appearance when mouse leaves
    Private Sub btnDefault_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles btnDefault.MouseLeave
        btnDefault.BackColor = Color.DarkGreen
        btnDefault.ForeColor = Color.White
        btnDefault.FlatStyle = FlatStyle.Standard

    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub btnStudents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStudents.Click
        Me.Hide()
        ManageStudent.Show()
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
