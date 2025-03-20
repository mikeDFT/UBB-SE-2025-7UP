using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LoanShark.Data;
using LoanShark.Domain;
using LoanShark.Repository;
using Microsoft.UI.Xaml;

namespace LoanShark.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string? welcomeText;
        private ObservableCollection<BankAccount> userBankAccounts;
        private readonly LoginRepository repository;
        
        public event PropertyChangedEventHandler? PropertyChanged;

        public MainPageViewModel()
        {
            repository = new LoginRepository();
            userBankAccounts = new ObservableCollection<BankAccount>();
            InitializeWelcomeText();
            LoadUserBankAccounts();
        }

        public string WelcomeText
        {
            get => this.welcomeText ?? "Welcome, user";
            set
            {
                if (this.welcomeText != value)
                {
                    this.welcomeText = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<BankAccount> UserBankAccounts
        {
            get => userBankAccounts;
            set
            {
                if (userBankAccounts != value)
                {
                    userBankAccounts = value;
                    OnPropertyChanged();
                }
            }
        }

        private void InitializeWelcomeText()
        {
            try {
                string? firstName = UserSession.Instance.GetUserData("first_name");
                this.WelcomeText = firstName != null ? $"Welcome back, {firstName}" : "Welcome, user";
            }
            catch (Exception ex) {
                this.WelcomeText = "Welcome, user";
                Debug.Print($"Error getting user data: {ex.Message}");
            }
        }

        private async void LoadUserBankAccounts()
        {
            try
            {
                string? userId = UserSession.Instance.GetUserData("id_user");
                if (string.IsNullOrEmpty(userId))
                {
                    Debug.Print("User ID is null or empty");
                    return;
                }

                int idUser = int.Parse(userId);
                DataTable bankAccountsData = await repository.GetUserBankAccounts(idUser);

                userBankAccounts.Clear();
                foreach (DataRow row in bankAccountsData.Rows)
                {
                    string iban = row["iban"]?.ToString() ?? string.Empty;
                    string currency = row["currency"]?.ToString() ?? string.Empty;
                    decimal amount = row["amount"] != DBNull.Value ? Convert.ToDecimal(row["amount"]) : 0;
                    string customName = row["custom_name"]?.ToString() ?? $"Account {userBankAccounts.Count + 1}";
                    decimal dailyLimit = row["daily_limit"] != DBNull.Value ? Convert.ToDecimal(row["daily_limit"]) : 0;
                    decimal maxPerTransaction = row["max_per_transaction"] != DBNull.Value ? Convert.ToDecimal(row["max_per_transaction"]) : 0;
                    int maxNrTransactionsDaily = row["max_nr_transactions_daily"] != DBNull.Value ? Convert.ToInt32(row["max_nr_transactions_daily"]) : 0;
                    bool blocked = row["blocked"] != DBNull.Value ? Convert.ToBoolean(row["blocked"]) : false;
                    
                    userBankAccounts.Add(new BankAccount(iban, idUser, amount, currency, customName, dailyLimit, maxPerTransaction, maxNrTransactionsDaily, blocked));
                }

                if (userBankAccounts.Count == 0)
                {
                    // Add a placeholder or default message if no accounts found
                    userBankAccounts.Add(new BankAccountMessage("No accounts found", "Please add a bank account"));
                }

                OnPropertyChanged(nameof(UserBankAccounts));
            }
            catch (Exception ex)
            {
                Debug.Print($"Error loading bank accounts: {ex.Message}");
                // Add a placeholder or error message
                userBankAccounts.Clear();
                userBankAccounts.Add(new BankAccountMessage("Error", "Could not load bank accounts"));
                OnPropertyChanged(nameof(UserBankAccounts));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
