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

    //repo class is static but it should be fine as long as the methods are static, you dont need to change anything 
    //about the functions
    static class Repo
    {

        // this retrieves all the transactions from the database, creeates a hashmap
        // for each row and then creates a transaction object from the hashmap
        // that is what the transaction has a constructor that takes a hashmap
        public static ObservableCollection<Transaction> getTransactionsNormal()
        {
            ObservableCollection<Transaction> TransactionsNormal = new ObservableCollection<Transaction>();
            try
            {
                DataLink dataLink = new DataLink();
                string storedProcedure = "GetTransactions";
                DataTable result = dataLink.ExecuteReader(storedProcedure);
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


        //this is ok, no need to change anything
        public static void UpdateTransactionDescription(int transactionId, string newDescription)
        {
            try
            {
                DataLink dataLink = new DataLink();
                string storedProcedure = "UpdateTransactionDescription";
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@TransactionID", transactionId),
                new SqlParameter("@NewDescription", newDescription)
                };
                dataLink.ExecuteNonQuery(storedProcedure, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        //gets transactions formatted for the menu
        public static ObservableCollection<string> getTransactionsForMenu()
        {
            ObservableCollection<string> TransactionsForMenu = new ObservableCollection<string>();
            try
            {
                DataLink dataLink = new DataLink();
                string storedProcedure = "GetTransactions";
                DataTable result = dataLink.ExecuteReader(storedProcedure);
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

        //gets transactions formatted for the detailed view
        public static ObservableCollection<string> getTransactionsDetailed()
        {

            ObservableCollection<string> TransactionsDetailed = new ObservableCollection<string>();
            try
            {
                DataLink dataLink = new DataLink();
                string storedProcedure = "GetTransactions";
                DataTable result = dataLink.ExecuteReader(storedProcedure);
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
