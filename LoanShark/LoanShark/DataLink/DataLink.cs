using System;
using System.Data;
using System.Diagnostics;
using LoanShark.Helper;
using Microsoft.Data.SqlClient;

namespace LoanShark.DataLink
{
    class DataLink
    {
        private SqlConnection sqlConnection;
        private readonly string connectionString;

        public DataLink() {
            connectionString = AppConfig.GetConnectionString("MyLocalDb");

            try {
                sqlConnection = new SqlConnection(connectionString);
            }
            catch (Exception ex) {
                throw new Exception($"Error connecting to the database: {ex.Message}");
            }
        }

        public void OpenConnection() {
            if (sqlConnection.State != System.Data.ConnectionState.Open) {
                sqlConnection.Open();
                Debug.Print("Connection to the database is now open");
            }
            else
            {
                Debug.Print("Connection was already opened beforehand");
            }
        }

        public void CloseConnection() {
            if (sqlConnection.State != System.Data.ConnectionState.Closed) {
                sqlConnection.Close();
                Debug.Print("Connection to the database is now closed");
            }
            else
            {
                Debug.Print("Connection was already closed beforehand");
            }
        }


        // Executes a stored procedure and returns a single scalar value (e.g., COUNT(*), SUM(), MAX(), etc.)
        public T? ExecuteScalar<T>(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(storedProcedure, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (sqlParameters != null)
                    {
                        command.Parameters.AddRange(sqlParameters);
                    }

                    var result = command.ExecuteScalar();
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
            finally
            {
                CloseConnection();
            }
        }


        // Executes a stored procedure and returns multiple rows and columns as a DataTable
        public DataTable ExecuteReader(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(storedProcedure, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (sqlParameters != null)
                    {
                        command.Parameters.AddRange(sqlParameters);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteReader: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }


        // Executes a stored procedure that modifies data (INSERT, UPDATE, DELETE) and returns the number of affected rows
        public int ExecuteNonQuery(string storedProcedure, SqlParameter[]? sqlParameters = null)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(storedProcedure, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (sqlParameters != null)
                    {
                        command.Parameters.AddRange(sqlParameters);
                    }

                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error - ExecuteNonQuery: {ex.Message}");
            }
            finally
            {
                CloseConnection();
            }
        }

    }
}
