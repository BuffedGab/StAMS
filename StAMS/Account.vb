Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text

Public Class Account
    Private conn As MySqlConnection
    Private cmd As MySqlCommand
    Private adapter As MySqlDataAdapter
    Private dt As DataTable


    Private Sub ManageUsers_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        LoadUsers()
        UpdateButtonStates()
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


    Private Sub LoadUsers()
        Try
            OpenConnection()
            Dim query As String = "SELECT EmployeeID, EmployeeNumber, FullName, Username, Role, Email FROM tblAccount"
            adapter = New MySqlDataAdapter(query, conn)
            dt = New DataTable()
            adapter.Fill(dt)
            DataGridView1.DataSource = dt

            With DataGridView1
                .ReadOnly = True
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .MultiSelect = False
            End With

        Catch ex As Exception
            MsgBox("Error Loading Data: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Function HashPassword(ByVal password As String) As String
        Dim sha256 As SHA256 = SHA256.Create()
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(password)
        Dim hash As Byte() = sha256.ComputeHash(bytes)
        Return Convert.ToBase64String(hash)
    End Function

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        If String.IsNullOrWhiteSpace(txtNumber.Text) OrElse String.IsNullOrWhiteSpace(txtFullName.Text) OrElse _
           String.IsNullOrWhiteSpace(txtUsername.Text) OrElse String.IsNullOrWhiteSpace(txtPassword.Text) OrElse _
           cbRole.SelectedIndex = -1 Then

            MsgBox("Please fill in all the fields!", MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Try
            OpenConnection()

            Dim checkQuery As String = "SELECT COUNT(*) FROM tblAccount WHERE EmployeeNumber = @EmployeeNumber OR Username = @Username"
            cmd = New MySqlCommand(checkQuery, conn)
            cmd.Parameters.AddWithValue("@EmployeeNumber", txtNumber.Text)
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text)

            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            If count > 0 Then
                MsgBox("User Already Exists!", MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            Dim query As String = "INSERT INTO tblAccount (EmployeeNumber, FullName, Username, Password, Role, Email) VALUES (@EmployeeNumber, @FullName, @Username, @Password, @Role, @Email)"
            cmd = New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@EmployeeNumber", txtNumber.Text)
            cmd.Parameters.AddWithValue("@FullName", txtFullName.Text)
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text)
            cmd.Parameters.AddWithValue("@Password", HashPassword(txtPassword.Text))
            cmd.Parameters.AddWithValue("@Role", cbRole.Text)
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text)
            cmd.ExecuteNonQuery()

            MsgBox("User Added Successfully!", MessageBoxIcon.Information)
            LoadUsers()
            btnClear.PerformClick()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub


    Private Sub chkChangePassword_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkChangePassword.CheckedChanged
        ' Enable or disable password field based on checkbox
        txtPassword.Enabled = chkChangePassword.Checked
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click
        If String.IsNullOrWhiteSpace(lblID.Text) Then
            MsgBox("Please select a user to update!")
            Exit Sub
        End If

        Try
            OpenConnection()

            ' If the checkbox is unchecked, don't update the password
            Dim query As String
            If chkChangePassword.Checked Then
                query = "UPDATE tblAccount SET EmployeeNumber = @EmployeeNumber, FullName = @FullName, Username = @Username, Password = @Password, Role = @Role, Email = @Email WHERE EmployeeID = @EmployeeID"
                cmd.Parameters.AddWithValue("@Password", HashPassword(txtPassword.Text))
            Else
                query = "UPDATE tblAccount SET EmployeeNumber = @EmployeeNumber, FullName = @FullName, Username = @Username, Role = @Role, Email = @Email WHERE EmployeeID = @EmployeeID"
            End If

            cmd = New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@EmployeeID", lblID.Text)
            cmd.Parameters.AddWithValue("@EmployeeNumber", txtNumber.Text)
            cmd.Parameters.AddWithValue("@FullName", txtFullName.Text)
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text)
            cmd.Parameters.AddWithValue("@Role", cbRole.Text)
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text)
            cmd.ExecuteNonQuery()

            MsgBox("User Updated Successfully!", MessageBoxIcon.Information)
            LoadUsers()
            btnClear.PerformClick()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        If MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                OpenConnection()
                Dim query As String = "DELETE FROM tblAccount WHERE EmployeeID = @EmployeeID"
                cmd = New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@EmployeeID", lblID.Text)
                cmd.ExecuteNonQuery()
                MsgBox("User Deleted Successfully!", MessageBoxIcon.Information)
                LoadUsers()
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
            lblID.Text = row.Cells("EmployeeID").Value.ToString()
            txtNumber.Text = row.Cells("EmployeeNumber").Value.ToString()
            txtFullName.Text = row.Cells("FullName").Value.ToString()
            txtUsername.Text = row.Cells("Username").Value.ToString()
            cbRole.Text = row.Cells("Role").Value.ToString()
            txtPassword.Text = ""
            txtEmail.Text = row.Cells("Email").Value.ToString()
            UpdateButtonStates()
        End If
    End Sub


    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        txtNumber.Clear()
        txtFullName.Clear()
        txtUsername.Clear()
        txtPassword.Clear()
        txtEmail.Clear()
        cbRole.SelectedIndex = -1
        lblID.Text = ""
        UpdateButtonStates()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
        Dashboard.Show()
    End Sub

    Private Sub UpdateButtonStates()
        Dim isTyping As Boolean = Not String.IsNullOrWhiteSpace(txtNumber.Text) OrElse
                                  Not String.IsNullOrWhiteSpace(txtFullName.Text) OrElse
                                  Not String.IsNullOrWhiteSpace(txtUsername.Text) OrElse
                                  Not String.IsNullOrWhiteSpace(txtPassword.Text) OrElse
                                  Not String.IsNullOrWhiteSpace(txtEmail.Text) OrElse
                                  cbRole.SelectedIndex <> -1

        Dim hasSelection As Boolean = Not String.IsNullOrWhiteSpace(txtNumber.Text)

        If Not isTyping AndAlso Not hasSelection Then
            btnAdd.Enabled = True
            btnClear.Enabled = True
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
        ElseIf hasSelection Then
            btnAdd.Enabled = False
            btnUpdate.Enabled = True
            btnDelete.Enabled = True
            btnClear.Enabled = True
        Else
            btnAdd.Enabled = True
            btnUpdate.Enabled = False
            btnDelete.Enabled = False
            btnClear.Enabled = True
        End If
    End Sub

    Private Sub txtNumber_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtNumber.KeyPress
        ' Allow only digits and control keys (like Backspace)
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If

        ' Limit to 11 digits
        If Char.IsDigit(e.KeyChar) AndAlso txtNumber.Text.Length >= 11 Then
            e.Handled = True
        End If
    End Sub


    Private Sub txtFullName_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtFullName.KeyPress
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtSearch.TextChanged
        Try
            OpenConnection()
            Dim query As String = "SELECT EmployeeID, EmployeeNumber, FullName, Username, Role, Email FROM tblAccount " &
                                  "WHERE FullName LIKE @search OR EmployeeNumber LIKE @search OR Username LIKE @search"
            cmd = New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@search", "%" & txtSearch.Text & "%")

            adapter = New MySqlDataAdapter(cmd)
            dt = New DataTable()
            adapter.Fill(dt)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MsgBox("Search Error: " & ex.Message)
        Finally
            CloseConnection()
        End Try
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
End Class
