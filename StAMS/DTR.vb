Imports MySql.Data.MySqlClient

Public Class DTR

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
        Dashboard.Show()
    End Sub

    Private Sub DTR_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        With DataGridView1
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = True
            .AllowUserToResizeRows = False
            .MultiSelect = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .DefaultCellStyle.SelectionBackColor = .DefaultCellStyle.BackColor
            .DefaultCellStyle.SelectionForeColor = .DefaultCellStyle.ForeColor
            .EnableHeadersVisualStyles = False
        End With

        dtpFrom.Value = Date.Today
        dtpTo.Value = Date.Today
    End Sub

    Private Sub btnFilter_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.Click
        Dim connStr As String = "Server=localhost;Database=attendance;Uid=root;Pwd=;"

        Try
            Using conn As New MySqlConnection(connStr)
                conn.Open()

                Dim fromDate As Date = dtpFrom.Value.Date
                Dim toDate As Date = dtpTo.Value.Date

                Dim query As String = "SELECT FullName, YearLevel, Section, Time, Date, Late, Absent " &
                                      "FROM tblDTR " &
                                      "WHERE Date BETWEEN @FromDate AND @ToDate"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@FromDate", fromDate)
                    cmd.Parameters.AddWithValue("@ToDate", toDate)

                    Dim dt As New DataTable()
                    Dim adapter As New MySqlDataAdapter(cmd)
                    adapter.Fill(dt)
                    DataGridView1.DataSource = dt
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error fetching data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnFilter_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.MouseEnter
        btnFilter.BackColor = Color.LightBlue
        btnFilter.ForeColor = Color.White
        btnFilter.FlatStyle = FlatStyle.Flat

    End Sub

    ' Reset the button appearance when mouse leaves
    Private Sub btnUpdate_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles btnFilter.MouseLeave
        btnFilter.BackColor = Color.DarkBlue
        btnFilter.ForeColor = Color.White
        btnFilter.FlatStyle = FlatStyle.Standard

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