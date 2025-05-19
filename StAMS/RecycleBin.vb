Imports MySql.Data.MySqlClient

Public Class RecycleBin

    Private Sub RecycleBin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dgvDeletedStudents.MultiSelect = True
        dgvDeletedStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect
    End Sub

    Private Sub dgvDeletedStudents_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgvDeletedStudents.SelectionChanged
        ' Enable the Restore button only if one or more rows are selected
        btnRestore.Enabled = dgvDeletedStudents.SelectedRows.Count > 0
    End Sub

    Private Sub dgvDeletedStudents_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) _
    Handles dgvDeletedStudents.CellClick
        If e.RowIndex >= 0 AndAlso dgvDeletedStudents.Columns(e.ColumnIndex).Name = "Delete" Then
            Dim studentID As String = dgvDeletedStudents.Rows(e.RowIndex).Cells("StudentID").Value.ToString()

            If MessageBox.Show("Permanently delete this student?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                Try
                    Dim con As New MySqlConnection("Server=localhost;Database=attendance;Uid=root;Pwd=;")
                    con.Open()

                    Dim query As String = "DELETE FROM tblStudent WHERE StudentID = @id"
                    Using cmd As New MySqlCommand(query, con)
                        cmd.Parameters.AddWithValue("@id", studentID)
                        cmd.ExecuteNonQuery()
                    End Using

                    con.Close()
                    MessageBox.Show("Student deleted permanently.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadDeletedStudents()

                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End If
        End If
    End Sub

    Private Sub btnRestore_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRestore.Click
        Dim selectedCount As Integer = 0

        ' Check if the "Select" column exists before proceeding
        If dgvDeletedStudents.Columns.Contains("Select") Then
            For Each row As DataGridViewRow In dgvDeletedStudents.Rows
                ' Ensure the cell exists and has a value of True before incrementing selectedCount
                If row.Cells("Select") IsNot Nothing AndAlso Convert.ToBoolean(row.Cells("Select").Value) = True Then
                    selectedCount += 1
                End If
            Next
        End If

        If selectedCount = 0 Then
            MessageBox.Show("Please check at least one student to restore.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If MessageBox.Show("Are you sure you want to restore the selected student(s)?", "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Dim con As New MySqlConnection("Server=localhost;Database=attendance;Uid=root;Pwd=;")
                con.Open()

                ' Ensure the "Select" column exists and is being accessed properly
                If dgvDeletedStudents.Columns.Contains("Select") Then
                    For Each row As DataGridViewRow In dgvDeletedStudents.Rows
                        ' Check if the cell is not null and the value is True
                        If row.Cells("Select") IsNot Nothing AndAlso Convert.ToBoolean(row.Cells("Select").Value) = True Then
                            Dim studentID As String = row.Cells("StudentID").Value.ToString()

                            Dim query As String = "UPDATE tblStudent SET IsDeleted = 0 WHERE StudentID = @id"
                            Using cmd As New MySqlCommand(query, con)
                                cmd.Parameters.AddWithValue("@id", studentID)
                                cmd.ExecuteNonQuery()
                            End Using
                        End If
                    Next
                End If

                con.Close()
                MessageBox.Show("Selected student(s) restored successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadDeletedStudents() ' Refresh after restore

            Catch ex As Exception
                MessageBox.Show("Error restoring student(s): " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub LoadDeletedStudents()
        Try
            Dim con As New MySqlConnection("Server=localhost;Database=attendance;Uid=root;Pwd=;")
            con.Open()

            Dim query As String = "SELECT StudentID, FullName, LRN, YearLevel, Section FROM tblStudent WHERE IsDeleted = 1"
            Dim adapter As New MySqlDataAdapter(query, con)
            Dim table As New DataTable()
            adapter.Fill(table)

            dgvDeletedStudents.DataSource = table

            ' Add checkbox and delete columns only once
            If Not dgvDeletedStudents.Columns.Contains("Select") Then
                AddCheckboxColumn()
            End If

            If Not dgvDeletedStudents.Columns.Contains("Delete") Then
                AddDeleteButtonColumn()
            End If

            ' ✅ Safely initialize checkbox values to False
            If dgvDeletedStudents.Columns.Contains("Select") Then
                For Each row As DataGridViewRow In dgvDeletedStudents.Rows
                    If Not row.IsNewRow Then
                        Try
                            row.Cells("Select").Value = False
                        Catch ex As Exception
                            ' Just in case
                        End Try
                    End If
                Next
            End If

            con.Close()
        Catch ex As Exception
            MessageBox.Show("Error loading deleted students: " & ex.Message)
        End Try
    End Sub


    Private Sub AddCheckboxColumn()
        If Not dgvDeletedStudents.Columns.Contains("Select") Then
            Dim chk As New DataGridViewCheckBoxColumn()
            chk.Name = "Select"
            chk.HeaderText = ""
            chk.Width = 30
            chk.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvDeletedStudents.Columns.Insert(0, chk)
        End If
    End Sub

    Private Sub AddDeleteButtonColumn()
        If Not dgvDeletedStudents.Columns.Contains("Delete") Then
            Dim deleteBtn As New DataGridViewButtonColumn()
            deleteBtn.Name = "Delete"
            deleteBtn.HeaderText = "Action"
            deleteBtn.Text = "Delete"
            deleteBtn.UseColumnTextForButtonValue = True
            deleteBtn.Width = 80
            dgvDeletedStudents.Columns.Add(deleteBtn)
        End If
    End Sub

    Private Sub dgvDeletedStudents_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) _
    Handles dgvDeletedStudents.CellValueChanged
        If dgvDeletedStudents.Columns.Contains("Select") AndAlso e.ColumnIndex = dgvDeletedStudents.Columns("Select").Index Then
            UpdateActionButtons()
        End If
    End Sub

    Private Sub btnLoadDeleted_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLoadDeleted.Click
        LoadDeletedStudents()
    End Sub

    Private Sub btnSelectAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSelectAll.Click
        If dgvDeletedStudents.Columns.Contains("Select") Then
            For Each row As DataGridViewRow In dgvDeletedStudents.Rows
                If Not row.IsNewRow Then
                    row.Cells("Select").Value = True
                End If
            Next
        End If

        UpdateActionButtons()
    End Sub


    Private Sub btnDeselectAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeselectAll.Click
        If dgvDeletedStudents.Columns.Contains("Select") Then
            For Each row As DataGridViewRow In dgvDeletedStudents.Rows
                If Not row.IsNewRow Then
                    row.Cells("Select").Value = False
                End If
            Next
        End If

        UpdateActionButtons()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Dim selectedCount As Integer = 0

        If dgvDeletedStudents.Columns.Contains("Select") Then
            For Each row As DataGridViewRow In dgvDeletedStudents.Rows
                If row.Cells("Select") IsNot Nothing AndAlso Convert.ToBoolean(row.Cells("Select").Value) = True Then
                    selectedCount += 1
                End If
            Next
        End If

        If selectedCount = 0 Then
            MessageBox.Show("Please check at least one student to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If MessageBox.Show("Are you sure you want to permanently delete the selected student(s)?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim con As New MySqlConnection("Server=localhost;Database=attendance;Uid=root;Pwd=;")
                con.Open()

                For Each row As DataGridViewRow In dgvDeletedStudents.Rows
                    If row.Cells("Select") IsNot Nothing AndAlso Convert.ToBoolean(row.Cells("Select").Value) = True Then
                        Dim studentID As String = row.Cells("StudentID").Value.ToString()

                        Dim query As String = "DELETE FROM tblStudent WHERE StudentID = @id"
                        Using cmd As New MySqlCommand(query, con)
                            cmd.Parameters.AddWithValue("@id", studentID)
                            cmd.ExecuteNonQuery()
                        End Using
                    End If
                Next

                con.Close()
                MessageBox.Show("Selected student(s) permanently deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadDeletedStudents()

            Catch ex As Exception
                MessageBox.Show("Error deleting student(s): " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub UpdateActionButtons()
        Dim anyChecked As Boolean = False

        For Each row As DataGridViewRow In dgvDeletedStudents.Rows
            If Not row.IsNewRow AndAlso row.Cells("Select") IsNot Nothing Then
                Dim cellValue = row.Cells("Select").Value

                If cellValue IsNot Nothing AndAlso Not IsDBNull(cellValue) Then
                    If Convert.ToBoolean(cellValue) = True Then
                        anyChecked = True
                        Exit For
                    End If
                End If
            End If
        Next

        btnRestore.Enabled = anyChecked
        btnDelete.Enabled = anyChecked
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Me.Hide()
        ManageStudent.Show()
    End Sub
End Class
