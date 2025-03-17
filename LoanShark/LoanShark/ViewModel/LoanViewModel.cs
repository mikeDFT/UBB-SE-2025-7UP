using LoanShark.Domain;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Diagnostics;


namespace LoanShark.ViewModel
{
    public class LoanViewModel : INotifyPropertyChanged
    {
        private int userID;
        private string selectedBankAccount;
        private Loan selectedLoan;
        private float amount;
        private int selectedMonths;
        private string errorMessage;
        private string payErrorMessage;
        private string currentPage;

        public ObservableCollection<Loan> Loans { get; set; }
        public ObservableCollection<string> BankAccounts { get; set; }
        public ObservableCollection<int> Months { get; set; }
        public ObservableCollection<Loan> UnpaidLoans { get; set; }

        public string SelectedBankAccount
        {
            get => selectedBankAccount;
            set
            {
                selectedBankAccount = value;
                OnPropertyChanged(nameof(SelectedBankAccount));
                UpdateAmountToPay();
            }
        }

        public Loan SelectedLoan
        {
            get => selectedLoan;
            set
            {
                selectedLoan = value;
                OnPropertyChanged(nameof(SelectedLoan));
            }
        }

        public float Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged(nameof(Amount));
                UpdateAmountToPay();
            }
        }

        public int SelectedMonths
        {
            get => selectedMonths;
            set
            {
                selectedMonths = value;
                OnPropertyChanged(nameof(SelectedMonths));
                UpdateAmountToPay();
            }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public string PayErrorMessage
        {
            get => payErrorMessage;
            set
            {
                payErrorMessage = value;
                OnPropertyChanged(nameof(PayErrorMessage));
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

        public event PropertyChangedEventHandler PropertyChanged;

        public LoanViewModel(int userID)
        {
            this.userID = userID;
            Loans = new ObservableCollection<Loan>(GetUnpaidLoans().OrderBy(loan => loan.DateTaken));
            BankAccounts = new ObservableCollection<string>(GetBankAccounts());
            Months = new ObservableCollection<int> { 6, 12, 24, 36 };
            UnpaidLoans = new ObservableCollection<Loan>(GetUnpaidLoans());

            TakeLoanCommand = new RelayCommand(TakeLoan);
            PayLoanCommand = new RelayCommand(PayLoan);
            BackCommand = new RelayCommand(Back);
            GoToTakeLoanPageCommand = new RelayCommand(() => CurrentPage = "TakeLoanPage");
            GoToPayLoanPageCommand = new RelayCommand(() => CurrentPage = "PayLoanPage");

            CurrentPage = "MainLoansPage";
            UpdatePageVisibility();
            Debug.WriteLine(MainLoansPageVisibility.ToString());
            Debug.WriteLine(TakeLoanPageVisibility.ToString());
            Debug.WriteLine(PayLoanPageVisibility.ToString());
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
            var Loans = new List<Loan>();
            for (int i = 0; i < 5; i++)
                Loans.Add(new Loan(i, i, i.ToString(), i, i));
            return Loans;
        }

        private void UpdateAmountToPay()
        {
            // Implement logic to update amount to pay based on selected bank account, amount, and months
        }

        private void TakeLoan()
        {
            // Implement logic to take a loan
        }

        private void PayLoan()
        {
            // Implement logic to pay a loan
        }

        private void Back()
        {
            CurrentPage = "MainLoansPage";
        }

        private void UpdatePageVisibility()
        {
            MainLoansPageVisibility = CurrentPage == "MainLoansPage" ? Visibility.Visible : Visibility.Collapsed;
            TakeLoanPageVisibility = CurrentPage == "TakeLoanPage" ? Visibility.Visible : Visibility.Collapsed;
            PayLoanPageVisibility = CurrentPage == "PayLoanPage" ? Visibility.Visible : Visibility.Collapsed;
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