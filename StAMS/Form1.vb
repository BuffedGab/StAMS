Imports Emgu.CV
Imports Emgu.CV.Structure
Imports System.IO

Public Class Form1
    Dim capture As Emgu.CV.Capture

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            capture = New Capture()
            txtRFID.Focus()
        Catch ex As Exception
            MessageBox.Show("Webcam error: " & ex.Message)
        End Try
    End Sub

    Private Sub txtRFID_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtRFID.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            Dim rfidCode As String = txtRFID.Text.Trim()
            If rfidCode <> "" Then
                CaptureImage(rfidCode)
                txtRFID.Clear()
            End If
        End If
    End Sub

    Private Sub CaptureImage(ByVal rfid As String)
        If capture IsNot Nothing Then
            Dim frame As Image(Of Bgr, Byte) = capture.QueryFrame()
            If frame IsNot Nothing Then
                PictureBox1.Image = frame.ToBitmap()
                Dim filePath As String = Path.Combine(Application.StartupPath, "Captured_" & rfid & "_" & Now.ToString("yyyyMMdd_HHmmss") & ".jpg")
                frame.Save(filePath)
                MessageBox.Show("Image saved: " & filePath)
            End If
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer1.Tick
        If capture IsNot Nothing Then
            Dim frame As Image(Of Bgr, Byte) = capture.QueryFrame()
            If frame IsNot Nothing Then
                PictureBox1.Image = frame.ToBitmap()
            End If
        End If
    End Sub
End Class
