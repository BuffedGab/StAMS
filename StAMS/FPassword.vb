Imports System.Net.Mail
Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text
Public Class FPassword
    Dim conn As New MySqlConnection("server=localhost;user id=root;password=;database=attendance")
    Private Function ComputeSHA256Hash(ByVal input As String) As String
        Using sha256 As SHA256 = sha256.Create()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(input)
            Dim hashBytes As Byte() = sha256.ComputeHash(bytes)
            Return Convert.ToBase64String(hashBytes)
        End Using
    End Function
    Private Sub ClearFields()
        txtUsername.Clear()
        txtEmail.Clear()
        txtCode.Clear()
        txtNewPassword.Clear()
        txtConfirmPassword.Clear()
    End Sub
    Private Sub btnSendCode_Click(sender As Object, e As EventArgs) Handles btnSendCode.Click
        If txtUsername.Text.Trim() = "" Or txtEmail.Text.Trim() = "" Then
            MessageBox.Show("Please enter both username and email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            conn.Open()
            Dim checkCmd As New MySqlCommand("SELECT * FROM tblaccount WHERE Username=@username AND Email=@Email", conn)
            checkCmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim())
            checkCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim())

            Dim reader As MySqlDataReader = checkCmd.ExecuteReader()

            If reader.HasRows Then
                reader.Close()
                Dim rand As New Random()
                Dim code As String = rand.Next(100000, 999999).ToString()
                Dim upsertQuery As String = "INSERT INTO reset_codes (Username, Code, Expiration) " & _
                            "VALUES (@username, @code, NOW() + INTERVAL 10 MINUTE) " & _
                            "ON DUPLICATE KEY UPDATE Code = @code, Expiration = NOW() + INTERVAL 10 MINUTE"

                Dim upsertCmd As New MySqlCommand(upsertQuery, conn)
                upsertCmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim())
                upsertCmd.Parameters.AddWithValue("@code", code)
                upsertCmd.ExecuteNonQuery()

                Dim smtp As New SmtpClient("smtp.gmail.com", 587)
                smtp.EnableSsl = True
                smtp.Credentials = New Net.NetworkCredential("almertakeshi26@gmail.com", "iadbbktglgtyuqiv")

                Dim mail As New MailMessage()
                mail.From = New MailAddress("almertakeshi26@gmail.com")
                mail.To.Add(txtEmail.Text.Trim())
                mail.Subject = "Password Reset Code"
                mail.Body = "Your password reset code is: " & code

                smtp.Send(mail)

                MessageBox.Show("Code sent to your email successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Username and Email do not match any account.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show("Error sending code: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub
    Private Sub btnResetPassword_Click(sender As Object, e As EventArgs) Handles btnResetPassword.Click
        If txtUsername.Text.Trim() = "" OrElse
            txtCode.Text.Trim() = "" OrElse
            txtNewPassword.Text.Trim() = "" OrElse
            txtConfirmPassword.Text.Trim() = "" Then

            MessageBox.Show("All fields are required. Please fill in all the information.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If txtCode.Text.Trim() = "" Then
            MessageBox.Show("Please enter the verification code.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If txtNewPassword.Text <> txtConfirmPassword.Text Then
            MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            conn.Open()
            Dim validateCmd As New MySqlCommand("SELECT * FROM reset_codes WHERE Username=@username AND Code=@code AND Expiration > NOW()", conn)
            validateCmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim())
            validateCmd.Parameters.AddWithValue("@code", txtCode.Text.Trim())

            Dim reader As MySqlDataReader = validateCmd.ExecuteReader()

            If reader.HasRows Then
                reader.Close()
                Dim newHashedPassword As String = ComputeSHA256Hash(txtNewPassword.Text.Trim())
                Dim checkCmd As New MySqlCommand("SELECT Password FROM tblaccount WHERE Username=@username", conn)
                checkCmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim())
                Dim currentPassword As String = Convert.ToString(checkCmd.ExecuteScalar())

                If currentPassword = newHashedPassword Then
                    MessageBox.Show("New password cannot be the same as the old password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                Dim updateCmd As New MySqlCommand("UPDATE tblaccount SET Password=@password WHERE Username=@username", conn)
                updateCmd.Parameters.AddWithValue("@password", newHashedPassword)
                updateCmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim())
                updateCmd.ExecuteNonQuery()

                Dim deleteCmd As New MySqlCommand("DELETE FROM reset_codes WHERE Username=@username", conn)
                deleteCmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim())
                deleteCmd.ExecuteNonQuery()

                MessageBox.Show("Password successfully updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ClearFields()
            Else
                MessageBox.Show("Invalid or expired verification code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Error resetting password: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ClearFields()
        Login.Show()
        Me.Hide()
    End Sub

    Private Sub btnResetPassword_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetPassword.MouseEnter
        btnResetPassword.BackColor = Color.LightGreen
        btnResetPassword.ForeColor = Color.White
        btnResetPassword.FlatStyle = FlatStyle.Flat
        PictureBox2.BackColor = Color.LightGreen
    End Sub

    ' Reset the button appearance when mouse leaves
    Private Sub btnResetPassword_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetPassword.MouseLeave
        btnResetPassword.BackColor = Color.DarkGreen
        btnResetPassword.ForeColor = Color.White
        btnResetPassword.FlatStyle = FlatStyle.Standard
        PictureBox2.BackColor = Color.DarkGreen
    End Sub

    Private Sub btnCancel_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.MouseEnter
        btnCancel.BackColor = Color.Firebrick
        btnCancel.ForeColor = Color.White
        btnCancel.FlatStyle = FlatStyle.Flat
        PictureBox3.BackColor = Color.Firebrick
    End Sub

    ' Reset the button appearance when mouse leaves
    Private Sub btnCancel_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.MouseLeave
        btnCancel.BackColor = Color.DarkRed
        btnCancel.ForeColor = Color.White
        btnCancel.FlatStyle = FlatStyle.Standard
        PictureBox3.BackColor = Color.DarkRed
    End Sub

End Class
