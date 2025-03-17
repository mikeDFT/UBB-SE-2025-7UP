using LoanShark.Domain;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LoanShark.ViewModel;

namespace LoanShark
{
    public sealed partial class LoanView : Window
    {
        public ObservableCollection<Loan> Loans { get; set; }

        private LoanViewModel loansViewModel;

        public LoanView(int userID)
        {
            this.InitializeComponent();
            loansViewModel = new LoanViewModel(userID);
            Loans = new ObservableCollection<Loan>(GetUnpaidLoans().OrderBy(loan => loan.DateTaken));
            LoansListView.ItemsSource = Loans;
            InitializeTakeLoanPage();
            InitializePayLoanPage();
        }

        private void InitializeTakeLoanPage()
        {
            // Populate BankAccountComboBox with aggregated bank account information
            BankAccountComboBox.ItemsSource = GetBankAccounts();

            // Populate MonthsComboBox with possible number of months
            MonthsComboBox.ItemsSource = new List<int> { 6, 12, 24, 36 };
        }

        private void InitializePayLoanPage()
        {
            // Populate PayBankAccountComboBox with aggregated bank account information
            PayBankAccountComboBox.ItemsSource = GetBankAccounts();

            // Populate LoanToPayComboBox with unpaid loans
            LoanToPayComboBox.ItemsSource = GetUnpaidLoans().Select(loan => new
            {
                Display = $"{loan.Currency} - {loan.AmountToPay} - {loan.DateDeadline}",
                Loan = loan
            }).ToList();
            LoanToPayComboBox.DisplayMemberPath = "Display";
            LoanToPayComboBox.SelectedValuePath = "Loan";
        }

        private IEnumerable<string> GetBankAccounts()
        {
            // Assuming you have a method to get all bank accounts
            return new List<string>
            {
                "IBAN1 - EUR - 1000",
                "IBAN2 - USD - 2000",
                "IBAN3 - GBP - 3000"
            };
        }

        private IEnumerable<Loan> getGeneratedLoans()
        {
            var Loans = new List<Loan>(); // GetAllLoans()
            for (int i = 0; i < 5; i++)
                Loans.Add(new Loan(i, i, i.ToString(), i, i));

            return Loans;
        }

        private IEnumerable<Loan> GetAllLoans()
        {
            return getGeneratedLoans();
        }

        private IEnumerable<Loan> GetUnpaidLoans()
        {
            // Assuming you have a method to get all loans
            var allLoans = GetAllLoans();
            return allLoans.Where(loan => loan.State == "unpaid");
        }

        private void GoToTakeLoanPageButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage("TakeLoanPage");
        }

        private void GoToPayLoanPageButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage("PayLoanPage");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage("MainLoansPage");
        }

        private void ShowPage(string pageName)
        {
            MainLoansPage.Visibility = pageName == "MainLoansPage" ? Visibility.Visible : Visibility.Collapsed;
            TakeLoanPage.Visibility = pageName == "TakeLoanPage" ? Visibility.Visible : Visibility.Collapsed;
            PayLoanPage.Visibility = pageName == "PayLoanPage" ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ClearTakeLoanPage()
        {
            BankAccountComboBox.SelectedIndex = -1;
            AmountTextBox.Text = string.Empty;
            MonthsComboBox.SelectedIndex = -1;
            TaxPercentageTextBlock.Text = "Tax Percentage: N/A";
            AmountToPayTextBlock.Text = "Amount to Pay: N/A";
            ErrorMessageTextBlock.Text = string.Empty;
        }
        private void ClearPayLoanPage()
        {
            PayBankAccountComboBox.SelectedIndex = -1;
            LoanToPayComboBox.SelectedIndex = -1;
            PayErrorMessageTextBlock.Text = string.Empty;
        }

        private void BankAccountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAmountToPay();
        }

        private void MonthsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAmountToPay();
        }

        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAmountToPay();
        }

        private void UpdateAmountToPay()
        {
            if (BankAccountComboBox.SelectedIndex == -1 || string.IsNullOrEmpty(AmountTextBox.Text) || MonthsComboBox.SelectedIndex == -1)
            {
                TaxPercentageTextBlock.Text = "Tax Percentage: N/A";
                AmountToPayTextBlock.Text = "Amount to Pay: N/A";
                return;
            }

            if (float.TryParse(AmountTextBox.Text, out float amount) && amount > 0 && amount <= 1000000)
            {
                int months = (int)MonthsComboBox.SelectedItem;
                float taxPercentage = CalculateTaxPercentage(months);
                float amountToPay = amount + (amount * taxPercentage / 100);

                TaxPercentageTextBlock.Text = $"Tax Percentage: {taxPercentage}%";
                AmountToPayTextBlock.Text = $"Amount to Pay: {amountToPay}";
            }
            else
            {
                TaxPercentageTextBlock.Text = "Tax Percentage: N/A";
                AmountToPayTextBlock.Text = "Amount to Pay: N/A";
            }
        }

        private float CalculateTaxPercentage(int months)
        {
            // Implement your tax percentage calculation logic here
            return months * 0.1f; // Example calculation
        }

        private void TakeLoanButton_Click(object sender, RoutedEventArgs e)
        {
            if (BankAccountComboBox.SelectedIndex == -1 || string.IsNullOrEmpty(AmountTextBox.Text) || MonthsComboBox.SelectedIndex == -1)
            {
                ErrorMessageTextBlock.Text = "Invalid data provided";
                return;
            }

            if (float.TryParse(AmountTextBox.Text, out float amount) && amount > 0 && amount <= 1000000)
            {
                // Save the loan and update the bank account balance
                SaveLoan();
                ShowPage("MainLoansPage");
                ClearTakeLoanPage();
            }
            else
            {
                ErrorMessageTextBlock.Text = "Invalid data provided";
            }
        }

        private void PayLoanButton_Click(object sender, RoutedEventArgs e)
        {
            if (PayBankAccountComboBox.SelectedIndex == -1 || LoanToPayComboBox.SelectedIndex == -1)
            {
                PayErrorMessageTextBlock.Text = "Invalid data provided";
                return;
            }

            var selectedBankAccount = PayBankAccountComboBox.SelectedItem.ToString();
            var selectedLoan = (Loan)LoanToPayComboBox.SelectedValue;

            if (HasSufficientFunds(selectedBankAccount, selectedLoan))
            {
                PayLoan(selectedBankAccount, selectedLoan);
                ShowPage("MainLoansPage");
                ClearPayLoanPage();
            }
            else
            {
                PayErrorMessageTextBlock.Text = "Not enough funds";
            }
        }

        private bool HasSufficientFunds(string bankAccount, Loan loan)
        {
            // Implement your logic to check if the bank account has sufficient funds to pay the loan
            return true; // Example logic
        }

        private void PayLoan(string bankAccount, Loan loan)
        {
            // Implement your logic to pay the loan and update the bank account balance
        }

        private void SaveLoan()
        {
            // Implement your loan saving logic here
        }
    }
}