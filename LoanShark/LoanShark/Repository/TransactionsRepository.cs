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
        private UserSession _userSession;

        public TransactionsRepository(UserSession userSession)
        {
            _dataLink = new DataLink();
            _userSession = userSession;
        }

        public UserSession GetUserSession()
        {
            return _userSession;
        }

        public List<BankAccount> GetAllBankAccounts()
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

        public BankAccount? GetBankAccountByIBAN(string iban)
        {
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

        public List<Transaction> GetAllTransactions()
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

        public List<Transaction> GetBankAccountTransactions(string iban)
        {
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


        public void AddTransaction(Transaction transaction)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@SenderIBAN", transaction.SenderIban),
                new SqlParameter("@ReceiverIBAN", transaction.ReceiverIban),
                new SqlParameter("@SenderCurrency", transaction.SenderCurrency),
                new SqlParameter("@ReceiverCurrency", transaction.ReceiverCurrency),
                new SqlParameter("@SenderAmount", transaction.SenderAmount),
                new SqlParameter("@ReceiverAmount", transaction.ReceiverAmount),
                new SqlParameter("@TransactionType", transaction.TransactionType),
                new SqlParameter("@TransactionDescription", transaction.TransactionDescription)
            };

            _dataLink.ExecuteNonQuery("AddTransaction", parameters);
        }

        public void UpdateBankAccountBalance(string iban, decimal newBalance)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@IBAN", iban),
                new SqlParameter("@NewBalance", newBalance)
            };

            _dataLink.ExecuteNonQuery("UpdateBankAccountBalance", parameters);
        }

        public Dictionary<string, decimal> GetAllCurrencyExchangeRates()
        {
            Dictionary<string, decimal> currencyRates = new Dictionary<string, decimal>();

            DataTable result = _dataLink.ExecuteReader("GetAllCurrencyExchangeRates");

            foreach (DataRow row in result.Rows)
            {
                string fromCurrency = row["from_currency"].ToString();
                decimal rate = Convert.ToDecimal(row["rate"]);

                currencyRates[fromCurrency] = rate;
            }

            return currencyRates;
        }
    }
}
