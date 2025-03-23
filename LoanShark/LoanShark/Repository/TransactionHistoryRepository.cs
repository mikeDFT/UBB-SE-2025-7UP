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
using System.Diagnostics;

namespace LoanShark.Repository
{

    //repo class is static but it should be fine as long as the methods are static, you dont need to change anything 
    //about the functions
    public class TransactionHistoryRepository
    {

        public TransactionHistoryRepository() {}


        // this retrieves all the transactions from the database, creeates a hashmap
        // for each row and then creates a transaction object from the hashmap
        // that is what the transaction has a constructor that takes a hashmap
        public async Task<ObservableCollection<Transaction>> getTransactionsNormal()
        {
            ObservableCollection<Transaction> TransactionsNormal = new ObservableCollection<Transaction>();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Iban", UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty)
                };
                DataTable result = await DataLink.Instance.ExecuteReader("GetAllTransactionsByIban", parameters);
                foreach (DataRow row in result.Rows)
                {
                    Dictionary<string, object> hashMap = new Dictionary<string, object>();
                    foreach (DataColumn column in result.Columns)
                    {
                        hashMap.Add(column.ColumnName, row[column]);
                    }

                    Debug.WriteLine("hashMap: " + hashMap);

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
        public static async Task UpdateTransactionDescription(int transactionId, string newDescription)
        {
            try
            {
                string storedProcedure = "UpdateTransactionDescription";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@TransactionID", transactionId),
                    new SqlParameter("@NewDescription", newDescription)
                };
                await DataLink.Instance.ExecuteNonQuery(storedProcedure, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        //gets transactions formatted for the menu
        public async Task<ObservableCollection<string>> getTransactionsForMenu()
        {
            ObservableCollection<string> TransactionsForMenu = new ObservableCollection<string>();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Iban", UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty)
                };
                DataTable result = await DataLink.Instance.ExecuteReader("GetAllTransactionsByIban", parameters);
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
        public async Task<ObservableCollection<string>> getTransactionsDetailed()
        {
            ObservableCollection<string> TransactionsDetailed = new ObservableCollection<string>();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Iban", UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty)
                };
                DataTable result = await DataLink.Instance.ExecuteReader("GetAllTransactionsByIban", parameters);
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
