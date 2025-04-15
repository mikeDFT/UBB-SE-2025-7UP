using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using LoanShark.Data;
using LoanShark.Domain;
using Microsoft.Data.SqlClient;

namespace LoanShark.Repository
{
    /// <summary>
    /// Interface for bank account data access operations
    /// </summary>
    public interface IBankAccountRepository
    {
        /// <summary>
        /// Retrieves all bank accounts from the database
        /// </summary>
        /// <returns>A list of all bank accounts</returns>
        Task<List<BankAccount>?> GetAllBankAccounts();

        /// <summary>
        /// Retrieves all bank accounts for a specific user
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <returns>A list of bank accounts belonging to the user</returns>
        Task<List<BankAccount>?> GetBankAccountsByUserId(int userID);

        /// <summary>
        /// Retrieves a bank account by its IBAN
        /// </summary>
        /// <param name="iban">The IBAN of the bank account to retrieve</param>
        /// <returns>The bank account with the specified IBAN, or null if not found</returns>
        Task<BankAccount?> GetBankAccountByIBAN(string iban);

        /// <summary>
        /// Adds a new bank account to the database
        /// </summary>
        /// <param name="bankAccount">The bank account to add</param>
        /// <returns>True if the bank account was added successfully, false otherwise</returns>
        Task<bool> AddBankAccount(BankAccount bankAccount);

        /// <summary>
        /// Removes a bank account from the database
        /// </summary>
        /// <param name="iban">The IBAN of the bank account to remove</param>
        /// <returns>True if the bank account was removed successfully, false otherwise</returns>
        Task<bool> RemoveBankAccount(string iban);

        /// <summary>
        /// Updates a bank account in the database
        /// </summary>
        /// <param name="iban">The IBAN of the bank account to update</param>
        /// <param name="nba">The updated bank account information</param>
        /// <returns>True if the bank account was updated successfully, false otherwise</returns>
        Task<bool> UpdateBankAccount(string iban, BankAccount nba);

        /// <summary>
        /// Retrieves all available currencies from the database
        /// </summary>
        /// <returns>A list of currency names as strings</returns>
        Task<List<string>> GetCurrencies();

        /// <summary>
        /// Retrieves user credentials for the specified email
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>A list containing the user's hashed password and salt</returns>
        Task<List<string>> GetCredentials(string email);
    }

    /// <summary>
    /// Repository class for bank account data access
    /// </summary>
    public class BankAccountRepository : IBankAccountRepository
    {
        /// <summary>
        /// Initializes a new instance of the BankAccountRepository class
        /// </summary>
        public BankAccountRepository()
        {
        }

        /// <summary>
        /// Retrieves all bank accounts from the database
        /// </summary>
        /// <returns>A list of all bank accounts, or null if an error occurs</returns>
        public async Task<List<BankAccount>?> GetAllBankAccounts()
        {
            try
            {
                DataTable dataTable = await DataLink.Instance.ExecuteReader("GetAllBankAccounts");
                return await ConvertDataTableToBankAccountList(dataTable);
            }
            catch (Exception)
            {
                Debug.WriteLine("Oopsie on get all bank accounts repo");
                return null;
            }
        }

        /// <summary>
        /// Retrieves all bank accounts for a specific user
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <returns>A list of bank accounts belonging to the user, or null if an error occurs</returns>
        public async Task<List<BankAccount>?> GetBankAccountsByUserId(int userID)
        {
            try
            {
                var sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@id_user", userID)
                };
                DataTable dataTable = await DataLink.Instance.ExecuteReader("GetBankAccountsByUser", sqlParams);
                return await ConvertDataTableToBankAccountList(dataTable);
            }
            catch (Exception)
            {
                Debug.WriteLine("Oopsie on get bank accounts by user id repo");
                return null;
            }
        }

        /// <summary>
        /// Retrieves a bank account by its IBAN
        /// </summary>
        /// <param name="iban">The IBAN of the bank account to retrieve</param>
        /// <returns>The bank account with the specified IBAN, or null if not found or an error occurs</returns>
        public async Task<BankAccount?> GetBankAccountByIBAN(string iban)
        {
            try
            {
                var sqlParams = new SqlParameter[] { new SqlParameter("@iban", iban) };
                DataTable dataTable = await DataLink.Instance.ExecuteReader("GetBankAccountByIBAN", sqlParams);
                if (dataTable != null)
                {
                    return ConvertDataTableRowToBankAccount(dataTable.Rows[0]);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Oopsie on get bank account by IBAN repo");
                return null;
            }
        }

        /// <summary>
        /// Adds a new bank account to the database
        /// </summary>
        /// <param name="bankAccount">The bank account to add</param>
        /// <returns>True if the bank account was added successfully, false otherwise</returns>
        public async Task<bool> AddBankAccount(BankAccount bankAccount)
        {
            try
            {
                Debug.WriteLine(bankAccount.Iban);
                var sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@iban", bankAccount.Iban),
                    new SqlParameter("@currency", bankAccount.Currency),
                    new SqlParameter("@amount", bankAccount.Balance),
                    new SqlParameter("@id_user", bankAccount.UserID),
                    new SqlParameter("@custom_name",                bankAccount.Name),
                    new SqlParameter("@daily_limit",                bankAccount.DailyLimit),
                    new SqlParameter("@max_per_transaction",        bankAccount.MaximumPerTransaction),
                    new SqlParameter("@max_nr_transactions_daily",  bankAccount.MaximumNrTransactions),
                    new SqlParameter("@blocked",                    bankAccount.Blocked)
                };
                await DataLink.Instance.ExecuteNonQuery("AddBankAccount", sqlParams);
                return true;
            }
            catch (Exception)
            {
                Debug.WriteLine("Oopsie on add bank account repo");
                return false;
            }
        }

        /// <summary>
        /// Removes a bank account from the database
        /// </summary>
        /// <param name="iban">The IBAN of the bank account to remove</param>
        /// <returns>True if the bank account was removed successfully, false otherwise</returns>
        public async Task<bool> RemoveBankAccount(string iban)
        {
            try
            {
                Debug.WriteLine(iban);
                var sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@iban", iban)
                };
                await DataLink.Instance.ExecuteNonQuery("RemoveBankAccount", sqlParams);
                return true;
            }
            catch (Exception)
            {
                Debug.WriteLine("Oopsie on remove bank account repo");
                return false;
            }
        }

        /// <summary>
        /// Converts a DataRow to a BankAccount object
        /// </summary>
        /// <param name="row">The DataRow to convert</param>
        /// <returns>A BankAccount object with data from the row</returns>
        public BankAccount ConvertDataTableRowToBankAccount(DataRow row)
        {
            return new BankAccount(
                         Convert.ToString(row["iban"]) ?? string.Empty,
                         Convert.ToString(row["currency"]) ?? string.Empty,
                         decimal.Parse(row["amount"].ToString() ?? "0"),
                         Convert.ToBoolean(row["blocked"]),
                         Convert.ToInt32(row["id_user"]),
                         Convert.ToString(row["custom_name"]) ?? string.Empty,
                         decimal.Parse(row["daily_limit"].ToString() ?? "0"),
                         decimal.Parse(row["max_per_transaction"].ToString() ?? "0"),
                         Convert.ToInt32(row["max_nr_transactions_daily"]));
        }

        /// <summary>
        /// Converts a DataTable to a list of BankAccount objects
        /// </summary>
        /// <param name="dataTable">The DataTable to convert</param>
        /// <returns>A list of BankAccount objects</returns>
        public Task<List<BankAccount>> ConvertDataTableToBankAccountList(DataTable dataTable)
        {
            List<BankAccount> bankAccounts = new List<BankAccount>();
            foreach (DataRow row in dataTable.Rows)
            {
                bankAccounts.Add(ConvertDataTableRowToBankAccount(row));
            }
            return Task.FromResult(bankAccounts);
        }

        /// <summary>
        /// Converts a DataTable to a list of currency strings
        /// </summary>
        /// <param name="dataTable">The DataTable to convert</param>
        /// <returns>A list of currency names as strings</returns>
        public Task<List<string>> ConvertDataTableToCurrencyList(DataTable dataTable)
        {
            List<string> currencies = new List<string>();
            foreach (DataRow row in dataTable.Rows)
            {
                currencies.Add(Convert.ToString(row["currency_name"]) ?? string.Empty);
            }
            return Task.FromResult(currencies);
        }

        /// <summary>
        /// Retrieves all available currencies from the database
        /// </summary>
        /// <returns>A list of currency names as strings</returns>
        public async Task<List<string>> GetCurrencies()
        {
            DataTable dataTable = await DataLink.Instance.ExecuteReader("GetCurrencies");
            return await ConvertDataTableToCurrencyList(dataTable);
        }

        /// <summary>
        /// Retrieves user credentials for the specified email
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>A list containing the user's hashed password and salt</returns>
        public async Task<List<string>> GetCredentials(string email)
        {
            var sqlParams = new SqlParameter[] { new SqlParameter("@email", email) };
            DataTable dataTable = await DataLink.Instance.ExecuteReader("GetCredentials", sqlParams);
            List<string> credentials = new List<string>();
            credentials.Add(Convert.ToString(dataTable.Rows[0]["hashed_password"]) ?? string.Empty);
            credentials.Add(Convert.ToString(dataTable.Rows[0]["password_salt"]) ?? string.Empty);
            return credentials;
        }

        // updates the bank account with the given iban with the new attributes by calling
        // the sql procedure UpdateBankAccount
        public async Task<bool> UpdateBankAccount(string iban, BankAccount nba)
        {
            try
            {
                var sqlParams = new SqlParameter[]
                {
                             new SqlParameter("@iban", iban),
                             new SqlParameter("@custom_name", nba.Name),
                             new SqlParameter("@daily_limit", nba.DailyLimit),
                             new SqlParameter("@max_per_transaction", nba.MaximumPerTransaction),
                             new SqlParameter("@max_nr_transactions_daily", nba.MaximumNrTransactions),
                             new SqlParameter("@blocked", nba.Blocked)
                };
                await DataLink.Instance.ExecuteNonQuery("UpdateBankAccount", sqlParams);
                return true;
            }
            catch (Exception)
            {
                Debug.WriteLine("Could not update bank account");
                return false;
            }
        }
    }
}