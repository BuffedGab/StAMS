Imports MySql.Data.MySqlClient
Imports System.IO

Public Class AttendanceVerify
    Private conn As MySqlConnection
    Private cmd As MySqlCommand
    Private reader As MySqlDataReader

    ' Open database connection
    Private Sub OpenConnection()
        conn = New MySqlConnection("server=localhost;userid=root;password=;database=attendance;")
        conn.Open()
    End Sub

    ' Close database connection
    Private Sub CloseConnection()
        If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
            conn.Close()
        End If
    End Sub

    ' Handle the RFID button click event
    Private Sub btnRFID_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRFID.Click
        If String.IsNullOrWhiteSpace(txtRFID.Text) Then
            MsgBox("Please enter or scan RFID.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Try
            ' Open database connection
            OpenConnection()

            ' Step 1: Fetch from tblDTR using RFID and the selected date from dtpDate
            ' Only fetch records where Time or Late are not empty or null, and Absent is not set (0 or NULL)
            Dim queryDTR As String = "SELECT * FROM tblDTR WHERE RFID = @rfid AND DATE(Date) = @date AND (Time IS NOT NULL OR Late IS NOT NULL) AND (Absent IS NULL OR Absent = '0') ORDER BY AttendanceID DESC LIMIT 1"
            cmd = New MySqlCommand(queryDTR, conn)
            cmd.Parameters.AddWithValue("@rfid", txtRFID.Text)
            cmd.Parameters.AddWithValue("@date", dtpDate.Value.ToString("yyyy-MM-dd"))

            reader = cmd.ExecuteReader()

            If reader.Read() Then
                ' If a record is found, display the data
                lblLRN.Text = reader("LRN").ToString()
                txtRFID.Text = reader("RFID").ToString()
                lblFullName.Text = reader("FullName").ToString()
                lblYearLevel.Text = reader("YearLevel").ToString()
                lblSection.Text = reader("Section").ToString()
                lblContact.Text = reader("Contact").ToString()
                lblTime.Text = reader("Time").ToString()

                ' Fetch and display the Date
                If Not IsDBNull(reader("Date")) AndAlso Not String.IsNullOrEmpty(reader("Date").ToString()) Then
                    Dim fetchedDate As DateTime
                    If DateTime.TryParse(reader("Date").ToString(), fetchedDate) Then
                        Dim localDate As DateTime = fetchedDate.ToLocalTime()
                        lblDate.Text = localDate.ToString("MM/dd/yyyy")
                    Else
                        lblDate.Text = "Invalid Date"
                    End If
                Else
                    lblDate.Text = "No Date"
                End If

                lblLate.Text = reader("Late").ToString()
                lblAbsent.Text = reader("Absent").ToString()

                ' Load CapturedImage
                If Not IsDBNull(reader("CapturedImage")) Then
                    Dim imgData As Byte() = CType(reader("CapturedImage"), Byte())
                    Using ms As New MemoryStream(imgData)
                        pbCapturedImage.Image = Image.FromStream(ms)
                    End Using
                Else
                    pbCapturedImage.Image = Nothing
                End If
            Else
                ' No valid record or student is absent
                MsgBox("No valid attendance record found for this RFID or the student is marked absent.", MsgBoxStyle.Information)

                ' Clear all labels and images
                lblLRN.Text = ""
                lblFullName.Text = ""
                lblYearLevel.Text = ""
                lblSection.Text = ""
                lblContact.Text = ""
                lblTime.Text = ""
                lblDate.Text = ""
                lblLate.Text = ""
                lblAbsent.Text = ""
                pbCapturedImage.Image = Nothing
                pbImage.Image = Nothing

                ' Clear and refocus RFID input
                txtRFID.Clear()
                txtRFID.Focus()

                reader.Close()
                CloseConnection()
                Exit Sub
            End If
            reader.Close()

            ' Step 2: Fetch student image from tblStudent using LRN
            If Not String.IsNullOrWhiteSpace(lblLRN.Text) Then
                Dim queryStudent As String = "SELECT Image FROM tblStudent WHERE LRN = @lrn LIMIT 1"
                cmd = New MySqlCommand(queryStudent, conn)
                cmd.Parameters.AddWithValue("@lrn", lblLRN.Text)
                reader = cmd.ExecuteReader()

                If reader.Read() AndAlso Not IsDBNull(reader("Image")) Then
                    Dim imgData As Byte() = CType(reader("Image"), Byte())
                    Using ms As New MemoryStream(imgData)
                        pbImage.Image = Image.FromStream(ms)
                    End Using
                Else
                    pbImage.Image = Nothing
                    MsgBox("No image found for this student.", MsgBoxStyle.Information)
                End If
                reader.Close()
            Else
                pbImage.Image = Nothing
            End If

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            txtRFID.Clear()
            txtRFID.Focus()
            CloseConnection()
        End Try
    End Sub





    Private Sub btnMarkAbsent_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMarkAbsent.Click
        If String.IsNullOrWhiteSpace(txtRFID.Text) Then
            MsgBox("Please enter or scan RFID first.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        ' Confirmation before proceeding
        Dim confirmResult As DialogResult = MessageBox.Show("Are you sure you want to mark this student as absent?", "Confirm Absence", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmResult = DialogResult.No Then
            Exit Sub
        End If

        Try
            OpenConnection()

            ' Check if a DTR record exists for the student on the selected date
            Dim checkQuery As String = "SELECT AttendanceID FROM tblDTR WHERE RFID = @rfid AND DATE(Date) = @date LIMIT 1"
            cmd = New MySqlCommand(checkQuery, conn)
            cmd.Parameters.AddWithValue("@rfid", txtRFID.Text)
            cmd.Parameters.AddWithValue("@date", dtpDate.Value.ToString("yyyy-MM-dd"))

            Dim attendanceID As Object = cmd.ExecuteScalar()

            If attendanceID IsNot Nothing Then
                ' Record exists, update it
                Dim updateQuery As String = "UPDATE tblDTR SET Time = NULL, Late = '0', Absent = @date WHERE AttendanceID = @id"
                cmd = New MySqlCommand(updateQuery, conn)

                Dim absentDate As String = dtpDate.Value.ToString("yyyy-MM-dd")
                cmd.Parameters.AddWithValue("@date", absentDate)
                cmd.Parameters.AddWithValue("@id", attendanceID)

                Dim result As Integer = cmd.ExecuteNonQuery()

                If result > 0 Then
                    MsgBox("Student successfully marked as absent.", MsgBoxStyle.Information)
                    lblAbsent.Text = absentDate
                    lblTime.Text = ""
                    lblLate.Text = "0"
                Else
                    MsgBox("Failed to update absence.", MsgBoxStyle.Exclamation)
                End If
            Else
                MsgBox("No existing DTR record found for this student on the selected date.", MsgBoxStyle.Information)
            End If

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub




    Private Sub btnRFID_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRFID.MouseLeave

    End Sub
    Private Sub btnRFID_MouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRFID.MouseEnter

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

    Private Sub btnManageAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnManageAccount.Click
        Me.Hide()
        Account.Show()
    End Sub

    Private Sub AttendanceVerify_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pbImage.SizeMode = PictureBoxSizeMode.StretchImage
        pbCapturedImage.SizeMode = PictureBoxSizeMode.StretchImage
    End Sub

    Private Sub pbImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbImage.Click

    End Sub
End Class
