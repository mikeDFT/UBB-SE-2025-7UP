using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.Service;
using LoanShark.View;

namespace LoanShark.ViewModel
{
    public class BankAccountUpdateViewModel : INotifyPropertyChanged
    {
        private readonly BankAccountService? bankAccountService;
        private BankAccount? bankAccount;

        public event PropertyChangedEventHandler? PropertyChanged;

        private string accountIBAN = string.Empty;
        public string AccountIBAN
        {
            get => accountIBAN;
            set
            {
                if (accountIBAN != value)
                {
                    accountIBAN = value ?? string.Empty;
                    OnPropertyChanged();
                }
            }
        }

        private string accountName = string.Empty;
        public string AccountName
        {
            get => accountName;
            set
            {
                if (accountName != value)
                {
                    accountName = value ?? string.Empty;
                    OnPropertyChanged();
                }
            }
        }

        // left those two as double because the NumberBox expects a double to display the value
        private double dailyLimit = 1000.0;
        public double DailyLimit
        {
            get => dailyLimit;
            set
            {
                if (dailyLimit != value)
                {
                    dailyLimit = value;
                    OnPropertyChanged();
                }
            }
        }

        private double maximumPerTransaction = 200.0;
        public double MaximumPerTransaction
        {
            get => maximumPerTransaction;
            set
            {
                if (maximumPerTransaction != value)
                {
                    maximumPerTransaction = value;
                    OnPropertyChanged();
                }
            }
        }

        private int maximumNrTransactions = 10;
        public int MaximumNrTransactions
        {
            get => maximumNrTransactions;
            set
            {
                if (maximumNrTransactions != value)
                {
                    maximumNrTransactions = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isBlocked = false;
        public bool IsBlocked
        {
            get => isBlocked;
            set
            {
                if (isBlocked != value)
                {
                    isBlocked = value;
                    OnPropertyChanged();
                }
            }
        }

        public Action? OnUpdateSuccess { get; set; }
        public Action? OnClose { get; set; }

        // initializes the view model and loads the bank account for which the settings are to be updated
        public BankAccountUpdateViewModel()
        {
            if (string.IsNullOrEmpty(UserSession.Instance.GetUserData("current_bank_account_iban")))
            {
                throw new ArgumentException("IBAN cannot be null or empty");
            }

            try
            {
                bankAccountService = new BankAccountService();
                // Note: Async initialization will be done by calling InitializeAsync()
                _ = InitializeAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in BankAccountUpdateViewModel constructor: {ex.Message}");
                throw;
            }
        }

        public async Task InitializeAsync()
        {
            await LoadBankAccount();
            BankAccount bk = bankAccount;

            AccountName = bk.Name;
            DailyLimit = decimal.ToDouble(bk.DailyLimit);
            MaximumPerTransaction = decimal.ToDouble(bk.MaximumPerTransaction);
            MaximumNrTransactions = bk.MaximumNrTransactions;
            IsBlocked = bk.Blocked;
        }

        // loads the bank account with the given iban from the database
        private async Task LoadBankAccount()
        {
            if (string.IsNullOrEmpty(UserSession.Instance.GetUserData("current_bank_account_iban")))
            {
                throw new ArgumentException("IBAN cannot be null or empty");
            }

            try
            {
                string iban = UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty;
                bankAccount = await bankAccountService.FindBankAccount(iban);
                if (bankAccount != null)
                {
                    AccountIBAN = bankAccount.Iban ?? string.Empty;
                    AccountName = bankAccount.Name ?? string.Empty;
                    DailyLimit = decimal.ToDouble(bankAccount.DailyLimit);
                    MaximumPerTransaction = decimal.ToDouble(bankAccount.MaximumPerTransaction);
                    MaximumNrTransactions = bankAccount.MaximumNrTransactions;
                    IsBlocked = bankAccount.Blocked;
                }
                else
                {
                    Debug.WriteLine($"Bank account not found for IBAN: {UserSession.Instance.GetUserData("current_bank_account_iban")}");
                    throw new InvalidOperationException($"Bank account not found for IBAN: {UserSession.Instance.GetUserData("current_bank_account_iban")}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading bank account: {ex.Message}");
                throw;
            }
        }
        // checks for the inputs to be valid and be changed from the initial ones and updates the database
        // with the new settings, otherwise returns a message according to the error
        public async Task<string> UpdateBankAccount()
        {
            try
            {
                if (string.IsNullOrEmpty(AccountIBAN))
                {
                    return "IBAN cannot be empty";
                }

                if (string.IsNullOrEmpty(AccountName))
                {
                    return "Account name cannot be empty";
                }

                if (DailyLimit < 0)
                {
                    return "Daily limit cannot be negative";
                }

                if (MaximumPerTransaction < 0)
                {
                    return "Maximum per transaction cannot be negative";
                }

                if (MaximumNrTransactions < 0)
                {
                    return "Maximum number of transactions cannot be negative";
                }
                if (AccountName == bankAccount.Name && DailyLimit == decimal.ToDouble(bankAccount.DailyLimit) &&
                    MaximumPerTransaction == decimal.ToDouble(bankAccount.MaximumPerTransaction) &&
                    MaximumNrTransactions == bankAccount.MaximumNrTransactions &&
                    IsBlocked == bankAccount.Blocked)
                {
                    return "Failed to update bank account. No settings were changed";
                }

                bool result = await bankAccountService.UpdateBankAccount(
                    AccountIBAN,
                    AccountName,
                    (decimal)DailyLimit, // converting back from double to decimal
                    (decimal)MaximumPerTransaction,
                    MaximumNrTransactions,
                    IsBlocked);

                if (result)
                {
                    return "Success";
                }

                return "Failed to update bank account";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating bank account: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        public void DeleteBankAccount()
        {
            try
            {
                BankAccountDeleteView deleteBankAccountView = new BankAccountDeleteView();
                deleteBankAccountView.Activate();
                OnClose?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting bank account: {ex.Message}");
                throw new Exception("Error deleting bank account", ex);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
