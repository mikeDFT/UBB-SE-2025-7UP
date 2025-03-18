using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows.Input;

namespace LoanShark.ViewModel
{
    public class TransactionsViewModel
    {
        public ICommand BackCommand { get; }
        public ICommand SendMoneyCommand { get; }
        public ICommand PayLoanCommand { get; }
        public ICommand CurrencyExchangeCommand { get; }
        public ICommand TransactionsHistoryCommand { get; }

        public TransactionsViewModel()
        {
            BackCommand = new RelayCommand(() => { /* Navigate Back (not implemented) */ });
            SendMoneyCommand = new RelayCommand(() => { /* Navigate to Send Money (not implemented) */ });
            PayLoanCommand = new RelayCommand(() => { /* Navigate to Pay Loan (not implemented) */ });
            CurrencyExchangeCommand = new RelayCommand(() => { /* Navigate to Currency Exchange (not implemented) */ });
            TransactionsHistoryCommand = new RelayCommand(() => { /* Navigate to Transactions History (not implemented) */ });
        }
    }
}
