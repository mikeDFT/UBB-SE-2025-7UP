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
        private int userID;
        private string selectedBankAccount;
        private Loan selectedLoan;
        private string selectedLoanDisplay;
        private float amount;
        private int selectedMonths;
        private string errorMessage;
        private string payErrorMessage;
        private string currentPage;
        private string taxPercentage = "Tax Percentage: N/A";
        private string amountToPay = "Amount to Pay: N/A";
        private string selectedLoanAmount = "Loan Amount: N/A";
        private string selectedLoanCurrency = "Loan Currency: N/A";
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
                    UpdateAmountToPay();
                    
                    // For Pay Loan page
                    UpdatePaymentDetails();
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

        public float Amount
        {
            get => amount;
            set
            {
                if (amount != value)
                {
                    amount = value;
                    OnPropertyChanged(nameof(Amount));
                    Debug.WriteLine("Amount", Amount);
                    UpdateAmountToPay();
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
                    UpdateAmountToPay();
                }
            }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
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

        public string SelectedLoanCurrency
        {
            get => selectedLoanCurrency;
            set
            {
                if (selectedLoanCurrency != value)
                {
                    selectedLoanCurrency = value;
                    OnPropertyChanged(nameof(SelectedLoanCurrency));
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
                    UpdatePaymentDetails();
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

        public LoanViewModel(int userID)
        {
            this.userID = userID;
            
            // Initialize the loan service
            _loanService = new LoanService();
            
            // Initialize collections
            Loans = new ObservableCollection<Loan>();
            BankAccounts = new ObservableCollection<string>();
            Months = new ObservableCollection<int> { 6, 12, 24, 36 };
            UnpaidLoans = new ObservableCollection<Loan>();
            UnpaidLoansDisplay = new ObservableCollection<string>();
            
            // Load data from service
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

        private IEnumerable<string> GetBankAccounts()
        {
            return new List<string>
            {
                "IBAN1 - EUR - 1000",
                "IBAN2 - USD - 2000",
                "IBAN3 - GBP - 3000"
            };
        }

        private IEnumerable<Loan> GetUnpaidLoans()
        {
            var allLoans = GetAllLoans();
            return allLoans.Where(loan => loan.State == "unpaid");
        }

        private IEnumerable<Loan> GetAllLoans()
        {
            var currencies = new List<string> { "EUR", "USD", "GBP" };
            var Loans = new List<Loan>();
            for (int i = 0; i < 5; i++)
                Loans.Add(new Loan(i, i, currencies[i % currencies.Count], i, i));
            return Loans;
        }

        private bool areTakeLoanDetailsValid()
        {
            Debug.WriteLine($"Validating loan details: BankAccount={SelectedBankAccount}, Amount={Amount}, Months={SelectedMonths}");
            
            // Check if bank account is selected
            if (string.IsNullOrEmpty(SelectedBankAccount))
            {
                Debug.WriteLine("Validation failed: No bank account selected");
                return false;
            }
            
            // Check if amount is valid
            if (Amount <= 0 || Amount > 1000000)
            {
                Debug.WriteLine($"Validation failed: Invalid amount {Amount}");
                return false;
            }
            
            // Check if months is valid
            if (SelectedMonths <= 0)
            {
                Debug.WriteLine("Validation failed: No months selected");
                return false;
            }
            
            Debug.WriteLine("Validation passed");
            return true;
        }

        private void UpdateAmountToPay()
        {
            Debug.WriteLine($"UpdateAmountToPay called. SelectedBankAccount: {SelectedBankAccount}, Amount: {Amount}, SelectedMonths: {SelectedMonths}");
            
            if (string.IsNullOrEmpty(SelectedBankAccount) || Amount <= 0 || SelectedMonths <= 0)
            {
                TaxPercentage = "Tax Percentage: N/A";
                AmountToPay = "Amount to Pay: N/A";
                return;
            }
            
            try
            {
                // Use service to calculate tax percentage and amount to pay
                float taxPercentageValue = _loanService.CalculateTaxPercentage(SelectedMonths);
                float amountToPayValue = _loanService.CalculateAmountToPay(Amount, taxPercentageValue);
                
                TaxPercentage = $"Tax Percentage: {taxPercentageValue}%";
                AmountToPay = $"Amount to Pay: {amountToPayValue:F2}";
                
                Debug.WriteLine($"Calculated: Tax %: {taxPercentageValue}, Amount to pay: {amountToPayValue}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in UpdateAmountToPay: {ex.Message}");
                TaxPercentage = "Tax Percentage: Error";
                AmountToPay = "Amount to Pay: Error";
            }
        }

        private float CalculateTaxPercentage(int months)
        {
            // 1.5% per month
            return months * 1.5f;
        }

        private void TakeLoan()
        {
            Debug.WriteLine("TakeLoan method called");
            
            try
            {
                // Validate inputs through the service
                if (!_loanService.ValidateLoanRequest(Amount, SelectedMonths) || string.IsNullOrEmpty(SelectedBankAccount))
                {
                    ErrorMessage = "Invalid data provided";
                    Debug.WriteLine($"Validation failed: SelectedBankAccount: {SelectedBankAccount}, Amount: {Amount}, SelectedMonths: {SelectedMonths}");
                    return;
                }

                // Get currency from selected bank account
                string currency = ExtractCurrencyFromBankAccount(SelectedBankAccount);
                
                // Create new loan using the service
                var newLoan = _loanService.CreateLoan(userID, Amount, currency, SelectedMonths);
                
                // Add to collections
                Loans.Add(newLoan);
                UnpaidLoans.Add(newLoan);
                
                // Add to display collection
                string displayString = $"{newLoan.Currency} - {newLoan.AmountToPay:F2} - {newLoan.DateDeadline:d}";
                UnpaidLoansDisplay.Add(displayString);
                
                // Clear input fields
                ClearTakeLoanFields();
                
                // Navigate back to main page
                CurrentPage = "MainLoansPage";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in TakeLoan: {ex.Message}");
                ErrorMessage = "An error occurred while processing your loan";
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
            
            ErrorMessage = string.Empty;
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
            SelectedLoanCurrency = "Loan Currency: N/A";
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
                SelectedLoanCurrency = "Loan Currency: N/A";
                ConvertedLoanAmount = "Converted Amount: N/A";
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
                    SelectedLoanCurrency = $"Loan Currency: {loan.Currency}";
                    Debug.WriteLine($"Found matching loan: ID={loan.LoanID}, Amount={loan.AmountToPay}, Currency={loan.Currency}");
                    break;
                }
            }
            
            UpdatePaymentDetails();
        }

        private void UpdatePaymentDetails()
        {
            Debug.WriteLine("UpdatePaymentDetails called");
            
            if (selectedLoan == null || string.IsNullOrEmpty(SelectedBankAccount))
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
                    float accountBalance = float.Parse(parts[2].Trim());
                    string bankAccountId = parts[0].Trim();
                    
                    // Set account balance display
                    SelectedAccountBalance = $"Account Balance: {accountBalance:F2} {accountCurrency}";
                    
                    // Extract loan details
                    float loanAmount = (float)selectedLoan.AmountToPay;
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
                        float convertedAmount = _loanService.ConvertCurrency(loanAmount, loanCurrency, accountCurrency);
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

        private bool HasSufficientFunds()
        {
            if (selectedLoan == null || string.IsNullOrEmpty(SelectedBankAccount))
            {
                return false;
            }

            try
            {
                // Extract account details
                string[] accountParts = SelectedBankAccount.Split('-');
                if (accountParts.Length < 3) return false;
                
                string accountCurrency = accountParts[1].Trim();
                float accountBalance = float.Parse(accountParts[2].Trim());
                
                // Extract loan details
                float loanAmount = (float)selectedLoan.AmountToPay;
                string loanCurrency = selectedLoan.Currency;
                
                // If currencies match, simple comparison
                if (accountCurrency == loanCurrency)
                {
                    Debug.WriteLine($"Currencies match: Account={accountBalance} {accountCurrency}, Loan={loanAmount} {loanCurrency}");
                    return accountBalance >= loanAmount;
                }
                
                // Currencies differ, do conversion
                float convertedLoanAmount = ConvertCurrency(loanAmount, loanCurrency, accountCurrency);
                Debug.WriteLine($"Currencies differ: Account={accountBalance} {accountCurrency}, Loan={loanAmount} {loanCurrency}, Converted={convertedLoanAmount} {accountCurrency}");
                
                return accountBalance >= convertedLoanAmount;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking funds: {ex.Message}");
                return false;
            }
        }

        private float ConvertCurrency(float amount, string fromCurrency, string toCurrency)
        {
            // Simple conversion rates for example:
            // 1 EUR = 1.1 USD
            // 1 EUR = 0.85 GBP
            
            // Convert to EUR first as a base currency
            float amountInEUR;
            
            switch (fromCurrency) // to change
            {
                case "EUR":
                    amountInEUR = amount;
                    break;
                case "USD":
                    amountInEUR = amount / 1.1f;
                    break;
                case "GBP":
                    amountInEUR = amount / 0.85f;
                    break;
                default:
                    amountInEUR = amount; // Assume 1:1 for unknown currencies
                    break;
            }
            
            // Convert from EUR to target currency
            switch (toCurrency)
            {
                case "EUR":
                    return amountInEUR;
                case "USD":
                    return amountInEUR * 1.1f;
                case "GBP":
                    return amountInEUR * 0.85f;
                default:
                    return amountInEUR; // Assume 1:1 for unknown currencies
            }
        }

        private void PayLoan()
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
                bool success = _loanService.PayLoan(selectedLoan.LoanID, bankAccountId);
                
                if (!success)
                {
                    PayErrorMessage = "Not enough funds";
                    return;
                }
                
                // Remove the paid loan from collections
                Loans.Remove(selectedLoan);
                UnpaidLoans.Remove(selectedLoan);
                
                // Remove from display collection
                string displayToRemove = SelectedLoanDisplay;
                if (!string.IsNullOrEmpty(displayToRemove))
                {
                    for (int i = 0; i < UnpaidLoansDisplay.Count; i++)
                    {
                        if (UnpaidLoansDisplay[i] == displayToRemove)
                        {
                            UnpaidLoansDisplay.RemoveAt(i);
                            break;
                        }
                    }
                }
                
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
            // Format: "IBAN1 - EUR - 1000"
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

        // Load data from the service
        private void LoadData()
        {
            try
            {
                // Clear existing collections
                Loans.Clear();
                BankAccounts.Clear();
                UnpaidLoans.Clear();
                UnpaidLoansDisplay.Clear();
                
                // Get loans from service
                var userLoans = _loanService.GetUserLoans(userID);
                foreach (var loan in userLoans)
                {
                    Loans.Add(loan);
                }
                
                // Get unpaid loans for display
                var unpaidLoans = _loanService.GetUnpaidLoans(userID);
                foreach (var loan in unpaidLoans)
                {
                    UnpaidLoans.Add(loan);
                    string display = $"{loan.Currency} - {loan.AmountToPay:F2} - {loan.DateDeadline:d}";
                    UnpaidLoansDisplay.Add(display);
                }
                
                // Get bank accounts from service
                var formattedAccounts = _loanService.GetFormattedBankAccounts(userID);
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

        // Reload data from service
        public void RefreshData()
        {
            LoadData();
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
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