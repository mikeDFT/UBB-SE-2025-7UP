using LoanShark.Domain;
using LoanShark.Service;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Diagnostics;
using Windows.Media.Core;
using Microsoft.UI.Xaml.Controls;


namespace LoanShark.ViewModel
{
    public class LoanViewModel : INotifyPropertyChanged
    {
        private string selectedBankAccount;
        private Loan selectedLoan;
        private string selectedLoanDisplay;
        private decimal amount;
        private int selectedMonths;
        private string takeErrorMessage;
        private string payErrorMessage;
        private string currentPage;
        private string taxPercentage = "Tax Percentage: N/A";
        private string amountToPay = "Amount to Pay: N/A";
        private string selectedLoanAmount = "Loan Amount: N/A";
        private string selectedAccountBalance = "Account Balance: N/A";
        private string convertedLoanAmount = "Converted Amount: N/A";

        // Service for data operations and business logic
        private readonly LoanService _loanService;

        public ObservableCollection<Loan> Loans { get; set; }
        public ObservableCollection<string> BankAccounts { get; set; }
        public ObservableCollection<int> Months { get; set; }
        public ObservableCollection<Loan> UnpaidLoans { get; set; }
        public ObservableCollection<string> UnpaidLoansDisplay { get; set; }

        public string TaxPercentage
        {
            get => taxPercentage;
            set
            {
                if (taxPercentage != value)
                {
                    taxPercentage = value;
                    OnPropertyChanged(nameof(TaxPercentage));
                    Debug.WriteLine($"TaxPercentage updated to: {value}");
                }
            }
        }

        public string AmountToPay
        {
            get => amountToPay;
            set
            {
                if (amountToPay != value)
                {
                    amountToPay = value;
                    OnPropertyChanged(nameof(AmountToPay));
                    Debug.WriteLine($"AmountToPay updated to: {value}");
                }
            }
        }

        public string SelectedBankAccount
        {
            get => selectedBankAccount;
            set
            {
                if (selectedBankAccount != value)
                {
                    selectedBankAccount = value;
                    OnPropertyChanged(nameof(SelectedBankAccount));
                    
                    // For Take Loan page
                    UpdateTakeLoanBoxDetails();
                    
                    // For Pay Loan page
                    UpdatePayLoanBoxDetails();
                }
            }
        }

        public Loan SelectedLoan
        {
            get => selectedLoan;
            set
            {
                if (selectedLoan != value)
                {
                    selectedLoan = value;
                    OnPropertyChanged(nameof(SelectedLoan));
                }
            }
        }

        public decimal Amount
        {
            get => amount;
            set
            {
                if (amount != value)
                {
                    amount = value;
                    OnPropertyChanged(nameof(Amount));
                    Debug.WriteLine("Amount", Amount);
                    UpdateTakeLoanBoxDetails();
                }
            }
        }

        public int SelectedMonths
        {
            get => selectedMonths;
            set
            {
                if (selectedMonths != value)
                {
                    selectedMonths = value;
                    OnPropertyChanged(nameof(SelectedMonths));
                    Debug.WriteLine($"SelectedMonths changed to: {value}");
                    UpdateTakeLoanBoxDetails();
                }
            }
        }

        public string TakeErrorMessage
        {
            get => takeErrorMessage;
            set
            {
                if (takeErrorMessage != value)
                {
                    takeErrorMessage = value;
                    OnPropertyChanged(nameof(TakeErrorMessage));
                }
            }
        }

        public string PayErrorMessage
        {
            get => payErrorMessage;
            set
            {
                if (payErrorMessage != value)
                {
                    payErrorMessage = value;
                    OnPropertyChanged(nameof(PayErrorMessage));
                }
            }
        }

        public string CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
                UpdatePageVisibility();
            }
        }

        public string SelectedLoanAmount
        {
            get => selectedLoanAmount;
            set
            {
                if (selectedLoanAmount != value)
                {
                    selectedLoanAmount = value;
                    OnPropertyChanged(nameof(SelectedLoanAmount));
                }
            }
        }

        public string SelectedAccountBalance
        {
            get => selectedAccountBalance;
            set
            {
                if (selectedAccountBalance != value)
                {
                    selectedAccountBalance = value;
                    OnPropertyChanged(nameof(SelectedAccountBalance));
                    UpdateSelectedLoan();
                }
            }
        }

        public string SelectedLoanDisplay
        {
            get => selectedLoanDisplay;
            set
            {
                if (selectedLoanDisplay != value)
                {
                    selectedLoanDisplay = value;
                    OnPropertyChanged(nameof(SelectedLoanDisplay));
                    UpdateSelectedLoan();
                    UpdatePayLoanBoxDetails();
                }
            }
        }

        public string ConvertedLoanAmount
        {
            get => convertedLoanAmount;
            set
            {
                if (convertedLoanAmount != value)
                {
                    convertedLoanAmount = value;
                    OnPropertyChanged(nameof(ConvertedLoanAmount));
                }
            }
        }

        private Visibility mainLoansPageVisibility;
        public Visibility MainLoansPageVisibility
        {
            get => mainLoansPageVisibility;
            set
            {
                mainLoansPageVisibility = value;
                OnPropertyChanged(nameof(MainLoansPageVisibility));
            }
        }

        private Visibility takeLoanPageVisibility;
        public Visibility TakeLoanPageVisibility
        {
            get => takeLoanPageVisibility;
            set
            {
                takeLoanPageVisibility = value;
                OnPropertyChanged(nameof(TakeLoanPageVisibility));
            }
        }

        private Visibility payLoanPageVisibility;
        public Visibility PayLoanPageVisibility
        {
            get => payLoanPageVisibility;
            set
            {
                payLoanPageVisibility = value;
                OnPropertyChanged(nameof(PayLoanPageVisibility));
            }
        }

        private Visibility hasUnpaidLoans;
        public Visibility HasUnpaidLoans
        {
            get => hasUnpaidLoans;
            set
            {
                hasUnpaidLoans = value;
                OnPropertyChanged(nameof(hasUnpaidLoans));
            }
        }

        public ICommand TakeLoanCommand { get; }
        public ICommand PayLoanCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand GoToTakeLoanPageCommand { get; }
        public ICommand GoToPayLoanPageCommand { get; }
        public ICommand CloseCommand { get; }

        // Close action delegate for the CloseCommand
        public Action CloseAction { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateUnpaidLoansDisplay()
        {
            UnpaidLoansDisplay = new ObservableCollection<string>();
            foreach (var loan in UnpaidLoans)
            {
                string display = $"{loan.Currency} - {loan.AmountToPay:F2} - {loan.DateDeadline:d}";
                UnpaidLoansDisplay.Add(display);
            }
        }

        public LoanViewModel()
        {
            // Initialize the loan service
            _loanService = new LoanService();
            
            // Initialize collections
            Loans = new ObservableCollection<Loan>();
            BankAccounts = new ObservableCollection<string>();
            Months = new ObservableCollection<int> { 6, 12, 24, 36 };
            UnpaidLoans = new ObservableCollection<Loan>();
            UnpaidLoansDisplay = new ObservableCollection<string>();

            // Refreshing the data from the service
            LoadData();

            // Initialize commands
            TakeLoanCommand = new RelayCommand(TakeLoan);
            PayLoanCommand = new RelayCommand(PayLoan);
            BackCommand = new RelayCommand(Back);
            GoToTakeLoanPageCommand = new RelayCommand(() => CurrentPage = "TakeLoanPage");
            GoToPayLoanPageCommand = new RelayCommand(() => CurrentPage = "PayLoanPage");
            CloseCommand = new RelayCommand(() => CloseAction?.Invoke());

            // Set initial page
            CurrentPage = "MainLoansPage";
            UpdatePageVisibility();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // For the take a loan page (the details in the box)
        private void UpdateTakeLoanBoxDetails()
        {
            Debug.WriteLine($"UpdateTakeLoanBoxDetails called. SelectedBankAccount: {SelectedBankAccount}, Amount: {Amount}, SelectedMonths: {SelectedMonths}");

            if (string.IsNullOrEmpty(SelectedBankAccount) || Amount <= 0 || SelectedMonths <= 0)
            {
                // when there is no number of months chosen, the tax percentage is N/A
                if (SelectedMonths <= 0)
                {
                    TaxPercentage = "Tax Percentage: N/A";
                    AmountToPay = "Amount to Pay: N/A";
                }
                else
                {
                    decimal taxPercentageValue = _loanService.CalculateTaxPercentage(SelectedMonths);
                    decimal amountToPayValue = _loanService.CalculateAmountToPay(Amount, taxPercentageValue);
                    TaxPercentage = $"Tax Percentage: {taxPercentageValue}%";
                    AmountToPay = $"Amount to Pay: {amountToPayValue:F2}";
                }
            }
            
            try
            {
                // Use service to calculate tax percentage and amount to pay
                decimal taxPercentageValue = _loanService.CalculateTaxPercentage(SelectedMonths);
                decimal amountToPayValue = _loanService.CalculateAmountToPay(Amount, taxPercentageValue);
                
                TaxPercentage = $"Tax Percentage: {taxPercentageValue}%";
                AmountToPay = $"Amount to Pay: {amountToPayValue:F2}";}
            catch (Exception)
            {
                TaxPercentage = "Tax Percentage: Error";
                AmountToPay = "Amount to Pay: Error";
            }
        }

        private async void TakeLoan()
        {
            Debug.WriteLine("TakeLoan method called");
            
            try
            {
                // Validate inputs through the service
                string errorMessage = _loanService.ValidateLoanRequest(Amount, SelectedMonths);
                if (errorMessage != "success")
                {
                    TakeErrorMessage = errorMessage;
                    Debug.WriteLine($"Validation failed: SelectedBankAccount: {SelectedBankAccount}, Amount: {Amount}, SelectedMonths: {SelectedMonths}");
                    return;
                }

                // Get currency from selected bank account
                string currency = ExtractCurrencyFromBankAccount(SelectedBankAccount);
                
                // Create new loan using the service
                var newLoan = await _loanService.TakeLoanAsync(int.Parse(UserSession.Instance.GetUserData("id_user")), Amount, currency, ExtractIbanFromBankAccount(SelectedBankAccount), SelectedMonths);

                // Refreshing the data from the service
                LoadData();

                // Clear input fields
                ClearTakeLoanFields();
                
                // Navigate back to main page
                CurrentPage = "MainLoansPage";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TakeLoan: {ex.Message}");
                TakeErrorMessage = "An error occurred while processing your loan";
            }
        }

        private string ExtractCurrencyFromBankAccount(string bankAccount)
        {
            Debug.WriteLine($"Extracting currency from: {bankAccount}");
            
            // Format: "IBAN1 - EUR - 1000"
            string[] parts = bankAccount.Split('-');
            if (parts.Length >= 2)
            {
                string currency = parts[1].Trim();
                Debug.WriteLine($"Extracted currency: {currency}");
                return currency;
            }
            
            Debug.WriteLine("Could not extract currency, using default EUR");
            return "EUR"; // Default currency
        }

        private void ClearTakeLoanFields()
        {
            SelectedBankAccount = null;
            Amount = 0;
            selectedMonths = 0;
            
            TakeErrorMessage = string.Empty;
            TaxPercentage = "Tax Percentage: N/A";
            AmountToPay = "Amount to Pay: N/A";
        }

        private void ClearPayLoanFields()
        {
            SelectedBankAccount = null;
            selectedLoan = null;
            SelectedLoanDisplay = null;
            PayErrorMessage = string.Empty;
            SelectedLoanAmount = "Loan Amount: N/A";
            SelectedAccountBalance = "Account Balance: N/A";
            ConvertedLoanAmount = "Converted Amount: N/A";
        }

        private void UpdateSelectedLoan()
        {
            Debug.WriteLine($"UpdateSelectedLoan called with SelectedLoanDisplay: {SelectedLoanDisplay}");
            
            if (string.IsNullOrEmpty(SelectedLoanDisplay))
            {
                selectedLoan = null;
                SelectedLoanAmount = "Loan Amount: N/A";
                ConvertedLoanAmount = "Converted Amount: N/A";
                UpdatePayLoanBoxDetails();
                return;
            }

            // Find the loan that matches the display string
            foreach (var loan in UnpaidLoans)
            {
                string display = $"{loan.Currency} - {loan.AmountToPay:F2} - {loan.DateDeadline:d}";
                if (display == SelectedLoanDisplay)
                {
                    selectedLoan = loan;
                    SelectedLoanAmount = $"Loan Amount: {loan.AmountToPay:F2} {loan.Currency}";
                    Debug.WriteLine($"Found matching loan: ID={loan.LoanID}, Amount={loan.AmountToPay}, Currency={loan.Currency}");
                    break;
                }
            }
            
            UpdatePayLoanBoxDetails();
        }

        // For the pay a loan page (the details in the box)
        private async void UpdatePayLoanBoxDetails()
        {
            Debug.WriteLine("UpdatePayLoanBoxDetails called");
            
            if (string.IsNullOrEmpty(SelectedBankAccount))
            {
                SelectedAccountBalance = "Account Balance: N/A";
                ConvertedLoanAmount = "Converted Amount: N/A";
                return;
            }

            try
            {
                // Extract account details from bank account string
                string[] parts = SelectedBankAccount.Split('-');
                if (parts.Length >= 3)
                {
                    string accountCurrency = parts[1].Trim();
                    decimal accountBalance = decimal.Parse(parts[2].Trim());
                    string bankAccountId = parts[0].Trim();
                    
                    // Set account balance display
                    SelectedAccountBalance = $"Account Balance: {accountBalance:F2} {accountCurrency}";

                    // If there is no loan selected
                    if (string.IsNullOrEmpty(SelectedLoanDisplay))
                        return;

                    // Extract loan details
                    decimal loanAmount = selectedLoan.AmountToPay;
                    string loanCurrency = selectedLoan.Currency;
                    
                    // Check if currencies match
                    if (loanCurrency == accountCurrency)
                    {
                        // No conversion needed
                        ConvertedLoanAmount = $"Payment Required: {loanAmount:F2} {loanCurrency}";
                    }
                    else
                    {
                        // Convert loan amount to account currency using service
                        decimal convertedAmount = await _loanService.ConvertCurrency(loanAmount, loanCurrency, accountCurrency);
                        ConvertedLoanAmount = $"Payment Required: {convertedAmount:F2} {accountCurrency} (converted from {loanAmount:F2} {loanCurrency})";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating payment details: {ex.Message}");
                SelectedAccountBalance = "Account Balance: Error";
                ConvertedLoanAmount = "Converted Amount: Error";
            }
        }

        private bool arePayLoanDetailsValid()
        {
            Debug.WriteLine($"Validating pay loan details: BankAccount={SelectedBankAccount}, Loan={selectedLoan?.LoanID}");
            
            // Check if bank account is selected
            if (string.IsNullOrEmpty(SelectedBankAccount))
            {
                Debug.WriteLine("Validation failed: No bank account selected");
                return false;
            }
            
            // Check if loan is selected
            if (selectedLoan == null)
            {
                Debug.WriteLine("Validation failed: No loan selected");
                return false;
            }
            
            Debug.WriteLine("Validation passed");
            return true;
        }


        private async void PayLoan()
        {
            Debug.WriteLine("PayLoan method called");
            
            // Validate inputs
            if (!arePayLoanDetailsValid())
            {
                PayErrorMessage = "Invalid data provided";
                return;
            }

            try
            {
                // Get bank account ID from selected bank account
                string bankAccountId = ExtractIbanFromBankAccount(SelectedBankAccount);
                
                // Pay the loan using the service
                string errorMessage = await _loanService.PayLoanAsync(int.Parse(UserSession.Instance.GetUserData("id_user")), selectedLoan.LoanID, bankAccountId);
                
                if (errorMessage != "success")
                {
                   PayErrorMessage = errorMessage;
                   return;
                }
                
                // Refreshing the data from the service
                LoadData();

                // Clear fields
                ClearPayLoanFields();
                
                // Navigate back to main page
                CurrentPage = "MainLoansPage";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in PayLoan: {ex.Message}");
                PayErrorMessage = "An error occurred while processing your payment";
            }
        }

        private string ExtractIbanFromBankAccount(string bankAccount)
        {
            string[] parts = bankAccount.Split('-');
            if (parts.Length >= 1)
            {
                return parts[0].Trim();
            }
            throw new ArgumentException("Invalid bank account format");
        }

        private void Back()
        {
            CurrentPage = "MainLoansPage";
        }

        private void UpdatePageVisibility()
        {
            ClearTakeLoanFields();
            ClearPayLoanFields();
            MainLoansPageVisibility = CurrentPage == "MainLoansPage" ? Visibility.Visible : Visibility.Collapsed;
            TakeLoanPageVisibility = CurrentPage == "TakeLoanPage" ? Visibility.Visible : Visibility.Collapsed;
            PayLoanPageVisibility = CurrentPage == "PayLoanPage" ? Visibility.Visible : Visibility.Collapsed;
        }

        // Load data from the service, also used for reloading (refreshing) data
        private async void LoadData()
        {
            try
            {
                // Clear existing collections
                Loans.Clear();
                BankAccounts.Clear();
                UnpaidLoans.Clear();
                UnpaidLoansDisplay.Clear();
                
                // Get loans from service
                var userLoans = await _loanService.GetUserLoans(int.Parse(UserSession.Instance.GetUserData("id_user")));
                foreach (var loan in userLoans)
                {
                    Loans.Add(loan);
                }

                // Get unpaid loans for display
                List<Loan> unpaidLoans = await _loanService.GetUnpaidUserLoans(int.Parse(UserSession.Instance.GetUserData("id_user")));
                unpaidLoans.Sort((loan1, loan2) => loan1.DateTaken.CompareTo(loan2.DateTaken));

                foreach (var loan in unpaidLoans)
                {
                    UnpaidLoans.Add(loan);
                    string display = $"{loan.Currency} - {loan.AmountToPay:F2} - {loan.DateDeadline:d}";
                    UnpaidLoansDisplay.Add(display);
                }
                
                // Get bank accounts from service
                var formattedAccounts = await _loanService.GetFormattedBankAccounts(int.Parse(UserSession.Instance.GetUserData("id_user")));
                foreach (var account in formattedAccounts)
                {
                    BankAccounts.Add(account);
                }
                
                Debug.WriteLine($"Loaded data: {Loans.Count} loans, {UnpaidLoans.Count} unpaid loans, {BankAccounts.Count} bank accounts");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data: {ex.Message}");
                // Consider showing an error message to the user
            }
        }
    }

    // For the commands used above (like CloseCommand etc)
    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }
}