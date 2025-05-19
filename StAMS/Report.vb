Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing
Imports System.IO

Public Class Report
    Private printRowIndex As Integer = 0
    Private Sub Report_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadAdvisoryFilters()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
        Dashboard.Show()
    End Sub

    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Try
            Dim connStr As String = "Server=localhost;Database=attendance;Uid=root;Pwd=;"
            Using conn As New MySqlConnection(connStr)
                conn.Open()

                ' Get filters
                Dim yearLevel As String = cbYrLevel.Text.Trim()
                Dim course As String = cbSection.Text.Trim()
                Dim fromDate As Date = dtpFrom.Value.Date
                Dim toDate As Date = dtpTo.Value.Date

                ' Base query
                Dim query As String = "SELECT Date, Time, FullName, YearLevel, Section, Late, Absent FROM tblDTR WHERE 1=1"

                ' Filter by attendance type
                If rbPresent.Checked Then
                    query &= " AND Time IS NOT NULL AND Absent IS NULL"
                ElseIf rbAbsent.Checked Then
                    query &= " AND Absent IS NOT NULL"
                ElseIf rbLate.Checked Then
                    query &= " AND Late IS NOT NULL"
                End If

                ' Additional filters
                If Not String.IsNullOrWhiteSpace(yearLevel) Then
                    query &= " AND YearLevel = @YearLevel"
                End If
                If Not String.IsNullOrWhiteSpace(course) Then
                    query &= " AND Section = @Section"
                End If

                query &= " AND Date BETWEEN @FromDate AND @ToDate"

                ' Prepare command
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@FromDate", fromDate)
                    cmd.Parameters.AddWithValue("@ToDate", toDate)

                    If Not String.IsNullOrWhiteSpace(yearLevel) Then
                        cmd.Parameters.AddWithValue("@YearLevel", yearLevel)
                    End If
                    If Not String.IsNullOrWhiteSpace(course) Then
                        cmd.Parameters.AddWithValue("@Section", course)
                    End If

                    ' Fill DataGridView
                    Dim dt As New DataTable()
                    Dim adapter As New MySqlDataAdapter(cmd)
                    adapter.Fill(dt)
                    DataGridView1.DataSource = dt
                    DataGridView1.ReadOnly = True
                    DataGridView1.AllowUserToAddRows = False
                    DataGridView1.AllowUserToDeleteRows = False
                    DataGridView1.AllowUserToOrderColumns = False
                    DataGridView1.AllowUserToResizeRows = False
                    DataGridView1.MultiSelect = False
                    DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect

                End Using
            End Using

        Catch ex As Exception
            MsgBox("Error loading attendance: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub LoadAdvisoryFilters()
        Dim connStr As String = "Server=localhost;Database=attendance;Uid=root;Pwd=;"
        Using conn As New MySqlConnection(connStr)
            Try
                conn.Open()

                ' Load unique YearLevels
                Dim yearQuery As String = "SELECT DISTINCT YearLevel FROM tblAdvisory ORDER BY YearLevel"
                Using yearCmd As New MySqlCommand(yearQuery, conn)
                    Using reader As MySqlDataReader = yearCmd.ExecuteReader()
                        cbYrLevel.Items.Clear()
                        While reader.Read()
                            cbYrLevel.Items.Add(reader("YearLevel").ToString())
                        End While
                    End Using
                End Using

                ' Load unique Sections
                Dim sectionQuery As String = "SELECT DISTINCT Section FROM tblAdvisory ORDER BY Section"
                Using sectionCmd As New MySqlCommand(sectionQuery, conn)
                    Using reader As MySqlDataReader = sectionCmd.ExecuteReader()
                        cbSection.Items.Clear()
                        While reader.Read()
                            cbSection.Items.Add(reader("Section").ToString())
                        End While
                    End Using
                End Using

            Catch ex As Exception
                MsgBox("Error loading advisory data: " & ex.Message, MsgBoxStyle.Critical)
            End Try
        End Using
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Try
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "CSV Files|*.csv"
            saveFileDialog.Title = "Save Attendance Report"
            saveFileDialog.FileName = "AttendanceReport.csv"

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                Using writer As New StreamWriter(saveFileDialog.FileName)
                    ' Write column headers
                    Dim headers As New List(Of String)
                    For Each column As DataGridViewColumn In DataGridView1.Columns
                        headers.Add(column.HeaderText)
                    Next
                    writer.WriteLine(String.Join(",", headers))

                    ' Write data rows
                    For Each row As DataGridViewRow In DataGridView1.Rows
                        If Not row.IsNewRow Then
                            Dim rowData As New List(Of String)
                            For Each cell As DataGridViewCell In row.Cells
                                Dim value As String
                                If cell.Value IsNot Nothing Then
                                    If TypeOf cell.Value Is Date Then
                                        value = CType(cell.Value, Date).ToString("yyyy-MM-dd") ' Change format if needed
                                    Else
                                        value = cell.Value.ToString()
                                    End If
                                Else
                                    value = ""
                                End If

                                ' Escape double quotes and wrap in quotes if it contains a comma
                                If value.Contains(",") OrElse value.Contains("""") Then
                                    value = """" & value.Replace("""", """""") & """"
                                End If

                                rowData.Add(value)
                            Next
                            writer.WriteLine(String.Join(",", rowData))
                        End If
                    Next
                End Using

                MessageBox.Show("CSV file saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Error saving CSV file: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cbYrLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbYrLevel.SelectedIndexChanged
        Dim selectedYearLevel As String = cbYrLevel.Text.Trim()
        cbSection.Items.Clear()

        Dim connStr As String = "Server=localhost;Database=attendance;Uid=root;Pwd=;"
        Using conn As New MySqlConnection(connStr)
            Try
                conn.Open()
                Dim query As String = "SELECT DISTINCT Section FROM tblAdvisory WHERE YearLevel = @YearLevel ORDER BY Section"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@YearLevel", selectedYearLevel)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            cbSection.Items.Add(reader("Section").ToString())
                        End While
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error loading sections: " & ex.Message, MsgBoxStyle.Critical)
            End Try
        End Using
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

    Private Sub btnBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackup.Click
        Me.Hide()
        BackupDatabase.Show()
    End Sub
End Class