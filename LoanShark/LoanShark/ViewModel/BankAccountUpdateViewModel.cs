using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using LoanShark.Domain;
using LoanShark.Service;

namespace LoanShark.ViewModel
{
    public class BankAccountUpdateViewModel : INotifyPropertyChanged
    {
        private readonly BankAccountService _bankAccountService;
        private BankAccount _bankAccount;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public Action OnUpdateSuccess { get; set; }
        public Action OnClose { get; set; }

        public BankAccountUpdateViewModel(string iban)
        {
            if (string.IsNullOrEmpty(iban))
            {
                throw new ArgumentException("IBAN cannot be null or empty");
            }

            try
            {
                _bankAccountService = new BankAccountService();
                LoadBankAccount(iban);
                BankAccount bk = _bankAccount;

                AccountName = bk.name;
                DailyLimit = bk.dailyLimit;
                MaximumPerTransaction = bk.maximumPerTransaction;
                MaximumNrTransactions = bk.maximumNrTransactions;
                IsBlocked = bk.blocked;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in BankAccountUpdateViewModel constructor: {ex.Message}");
                throw;
            }
        }

        private void LoadBankAccount(string iban)
        {
            if (string.IsNullOrEmpty(iban))
            {
                throw new ArgumentException("IBAN cannot be null or empty");
            }

            try
            {
                _bankAccount = _bankAccountService.findBankAccount(iban);
                if (_bankAccount != null)
                {
                    AccountIBAN = _bankAccount.iban ?? string.Empty;
                    AccountName = _bankAccount.name ?? string.Empty;
                    DailyLimit = _bankAccount.dailyLimit;
                    MaximumPerTransaction = _bankAccount.maximumPerTransaction;
                    MaximumNrTransactions = _bankAccount.maximumNrTransactions;
                    IsBlocked = _bankAccount.blocked;
                }
                else
                {
                    Debug.WriteLine($"Bank account not found for IBAN: {iban}");
                    throw new InvalidOperationException($"Bank account not found for IBAN: {iban}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading bank account: {ex.Message}");
                throw;
            }
        }

        public string UpdateBankAccount()
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

                bool result = _bankAccountService.updateBankAccount(
                    AccountIBAN,
                    AccountName,
                    (float)DailyLimit,
                    (float)MaximumPerTransaction,
                    MaximumNrTransactions,
                    IsBlocked);

                if (result)
                {
                    OnUpdateSuccess?.Invoke();
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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BankAccount GetBankAccount()
        {
            return _bankAccount;
        }

    }
}
