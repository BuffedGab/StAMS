Imports MySql.Data.MySqlClient

Module DatabaseModule
    ' Connection string (Modify based on your MySQL database settings in XAMPP)
    Public conn As MySqlConnection
    Public cmd As MySqlCommand
    Public adapter As MySqlDataAdapter
    Public dt As DataTable

    ' Function to establish a database connection
    Public Sub OpenConnection()
        Try
            ' Change this connection string to match your XAMPP MySQL settings
            conn = New MySqlConnection("Server=localhost;Database=attendance;Uid=root;Pwd=;")
            conn.Open()
            Console.WriteLine("Database Connected Successfully")
        Catch ex As Exception
            Console.WriteLine("Connection Error: " & ex.Message)
        End Try
    End Sub

    ' Function to close the database connection
    Public Sub CloseConnection()
        Try
            If conn IsNot Nothing AndAlso conn.State = ConnectionState.Open Then
                conn.Close()
                Console.WriteLine("Database Connection Closed")
            End If
        Catch ex As Exception
            Console.WriteLine("Error Closing Connection: " & ex.Message)
        End Try
    End Sub

    ' Function to execute a query (INSERT, UPDATE, DELETE)
    Public Sub ExecuteQuery(ByVal query As String)
        Try
            OpenConnection()
            cmd = New MySqlCommand(query, conn)
            cmd.ExecuteNonQuery()
            Console.WriteLine("Query Executed Successfully")
        Catch ex As Exception
            Console.WriteLine("Query Execution Error: " & ex.Message)
        Finally
            CloseConnection()
        End Try
    End Sub

    ' Function to retrieve data (SELECT)
    Public Function GetData(ByVal query As String) As DataTable
        dt = New DataTable()
        Try
            OpenConnection()
            adapter = New MySqlDataAdapter(query, conn)
            adapter.Fill(dt)
        Catch ex As Exception
            Console.WriteLine("Data Retrieval Error: " & ex.Message)
        Finally
            CloseConnection()
        End Try
        Return dt
    End Function
End Module
