using System.Windows.Input;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LoanShark.View;
using Microsoft.UI.Xaml;

namespace LoanShark.ViewModel
{
    public class TransactionsViewModel : ObservableObject
    {
        public ICommand CloseCommand { get; }
        public ICommand SendMoneyCommand { get; }
        public ICommand PayLoanCommand { get; }
        public ICommand CurrencyExchangeCommand { get; }

        public Action CloseAction { get; set; }

        public TransactionsViewModel()
        {
            CloseCommand = new RelayCommand(CloseWindow);
            SendMoneyCommand = new RelayCommand(OpenSendMoneyWindow);
            PayLoanCommand = new RelayCommand(OpenPayLoanWindow);
            CurrencyExchangeCommand = new RelayCommand(OpenCurrencyExchangeWindow);
        }

        private void OpenSendMoneyWindow()
        {
            OpenChildWindow(new SendMoneyView());
        }

        private void OpenPayLoanWindow()
        {
            OpenChildWindow(new LoanView());
        }

        private void OpenCurrencyExchangeWindow()
        {
            OpenChildWindow(new CurrencyExchangeTableView());
        }

        private void OpenChildWindow(Window childWindow)
        {
            CloseWindow();
            childWindow.Activate();
        }

        private void CloseWindow()
        {
            CloseAction?.Invoke();
        }
    }
}