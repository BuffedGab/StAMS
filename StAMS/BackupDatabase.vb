Imports System.Diagnostics
Imports System.IO
Imports System.Threading

Public Class BackupDatabase

    Private Sub btnBackup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackup.Click
        ' Run backup on a new thread to keep UI responsive
        Dim t As New Thread(AddressOf BackupDatabase)
        t.IsBackground = True
        t.Start()
    End Sub

    Private Sub BackupDatabase()
        Me.Invoke(Sub()
                      btnBackup.Enabled = False
                      ProgressBar1.Value = 0
                      lblBackupSize.Text = "Backing up... 0 KB"
                  End Sub)

        Dim sw As New Stopwatch()
        sw.Start()

        Dim backupFolder As String = "C:\backup"
        Dim databaseName As String = "attendance"
        Dim backupFile As String = Path.Combine(backupFolder, databaseName & "_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".sql")

        If Not Directory.Exists(backupFolder) Then
            Directory.CreateDirectory(backupFolder)
        End If

        Dim mysqldumpPath As String = "C:\xampp\mysql\bin\mysqldump.exe"
        Dim username As String = "root"
        Dim password As String = "" ' No password

        Try
            Dim args As String
            If String.IsNullOrEmpty(password) Then
                args = String.Format("-u {0} {1}", username, databaseName)
            Else
                args = String.Format("-u {0} -p{1} {2}", username, password, databaseName)
            End If

            Dim process As New Process()
            process.StartInfo.FileName = mysqldumpPath
            process.StartInfo.Arguments = args
            process.StartInfo.RedirectStandardOutput = True
            process.StartInfo.RedirectStandardError = True
            process.StartInfo.UseShellExecute = False
            process.StartInfo.CreateNoWindow = True

            process.Start()

            Dim output As String = process.StandardOutput.ReadToEnd()
            Dim errorOutput As String = process.StandardError.ReadToEnd()

            process.WaitForExit()

            ' Write backup file
            File.WriteAllText(backupFile, output)

            sw.Stop()

            If Not String.IsNullOrEmpty(errorOutput) Then
                Me.Invoke(Sub()
                              MessageBox.Show("❌ Error during backup: " & errorOutput, "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                          End Sub)
            Else
                Me.Invoke(Sub()
                              ProgressBar1.Value = 100
                              lblBackupSize.Text = "Backup Complete!"
                              MessageBox.Show("✅ Backup successful!" & vbCrLf &
                                              "📁 Saved to: " & backupFile & vbCrLf &
                                              "⏱ Time elapsed: " & sw.Elapsed.TotalSeconds.ToString("0.00") & " seconds",
                                              "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                          End Sub)
            End If
        Catch ex As Exception
            Me.Invoke(Sub()
                          MessageBox.Show("❌ Backup failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                      End Sub)
        Finally
            Me.Invoke(Sub()
                          btnBackup.Enabled = True
                          ProgressBar1.Value = 0
                      End Sub)
        End Try
    End Sub

    Private Sub btnBackup_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles btnBackup.MouseEnter
        btnBackup.BackColor = Color.LightGreen
        btnBackup.ForeColor = Color.White
        btnBackup.FlatStyle = FlatStyle.Flat

    End Sub

    ' Reset the button appearance when mouse leaves
    Private Sub btnAdd_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles btnBackup.MouseLeave
        btnBackup.BackColor = Color.DarkGreen
        btnBackup.ForeColor = Color.White
        btnBackup.FlatStyle = FlatStyle.Standard

    End Sub

    Private Sub btnBack_MouseEnter(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.MouseEnter
        btnBack.BackColor = Color.Firebrick
        btnBack.ForeColor = Color.White
        btnBack.FlatStyle = FlatStyle.Flat

    End Sub

    ' Reset the button appearance when mouse leaves
    Private Sub btnBack_MouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.MouseLeave
        btnBack.BackColor = Color.DarkRed
        btnBack.ForeColor = Color.White
        btnBack.FlatStyle = FlatStyle.Standard

    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Me.Hide()
        Report.Show()
    End Sub
End Class
