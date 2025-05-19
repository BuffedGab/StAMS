Imports MySql.Data.MySqlClient

Public Class Dashboard

    Public Sub UpdateStudentCount()
        Try
            Dim connStr As String = "Server=localhost;Database=attendance;Uid=root;Pwd=;"
            Using conn As New MySqlConnection(connStr)
                conn.Open()
                Dim query As String = "SELECT COUNT(*) FROM tblStudent"
                Using cmd As New MySqlCommand(query, conn)
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                    lblStudent.Text = count.ToString()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error updating student count: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Public Sub UpdateSectionCount()
        Try
            Dim connStr As String = "Server=localhost;Database=attendance;Uid=root;Pwd=;"
            Using conn As New MySqlConnection(connStr)
                conn.Open()
                Dim query As String = "SELECT DISTINCT Section FROM tblAdvisory"
                Using cmd As New MySqlCommand(query, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        Dim sections As New List(Of String)()

                        While reader.Read()
                            sections.Add(reader("Section").ToString())
                        End While

                        ' Join with new lines for vertical display
                        lblSection.Text = String.Join(Environment.NewLine, sections)

                        ' Center text horizontally and vertically
                        lblSection.TextAlign = ContentAlignment.MiddleCenter
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error loading sections: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnAttendance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttendance.Click
        Me.Hide()
        Attendance.Show()
    End Sub

    Private Sub Dashboard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UpdateStudentCount()
        UpdateSectionCount()
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
