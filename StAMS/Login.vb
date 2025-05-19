Imports MySql.Data.MySqlClient
Imports System.Security.Cryptography
Imports System.Text

Public Class Login

    Private Sub Login_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Load user roles into ComboBox
        cbUserType.Items.Add("Teacher")
        cbUserType.Items.Add("Class President")
        cbUserType.SelectedIndex = 0 ' default selection
    End Sub

    Private Function ComputeSHA256Hash(ByVal input As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(input)
            Dim hashBytes As Byte() = sha256.ComputeHash(bytes)
            Return Convert.ToBase64String(hashBytes) ' 🔁 Use Base64 instead of hex
        End Using
    End Function

    Private Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Dim connStr As String = "Server=localhost;Database=attendance;Uid=root;Pwd=;"
        Dim username As String = txtUsername.Text.Trim()
        Dim password As String = ComputeSHA256Hash(txtPassword.Text.Trim()) ' hash it before checking
        Dim role As String = cbUserType.Text.Trim()

        If username = "" Or password = "" Then
            MsgBox("Please enter both username and password.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Try
            Using conn As New MySqlConnection(connStr)
                conn.Open()

                Dim query As String = "SELECT * FROM tblAccount " &
                                      "WHERE LTRIM(RTRIM(Username)) = LTRIM(RTRIM(@Username)) " &
                                      "AND LTRIM(RTRIM(Password)) = LTRIM(RTRIM(@Password)) " &
                                      "AND LTRIM(RTRIM(Role)) = LTRIM(RTRIM(@Role))"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@Username", username)
                    cmd.Parameters.AddWithValue("@Password", password)
                    cmd.Parameters.AddWithValue("@Role", role)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.HasRows Then
                            MsgBox("Login successful!", MsgBoxStyle.Information)

                            If role = "Teacher" Then
                                Me.Hide()
                                Dashboard.Show()
                            ElseIf role = "Class President" Then
                                Me.Hide()
                                Attendance.Show()
                            End If
                        Else
                            MsgBox("Invalid credentials.", MsgBoxStyle.Critical)
                            txtPassword.Clear() ' 🔒 Clear the password textbox on failed login
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Login failed: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub


    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Application.Exit()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        If keyData = Keys.Enter Then
            btnLogin.PerformClick()
            Return True
        ElseIf keyData = Keys.Escape Then
            btnCancel.PerformClick()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        FPassword.Show()
        Me.Hide()
    End Sub

    Private Sub btnLogin_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.MouseEnter
        btnLogin.BackColor = color.LightGreen
        btnLogin.ForeColor = Color.White
        btnLogin.FlatStyle = FlatStyle.Flat
        PictureBox2.BackColor = Color.LightGreen
    End Sub

    ' Reset the button appearance when mouse leaves
    Private Sub btnLogin_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.MouseLeave
        btnLogin.BackColor = Color.DarkGreen
        btnLogin.ForeColor = Color.White
        btnLogin.FlatStyle = FlatStyle.Standard
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
