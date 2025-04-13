using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Diagnostics;
using LoanShark.Domain;
using LoanShark.Service;
using LoanShark.View;

namespace LoanShark.ViewModel
{
    public class BankAccountUpdateViewModel : INotifyPropertyChanged
    {
        private readonly BankAccountService? _bankAccountService;
        private BankAccount? _bankAccount;

        public event PropertyChangedEventHandler? PropertyChanged;

        private string _accountIBAN = string.Empty;
        public string AccountIBAN
        {
            get => _accountIBAN;
            set
            {
                if (_accountIBAN != value)
                {
                    _accountIBAN = value ?? string.Empty;
                    OnPropertyChanged();
                }
            }
        }

        private string _accountName = string.Empty;
        public string AccountName
        {
            get => _accountName;
            set
            {
                if (_accountName != value)
                {
                    _accountName = value ?? string.Empty;
                    OnPropertyChanged();
                }
            }
        }

        // left those two as double because the NumberBox expects a double to display the value
        private double _dailyLimit = 1000.0;
        public double DailyLimit
        {
            get => _dailyLimit;
            set
            {
                if (_dailyLimit != value)
                {
                    _dailyLimit = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _maximumPerTransaction = 200.0;
        public double MaximumPerTransaction
        {
            get => _maximumPerTransaction;
            set
            {
                if (_maximumPerTransaction != value)
                {
                    _maximumPerTransaction = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _maximumNrTransactions = 10;
        public int MaximumNrTransactions
        {
            get => _maximumNrTransactions;
            set
            {
                if (_maximumNrTransactions != value)
                {
                    _maximumNrTransactions = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isBlocked = false;
        public bool IsBlocked
        {
            get => _isBlocked;
            set
            {
                if (_isBlocked != value)
                {
                    _isBlocked = value;
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
                _bankAccountService = new BankAccountService();
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
            BankAccount bk = _bankAccount;

            AccountName = bk.Name;
            DailyLimit = Decimal.ToDouble(bk.DailyLimit);
            MaximumPerTransaction = Decimal.ToDouble(bk.MaximumPerTransaction);
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
                string iban = UserSession.Instance.GetUserData("current_bank_account_iban") ?? "";
                _bankAccount = await _bankAccountService.FindBankAccount(iban);
                if (_bankAccount != null)
                {
                    AccountIBAN = _bankAccount.Iban ?? string.Empty;
                    AccountName = _bankAccount.Name ?? string.Empty;
                    DailyLimit = Decimal.ToDouble(_bankAccount.DailyLimit);
                    MaximumPerTransaction = Decimal.ToDouble(_bankAccount.MaximumPerTransaction);
                    MaximumNrTransactions = _bankAccount.MaximumNrTransactions;
                    IsBlocked = _bankAccount.Blocked;
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
                if (AccountName == _bankAccount.Name && DailyLimit==Decimal.ToDouble(_bankAccount.DailyLimit) &&
                    MaximumPerTransaction == Decimal.ToDouble(_bankAccount.MaximumPerTransaction) &&
                    MaximumNrTransactions == _bankAccount.MaximumNrTransactions &&
                    IsBlocked == _bankAccount.Blocked)
                    return "Failed to update bank account. No settings were changed";

                bool result = await _bankAccountService.UpdateBankAccount(
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
            try {
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
