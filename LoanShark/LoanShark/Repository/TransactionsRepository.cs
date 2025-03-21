using System;
using System.Collections.Generic;
using System.Data;
using LoanShark.Domain;
using LoanShark.Data;
using Microsoft.Data.SqlClient;

namespace LoanShark.Repository
{
    public class TransactionsRepository
    {
        private readonly DataLink _dataLink;
        private readonly UserSession _userSession;

        public TransactionsRepository()
        {
            _dataLink = new DataLink();
            _userSession = new UserSession(); 
        }

        public string GetCurrentUserIBAN()
        {
            //try
            //{
            //    string? iban = _userSession.GetUserData("current_bank_account_iban");
            //    return iban ?? string.Empty;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception($"Error retrieving current user IBAN: {ex.Message}", ex);
            //}
            // return _userSession.GetUserData("current_bank_account_iban") ?? string.Empty;
            // this is a temporary solution until we merge the code with the others functionalities
            return "RO12BANK0000000000000001";
        }

        public void AddTransaction(Transaction transaction)
        {
            try
            {
                if (transaction == null)
                    throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null");

                SqlParameter[] parameters =
                {
                    new SqlParameter("@SenderIBAN", transaction.SenderIban),
                    new SqlParameter("@ReceiverIBAN", transaction.ReceiverIban),
                    new SqlParameter("@SenderCurrency", transaction.SenderCurrency),
                    new SqlParameter("@ReceiverCurrency", transaction.ReceiverCurrency),
                    new SqlParameter("@SenderAmount", transaction.SenderAmount),
                    new SqlParameter("@ReceiverAmount", transaction.ReceiverAmount),
                    new SqlParameter("@TransactionType", transaction.TransactionType),
                    new SqlParameter("@description", string.IsNullOrWhiteSpace(transaction.TransactionDescription) ? DBNull.Value : transaction.TransactionDescription)
                };

                _dataLink.ExecuteNonQuery("AddTransaction", parameters);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error in AddTransaction: {ex.Message}", ex);
            }
        }

        public List<BankAccount> GetAllBankAccounts()
        {
            try
            {
                List<BankAccount> bankAccounts = new List<BankAccount>();
                DataTable result = _dataLink.ExecuteReader("GetAllBankAccounts");

                foreach (DataRow row in result.Rows)
                {
                    bankAccounts.Add(new BankAccount(
                        row["iban"].ToString(),
                        Convert.ToInt32(row["id_user"]),
                        Convert.ToDecimal(row["amount"]),
                        row["currency"].ToString(),
                        row["custom_name"]?.ToString(),
                        Convert.ToDecimal(row["daily_limit"]),
                        Convert.ToDecimal(row["max_per_transaction"]),
                        Convert.ToInt32(row["max_nr_transactions_daily"]),
                        Convert.ToBoolean(row["blocked"])
                    ));
                }

                return bankAccounts;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error in GetAllBankAccounts: {ex.Message}", ex);
            }
        }

        public List<CurrencyExchange> GetAllCurrencyExchangeRates()
        {
            try
            {
                List<CurrencyExchange> exchangeRates = new List<CurrencyExchange>();
                DataTable result = _dataLink.ExecuteReader("GetAllCurrencyExchangeRates");

                foreach (DataRow row in result.Rows)
                {
                    exchangeRates.Add(new CurrencyExchange(
                        row["from_currency"].ToString(),
                        row["to_currency"].ToString(),
                        Convert.ToDecimal(row["rate"])
                    ));
                }

                return exchangeRates;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error in GetAllCurrencyExchangeRates: {ex.Message}", ex);
            }
        }

        public List<Transaction> GetAllTransactions()
        {
            try
            {
                List<Transaction> transactions = new List<Transaction>();
                DataTable result = _dataLink.ExecuteReader("GetAllTransactions");

                foreach (DataRow row in result.Rows)
                {
                    transactions.Add(new Transaction(
                        Convert.ToInt32(row["transaction_id"]),
                        row["sender_iban"].ToString(),
                        row["receiver_iban"].ToString(),
                        Convert.ToDateTime(row["transaction_datetime"]),
                        row["sender_currency"].ToString(),
                        row["receiver_currency"].ToString(),
                        Convert.ToDecimal(row["sender_amount"]),
                        Convert.ToDecimal(row["receiver_amount"]),
                        row["transaction_type"].ToString(),
                        row["transaction_description"].ToString()
                    ));
                }

                return transactions;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error in GetAllTransactions: {ex.Message}", ex);
            }
        }

        public BankAccount? GetBankAccountByIBAN(string iban)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(iban))
                    throw new ArgumentException("IBAN cannot be empty.", nameof(iban));

                SqlParameter[] parameters = { new SqlParameter("@IBAN", iban) };
                DataTable result = _dataLink.ExecuteReader("GetBankAccountByIBAN", parameters);

                if (result.Rows.Count == 0) return null;

                DataRow row = result.Rows[0];

                return new BankAccount(
                    row["iban"].ToString(),
                    Convert.ToInt32(row["id_user"]),
                    Convert.ToDecimal(row["amount"]),
                    row["currency"].ToString(),
                    row["custom_name"]?.ToString(),
                    Convert.ToDecimal(row["daily_limit"]),
                    Convert.ToDecimal(row["max_per_transaction"]),
                    Convert.ToInt32(row["max_nr_transactions_daily"]),
                    Convert.ToBoolean(row["blocked"])
                );
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error in GetBankAccountByIBAN: {ex.Message}", ex);
            }
        }

        public List<Transaction> GetBankAccountTransactions(string iban)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(iban))
                    throw new ArgumentException("IBAN cannot be empty.", nameof(iban));

                SqlParameter[] parameters = { new SqlParameter("@IBAN", iban) };
                DataTable result = _dataLink.ExecuteReader("GetBankAccountTransactions", parameters);

                List<Transaction> transactions = new List<Transaction>();

                foreach (DataRow row in result.Rows)
                {
                    transactions.Add(new Transaction(
                        Convert.ToInt32(row["transaction_id"]),
                        row["sender_iban"].ToString(),
                        row["receiver_iban"].ToString(),
                        Convert.ToDateTime(row["transaction_datetime"]),
                        row["sender_currency"].ToString(),
                        row["receiver_currency"].ToString(),
                        Convert.ToDecimal(row["sender_amount"]),
                        Convert.ToDecimal(row["receiver_amount"]),
                        row["transaction_type"].ToString(),
                        row["transaction_description"].ToString()
                    ));
                }

                return transactions;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error in GetBankAccountTransactions: {ex.Message}", ex);
            }
        }

        public decimal GetExchangeRate(string fromCurrency, string toCurrency)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@FromCurrency", fromCurrency),
                    new SqlParameter("@ToCurrency", toCurrency)
                };

                object result = _dataLink.ExecuteScalar<decimal>("GetExchangeRate", parameters);
                return result != null ? Convert.ToDecimal(result) : -1;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error in GetExchangeRate: {ex.Message}", ex);
            }
        }

        public void UpdateBankAccountBalance(string iban, decimal newBalance)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(iban))
                    throw new ArgumentException("IBAN must be provided.", nameof(iban));

                if (newBalance < 0)
                    throw new ArgumentException("Balance cannot be negative.");

                SqlParameter[] parameters =
                {
                    new SqlParameter("@IBAN", iban),
                    new SqlParameter("@amount", newBalance)
                };

                _dataLink.ExecuteNonQuery("UpdateBankAccountBalance", parameters);
            }
            catch (SqlException ex)
            {
                throw new Exception($"Database error in UpdateBankAccountBalance: {ex.Message}", ex);
            }
        }
    }
}
