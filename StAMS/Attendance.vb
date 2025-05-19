Imports MySql.Data.MySqlClient
Imports System.IO
Imports Emgu.CV
Imports Emgu.CV.Structure
Imports Emgu.CV.HaarCascade
Imports Emgu.CV.CvEnum


Public Class Attendance
    Private conn As MySqlConnection
    Private cmd As MySqlCommand
    Private adapter As MySqlDataAdapter
    Private reader As MySqlDataReader
    Dim capture As Emgu.CV.Capture



    Private Sub Attendance_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Timer1.Start()
        Timer2.Start()
        Timer3.Start()
        txtRFID.Focus()
        txtRFID.Select()

        LoadDefaultAcademicYear()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer1.Tick
        lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt")
    End Sub

    Private Sub Timer2_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer2.Tick
        lblDate.Text = DateTime.Now.ToString("MMMM dd, yyyy")
    End Sub

    Private Sub OpenConnection()
        Try
            If conn Is Nothing OrElse conn.State = ConnectionState.Closed Then
                conn = New MySqlConnection("Server=127.0.0.1;Database=attendance;Uid=root;Pwd=;")
                conn.Open()
            End If
        Catch ex As Exception
            MsgBox("Connection Failed: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub CloseConnection()
        If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
            conn.Close()
        End If
    End Sub

    Private Sub txtRFID_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtRFID.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtRFID.Text.Trim() <> "" Then
                Dim rfidInput As String = txtRFID.Text.Trim()
                ProcessAttendance(rfidInput)
                txtRFID.Clear()
            End If
        End If
    End Sub
    Private Sub ClearStudentDisplay()
        lblStudentID.Text = ""
        lblLRN.Text = ""
        lblFullName.Text = ""
        lblYrLevel.Text = ""
        lblSection.Text = ""
        lblNumber.Text = ""
        pbImage.Image = Nothing
        pbCapture.Image = Nothing
    End Sub
    Private Sub ProcessAttendance(ByVal rfid As String)
        Try
            OpenConnection()

            Dim query As String = "SELECT StudentID, FullName, YearLevel, Section, Contact, LRN, Image FROM tblStudent WHERE RFID = @RFID"
            cmd = New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@RFID", rfid)

            Dim studentFound As Boolean = False
            Dim studentID, fullName, yearLevel, section, contact, lrn As String
            Dim imgData As Byte() = Nothing

            Using reader As MySqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    studentFound = True
                    studentID = reader("StudentID").ToString()
                    fullName = reader("FullName").ToString()
                    yearLevel = reader("YearLevel").ToString().Trim().ToLower()
                    section = reader("Section").ToString()
                    contact = reader("Contact").ToString()
                    lrn = reader("LRN").ToString()

                    lblStudentID.Text = studentID
                    lblLRN.Text = lrn
                    lblFullName.Text = fullName
                    lblYrLevel.Text = reader("YearLevel").ToString()
                    lblSection.Text = section
                    lblNumber.Text = contact

                    If Not IsDBNull(reader("Image")) Then
                        imgData = DirectCast(reader("Image"), Byte())
                        pbImage.Image = ByteArrayToImage(imgData)
                        pbImage.SizeMode = PictureBoxSizeMode.StretchImage
                    Else
                        pbImage.Image = Nothing
                    End If
                End If
            End Using

            If Not studentFound Then
                MsgBox("Student not found!", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            'magproproceed lang once na magclose na yung reader
            Dim currentTime As DateTime = DateTime.Now
            Dim current As TimeSpan = currentTime.TimeOfDay

            If yearLevel = "grade 12" Then
                Dim allowedStart As TimeSpan = TimeSpan.Parse("06:00:00")
                Dim allowedEnd As TimeSpan = TimeSpan.Parse("08:00:00")
                Dim endedStart As TimeSpan = TimeSpan.Parse("12:30:00")
                Dim endedEnd As TimeSpan = TimeSpan.Parse("19:00:00")

                If current >= endedStart AndAlso current <= endedEnd Then
                    MsgBox("Grade 12 class already ended!", MsgBoxStyle.Exclamation)
                    txtRFID.Clear()
                    TimerClear.Stop()
                    TimerClear.Start()
                    Exit Sub
                End If

                If current < allowedStart OrElse current > allowedEnd Then
                    MsgBox("Grade 12 students can only tap between 6:00 AM and 8:00 AM! Attendance Considered Absent!", MsgBoxStyle.Exclamation)
                    RecordAbsence(rfid, lrn, fullName, yearLevel, section, contact)
                    txtRFID.Clear()
                    TimerClear.Stop()
                    TimerClear.Start()
                    Exit Sub
                End If
            End If

            If yearLevel = "grade 11" Then
                Dim allowedStart As TimeSpan = TimeSpan.Parse("12:30:00")
                Dim allowedEnd As TimeSpan = TimeSpan.Parse("15:00:00")

                If current < allowedStart Then
                    MsgBox("Grade 11 class hasn't started yet!", MsgBoxStyle.Exclamation)
                    txtRFID.Clear()
                    TimerClear.Stop()
                    TimerClear.Start()
                    Exit Sub
                End If

                If current > allowedEnd Then
                    MsgBox("Grade 11 students can only tap between 12:30 PM and 3:00 PM! Attendance Considered Absent", MsgBoxStyle.Exclamation)
                    RecordAbsence(rfid, lrn, fullName, yearLevel, section, contact)
                    txtRFID.Clear()
                    TimerClear.Stop()
                    TimerClear.Start()
                    Exit Sub
                End If
            End If

            ' Safe to check after reader is closed
            If AttendanceExists(lrn) Then
                MsgBox("Attendance already recorded today!", MsgBoxStyle.Information)
                Exit Sub
            End If

            Dim capturedImage As Byte() = CaptureImage()
            If capturedImage IsNot Nothing Then
                pbCapture.Image = ByteArrayToImage(capturedImage)
                pbCapture.SizeMode = PictureBoxSizeMode.StretchImage
            Else
                pbCapture.Image = Nothing
            End If

            RecordAttendance(rfid, lrn, fullName, yearLevel, section, contact, capturedImage)
            TimerClear.Stop()
            TimerClear.Start()

        Catch ex As Exception
            MsgBox("Error processing attendance: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub





    Private Function AttendanceExists(ByVal studentID As String) As Boolean
        Dim exists As Boolean = False
        Try
            OpenConnection()

            If reader IsNot Nothing AndAlso Not reader.IsClosed Then
                reader.Close()
            End If

            Dim query As String = "SELECT COUNT(*) FROM tblDTR WHERE LRN = @StudentID AND Date = @Date"
            Using checkCmd As New MySqlCommand(query, conn)
                checkCmd.Parameters.AddWithValue("@StudentID", studentID)
                checkCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"))

                exists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0
            End Using

        Catch ex As Exception
            MsgBox("Error checking attendance: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
        Return exists
    End Function

    Private Sub RecordAttendance(ByVal rfid As String, ByVal studentID As String, ByVal fullName As String,
                             ByVal yearLevel As String, ByVal section As String, ByVal contact As String,
                             ByVal photo As Byte())

        Try
            Dim currentTime As DateTime = DateTime.Now
            Dim lateTime As Object = DBNull.Value

            If currentTime.TimeOfDay > TimeSpan.Parse("06:30:00") AndAlso currentTime.TimeOfDay <= TimeSpan.Parse("13:00:00") Then
                lateTime = currentTime.ToString("HH:mm:ss")
            End If

            OpenConnection()

            Dim query As String = "INSERT INTO tblDTR (RFID, LRN, FullName, YearLevel, Section, Contact, Time, Date, Late, CapturedImage) " &
                                  "VALUES (@RFID, @LRN, @FullName, @YearLevel, @Section, @Contact, @Time, @Date, @Late, @CapturedImage)"

            cmd = New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@RFID", rfid)
            cmd.Parameters.AddWithValue("@LRN", studentID)
            cmd.Parameters.AddWithValue("@FullName", fullName)
            cmd.Parameters.AddWithValue("@YearLevel", yearLevel)
            cmd.Parameters.AddWithValue("@Section", section)
            cmd.Parameters.AddWithValue("@Contact", contact)
            cmd.Parameters.AddWithValue("@Time", currentTime.ToString("HH:mm:ss"))
            cmd.Parameters.AddWithValue("@Date", currentTime.ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@Late", If(lateTime Is DBNull.Value, DBNull.Value, lateTime))
            cmd.Parameters.AddWithValue("@CapturedImage", If(photo IsNot Nothing, photo, DBNull.Value))

            cmd.ExecuteNonQuery()

            MsgBox("Attendance Recorded Successfully!", MsgBoxStyle.Information)

        Catch ex As Exception
            MsgBox("Error recording attendance: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub


    Private Function ByteArrayToImage(ByVal data As Byte()) As Image
        Using ms As New MemoryStream(data)
            Return Image.FromStream(ms)
        End Using
    End Function

    Private Sub RecordAbsence(ByVal rfid As String, ByVal studentID As String, ByVal fullName As String, ByVal yearLevel As String, ByVal section As String, ByVal contact As String)
        Try
            Dim morningCutoff As DateTime = DateTime.Parse("06:30:00 AM")
            Dim afternoonCutoff As DateTime = DateTime.Parse("01:00:00 PM")
            Dim currentTime As DateTime = DateTime.Now

            Dim lateTime As Object = DBNull.Value

            OpenConnection()

            Dim query As String = "INSERT INTO tblDTR (RFID, LRN, FullName, YearLevel, Section, Contact, Time, Date, Late, Absent) " & _
                                  "VALUES (@RFID, @LRN, @FullName, @YearLevel, @Section, @Contact, @Time, @Date, @Late, @Absent)"
            cmd = New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@RFID", rfid)
            cmd.Parameters.AddWithValue("@LRN", studentID)
            cmd.Parameters.AddWithValue("@FullName", fullName)
            cmd.Parameters.AddWithValue("@YearLevel", yearLevel)
            cmd.Parameters.AddWithValue("@Section", section)
            cmd.Parameters.AddWithValue("@Contact", contact)
            cmd.Parameters.AddWithValue("@Time", DBNull.Value)
            cmd.Parameters.AddWithValue("@Date", currentTime.ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@Late", DBNull.Value)
            cmd.Parameters.AddWithValue("@Absent", currentTime.ToString("yyyy-MM-dd"))

            cmd.ExecuteNonQuery()

            'MsgBox("Absence Recorded Successfully!", MsgBoxStyle.Information)'

        Catch ex As Exception
            MsgBox("Error recording absence: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
    End Sub


    Private Sub CheckForAbsence(ByVal yearLevelFilter As String)
        Dim tempConn As New MySqlConnection("Server=127.0.0.1;Database=attendance;Uid=root;Pwd=;")
        Dim absentCount As Integer = 0
        Dim totalStudents As Integer = 0

        Try
            tempConn.Open()

            ' Count total students for the given year level
            Dim countQuery As String = "SELECT COUNT(*) FROM tblStudent WHERE LOWER(YearLevel) = @YearLevel"
            Using countCmd As New MySqlCommand(countQuery, tempConn)
                countCmd.Parameters.AddWithValue("@YearLevel", yearLevelFilter.ToLower())
                totalStudents = Convert.ToInt32(countCmd.ExecuteScalar())
            End Using

            ' Get all students for the given year level
            Dim query As String = "SELECT StudentID, FullName, YearLevel, Section, Contact FROM tblStudent WHERE LOWER(YearLevel) = @YearLevel"
            Using cmd As New MySqlCommand(query, tempConn)
                cmd.Parameters.AddWithValue("@YearLevel", yearLevelFilter.ToLower())

                Using reader As MySqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim studentLRN As String = GetStudentLRNByID(reader("StudentID").ToString())
                        Dim fullName As String = reader("FullName").ToString()
                        Dim yearLevel As String = reader("YearLevel").ToString()
                        Dim section As String = reader("Section").ToString()
                        Dim contact As String = reader("Contact").ToString()

                        If Not AttendanceExists(studentLRN) Then
                            RecordAbsence("", studentLRN, fullName, yearLevel, section, contact)
                            absentCount += 1
                        End If
                    End While
                End Using
            End Using

            Dim presentCount As Integer = totalStudents - absentCount
            Dim message As String = String.Format("Attendance Check Completed for {0}!" & vbCrLf &
                                           "Total Students: {1}" & vbCrLf &
                                           "Absent Students: {2}" & vbCrLf &
                                           "Present Students: {3}",
                                           yearLevelFilter, totalStudents, absentCount, presentCount)
            MsgBox(message, MsgBoxStyle.Information)


        Catch ex As Exception
            MsgBox("Error checking for absence: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            tempConn.Close()
        End Try
    End Sub


    Private Function HasAttendanceInTimeRange(ByVal studentID As String) As Boolean
        Dim exists As Boolean = False
        Try
            OpenConnection()

            Dim query As String = "SELECT COUNT(*) FROM tblDTR WHERE LRN = @StudentID AND Date = @Date " &
                                  "AND TIME(Time) BETWEEN '06:30:00' AND '12:30:00'"
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@StudentID", studentID)
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"))

                exists = Convert.ToInt32(cmd.ExecuteScalar()) > 0
            End Using

        Catch ex As Exception
            MsgBox("Error checking time-based attendance: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            CloseConnection()
        End Try
        Return exists
    End Function






    Private lastCheckedDate As DateTime? = Nothing
    Private checkStartTime As DateTime? = Nothing ' Track when checking started

    Private lastCheckedDateG12 As DateTime? = Nothing
    Private lastCheckedDateG11 As DateTime? = Nothing

    Private Sub Timer3_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer3.Tick
        Dim now As DateTime = DateTime.Now

        ' Check Grade 12 at 12:30 PM
        If now.Hour = 12 AndAlso now.Minute = 30 AndAlso (Not lastCheckedDateG12.HasValue OrElse lastCheckedDateG12.Value.Date <> now.Date) Then
            If Not checkStartTime.HasValue Then
                checkStartTime = now
                lastCheckedDateG12 = now
                MsgBox("Checking Grade 12 Attendance...", MsgBoxStyle.Information)
                CheckForAbsence("grade 12")
            End If
        End If

        ' Check Grade 11 at 7:00 PM
        If now.Hour = 19 AndAlso now.Minute = 0 AndAlso (Not lastCheckedDateG11.HasValue OrElse lastCheckedDateG11.Value.Date <> now.Date) Then
            If checkStartTime.HasValue AndAlso (now - checkStartTime.Value).TotalSeconds >= 10 Then
                Timer3.Stop()
                lastCheckedDateG11 = now
                MsgBox("Checking Grade 11 Attendance...", MsgBoxStyle.Information)
                CheckForAbsence("grade 11")
            End If
        End If

        ' Stop timer after 10 seconds
        If checkStartTime.HasValue AndAlso (now - checkStartTime.Value).TotalSeconds >= 10 Then
            Timer3.Stop()
            checkStartTime = Nothing
        End If
    End Sub




    Public Property AcademicYear As String
        Get
            Return lblAcadYear.Text
        End Get
        Set(ByVal value As String)
            lblAcadYear.Text = value
        End Set
    End Property

    Private Sub LoadDefaultAcademicYear()
        Try
            Dim connStr As String = "Server=127.0.0.1;Database=attendance;Uid=root;Pwd=;"
            Using conn As New MySqlConnection(connStr)
                conn.Open()

                Dim query As String = "SELECT Year FROM tblAcademic WHERE IsDefault = 1"
                Using cmd As New MySqlCommand(query, conn)
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        lblAcadYear.Text = result.ToString()
                    Else
                        lblAcadYear.Text = "No default year set"
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error loading academic year: " & ex.Message)
        End Try
    End Sub

    Private Sub txtRFID_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtRFID.KeyPress
        ' Allow only digits and control keys like Backspace
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If

        ' Limit to 12 characters max
        If Char.IsDigit(e.KeyChar) AndAlso txtRFID.TextLength >= 12 Then
            e.Handled = True
        End If

        If txtRFID.TextLength > 12 Then
            txtRFID.Text = txtRFID.Text.Substring(0, 12)
            txtRFID.SelectionStart = txtRFID.TextLength ' Keep cursor at the end
        End If
    End Sub

    'Try capture image pare'
    Private Function CaptureImage() As Byte()
        Try
            If capture Is Nothing Then
                capture = New Emgu.CV.Capture(0) ' 0 = default camera
            End If

            ' Get frame from capture
            Dim frame As Emgu.CV.Image(Of Emgu.CV.Structure.Bgr, Byte) = capture.QueryFrame()

            If frame IsNot Nothing Then
                Using ms As New MemoryStream()
                    frame.ToBitmap().Save(ms, Imaging.ImageFormat.Jpeg)
                    Return ms.ToArray()
                End Using
            End If
        Catch ex As Exception
            MsgBox("Error capturing image: " & ex.Message, MsgBoxStyle.Critical)
        End Try

        Return Nothing
    End Function

    Public Sub DetectFace(ByVal image As Bitmap)
        Try
            ' Convert the input Bitmap to a grayscale image
            Dim grayImage As New Image(Of Gray, Byte)(image)

            ' Load the HaarCascade XML file
            Dim faceCascade As New HaarCascade(Application.StartupPath & "\haarcascade_frontalface_default.xml")

            ' Detect faces
            Dim facesDetected As MCvAvgComp()() = grayImage.DetectHaarCascade(
                faceCascade,
                1.1,
                10,
                HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                New Size(20, 20)
            )

            ' Draw rectangles around detected faces
            For Each face In facesDetected(0)
                grayImage.Draw(face.rect, New Gray(255), 2)
            Next

            ' Show the result in PictureBox
            pbCapture.Image = grayImage.ToBitmap()

        Catch ex As Exception
            MsgBox("Error in face detection: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Function GetStudentLRNByID(ByVal studentID As String) As String
        Try
            OpenConnection()
            Dim query As String = "SELECT LRN FROM tblStudent WHERE StudentID = @StudentID"
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@StudentID", studentID)
                Return Convert.ToString(cmd.ExecuteScalar())
            End Using
        Catch ex As Exception
            MsgBox("Error getting LRN: " & ex.Message)
            Return ""
        Finally
            CloseConnection()
        End Try
    End Function

    Private Sub btnLogout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogout.Click
        Me.Hide()
        Login.Show()
    End Sub

    Private Sub TimerClear_Tick(sender As System.Object, e As System.EventArgs) Handles TimerClear.Tick
        TimerClear.Stop()
        ClearStudentDisplay()
    End Sub
End Class