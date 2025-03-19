using LoanShark.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanShark.Domain;

namespace LoanShark.Repository
{
    static class Repo
    {

        public static ObservableCollection<Transaction> getTransactionsNormal()
        {
            ObservableCollection<Transaction> TransactionsNormal = new ObservableCollection<Transaction>();
            try
            {
                // Create an instance of the DataLink class
                DataLink dataLink = new DataLink();

                // Define the stored procedure name
                string storedProcedure = "GetTransactions";

                // Call the ExecuteReader method
                DataTable result = dataLink.ExecuteReader(storedProcedure);

                // Process the result
                foreach (DataRow row in result.Rows)
                {
                    Dictionary<string, object> hashMap = new Dictionary<string, object>();
                    foreach (DataColumn column in result.Columns)
                    {
                        hashMap.Add(column.ColumnName, row[column]);
                    }

                    Transaction transaction = new Transaction(hashMap);
                    TransactionsNormal.Add(transaction);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return TransactionsNormal;
        }

        public static ObservableCollection<string> getTransactionsForMenu()
        {

            ObservableCollection<string> TransactionsForMenu = new ObservableCollection<string>();
            try
            {
                // Create an instance of the DataLink class
                DataLink dataLink = new DataLink();

                // Define the stored procedure name
                string storedProcedure = "GetTransactions";

                // Call the ExecuteReader method
                DataTable result = dataLink.ExecuteReader(storedProcedure);

                // Process the result
                foreach (DataRow row in result.Rows)
                {
                    Dictionary<string, object> hashMap = new Dictionary<string, object>();
                    foreach (DataColumn column in result.Columns)
                    {
                        hashMap.Add(column.ColumnName, row[column]);
                    }

                    Transaction transaction = new Transaction(hashMap);
                    TransactionsForMenu.Add(transaction.tostringForMenu());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return TransactionsForMenu;
        }

         public static ObservableCollection<string> getTransactionsDetailed()
        {

            ObservableCollection<string> TransactionsDetailed = new ObservableCollection<string>();
            try
            {
                // Create an instance of the DataLink class
                DataLink dataLink = new DataLink();

                // Define the stored procedure name
                string storedProcedure = "GetTransactions";

                // Call the ExecuteReader method
                DataTable result = dataLink.ExecuteReader(storedProcedure);

                // Process the result
                foreach (DataRow row in result.Rows)
                {
                    Dictionary<string, object> hashMap = new Dictionary<string, object>();
                    foreach (DataColumn column in result.Columns)
                    {
                        hashMap.Add(column.ColumnName, row[column]);
                    }

                    Transaction transaction = new Transaction(hashMap);
                    TransactionsDetailed.Add(transaction.tostringDetailed());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return TransactionsDetailed;
        }
    }
}
