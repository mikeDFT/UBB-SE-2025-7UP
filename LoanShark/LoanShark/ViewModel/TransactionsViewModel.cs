using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LoanShark.View;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using Microsoft.UI.Xaml;

namespace LoanShark.ViewModel
{
    public class TransactionsViewModel : ObservableObject
    {
        public ICommand CloseCommand { get; }
        public ICommand SendMoneyCommand { get; }
        public ICommand PayLoanCommand { get; }
        public ICommand CurrencyExchangeCommand { get; }
        public ICommand TransactionsHistoryCommand { get; }

        public Action CloseAction { get; set; }

        public TransactionsViewModel()
        {
            CloseCommand = new RelayCommand(CloseWindow);
            SendMoneyCommand = new RelayCommand(OpenSendMoneyWindow);
            PayLoanCommand = new RelayCommand(OpenPayLoanWindow);
            CurrencyExchangeCommand = new RelayCommand(OpenCurrencyExchangeWindow);
            TransactionsHistoryCommand = new RelayCommand(() => { /* Implement Transactions History */ });
        }

        private void OpenSendMoneyWindow()
        {
            OpenChildWindow(new SendMoneyView());
        }

        private void OpenPayLoanWindow()
        {
            OpenChildWindow(new LoanView(123));
        }

        private void OpenCurrencyExchangeWindow()
        {
            OpenChildWindow(new CurrencyExchangeTableView());
        }

        private void OpenChildWindow(Window childWindow)
        {
            childWindow.Activate();
        }

        private void CloseWindow()
        {
            CloseAction?.Invoke();
        }
    }
}