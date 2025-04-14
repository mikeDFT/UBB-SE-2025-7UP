using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.Repository;

namespace LoanShark.Service
{
    public class MainPageService
    {
        private readonly MainPageRepository repo;

        public MainPageService()
        {
            this.repo = new MainPageRepository();
        }

        public async Task<ObservableCollection<BankAccount>> GetUserBankAccounts(int userId)
        {
            try
            {
                ObservableCollection<BankAccount> bankAccounts = new ObservableCollection<BankAccount>();
                DataTable bankAccountsData = await this.repo.GetUserBankAccounts(userId);

                foreach (DataRow row in bankAccountsData.Rows)
                {
                    string iban = row["iban"]?.ToString() ?? string.Empty;
                    string currency = row["currency"]?.ToString() ?? string.Empty;
                    decimal amount = row["amount"] != DBNull.Value ? Convert.ToDecimal(row["amount"]) : 0;
                    string customName = row["custom_name"]?.ToString() ?? $"Account {bankAccounts.Count + 1}";
                    decimal dailyLimit = row["daily_limit"] != DBNull.Value ? Convert.ToDecimal(row["daily_limit"]) : 0;
                    decimal maxPerTransaction = row["max_per_transaction"] != DBNull.Value ? Convert.ToDecimal(row["max_per_transaction"]) : 0;
                    int maxNrTransactionsDaily = row["max_nr_transactions_daily"] != DBNull.Value ? Convert.ToInt32(row["max_nr_transactions_daily"]) : 0;
                    bool blocked = row["blocked"] != DBNull.Value ? Convert.ToBoolean(row["blocked"]) : false;
                    bankAccounts.Add(new BankAccount(iban, currency, amount, blocked, userId, customName, dailyLimit, maxPerTransaction, maxNrTransactionsDaily));
                }

                return bankAccounts;
            }
            catch (Exception ex)
            {
                Debug.Print($"Service error getting bank accounts: {ex.Message}");
                throw;
            }
        }

        public async Task<Tuple<decimal, string>> GetBankAccountBalanceByUserIban(string iban)
        {
            return await this.repo.GetBankAccountBalanceByUserIban(iban);
        }
    }
}
