using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using LoanShark.Helper;
using Microsoft.Data.SqlClient;

namespace LoanShark.Data
{
    class DataLink
    {
        // Singleton instance
        private static DataLink? instance;
        private static readonly object lockObject = new object();
        private SqlConnection sqlConnection;
        private readonly string? connectionString;
        // Singleton accessor
        public static DataLink Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new DataLink();
                    }
                    return instance;
                }
            }
        }

        private DataLink()
        {
            // connectionString = AppConfig.GetConnectionString("MyLocalDb");
            connectionString = @"Data Source=DESKTOP-FEAUT17;Initial Catalog=loan_shark;Integrated Security=True;TrustServerCertificate=True";
            try
            {
                sqlConnection = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error connecting to the database: {ex.Message}");
            }
        }
        // The open and close connection are intentionally not async to avoid blocking the main thread
        public void OpenConnection()
        {
            if (sqlConnection.State != System.Data.ConnectionState.Open)
            {
                sqlConnection.Open();
                Debug.Print("Connection to the database is now open");
            }
            else
            {
                Debug.Print("Connection was already opened beforehand");
            }
        }

        public void CloseConnection()
        {
            if (sqlConnection.State != System.Data.ConnectionState.Closed)
            {
                sqlConnection.Close();
                Debug.Print("Connection to the database is now closed");
            }
            else
            {
                Debug.Print("Connection was already closed beforehand");
            }
        }
        // Executes a stored procedure and returns a single scalar value (e.g., COUNT(*), SUM(), MAX(), etc.)
        public async Task<T?> ExecuteScalar<T>(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (sqlParameters != null)
                    {
                        command.Parameters.AddRange(sqlParameters);
                    }

                    var result = await command.ExecuteScalarAsync();
                    if (result == DBNull.Value || result == null)
                    {
                        return default;
                    }

                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecutingScalar: {ex.Message}");
            }
        }


        // Executes a stored procedure and returns multiple rows and columns as a DataTable
        public async Task<DataTable> ExecuteReader(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (sqlParameters != null)
                    {
                        command.Parameters.AddRange(sqlParameters);
                    }

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteReader: {ex.Message}");
            }
        }
        // Executes a stored procedure that modifies data (INSERT, UPDATE, DELETE) and returns the number of affected rows
        public async Task<int> ExecuteNonQuery(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (sqlParameters != null)
                    {
                        command.Parameters.AddRange(sqlParameters);
                    }

                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteNonQuery: {ex.Message}");
            }
        }
    }
}
