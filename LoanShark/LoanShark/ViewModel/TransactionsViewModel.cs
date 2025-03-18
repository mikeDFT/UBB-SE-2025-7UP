using CommunityToolkit.Mvvm.Input;
using LoanShark.View;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Windows.Input;

namespace LoanShark.ViewModel
{
    public class TransactionsViewModel
    {
        private readonly Page _page;

        public ICommand BackCommand { get; }
        public ICommand SendMoneyCommand { get; }
        public ICommand PayLoanCommand { get; }
        public ICommand CurrencyExchangeCommand { get; }
        public ICommand TransactionsHistoryCommand { get; }

        public TransactionsViewModel() { }

        public TransactionsViewModel(Page page)
        {
            _page = page;
            BackCommand = new RelayCommand(() => { /* Navigate Back (not implemented) */ });
            SendMoneyCommand = new RelayCommand(() =>  _page.Frame.Navigate(typeof(SendMoneyView)));
            PayLoanCommand = new RelayCommand(() => { /* Navigate to Pay Loan (not implemented) */ });
            CurrencyExchangeCommand = new RelayCommand(() => _page.Frame.Navigate(typeof(CurrencyExchangeTableView)));
            TransactionsHistoryCommand = new RelayCommand(() => { /* Navigate to Transactions History (not implemented) */ });
        }
    }
}
