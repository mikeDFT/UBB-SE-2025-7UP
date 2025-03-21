using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LoanShark.Service;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using System;

namespace LoanShark.ViewModel
{
    public partial class SendMoneyViewModel : ObservableObject
    {
        private readonly TransactionsService _transactionService;

        [ObservableProperty]
        private string iban;

        [ObservableProperty]
        private string sumOfMoney;

        [ObservableProperty]
        private string details;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private Visibility isErrorVisible = Visibility.Collapsed; 

        public ICommand PayCommand { get; }
        public ICommand CloseCommand { get; }

        public Action CloseAction { get; set; }

        public SendMoneyViewModel()
        {
            _transactionService = new TransactionsService();

            PayCommand = new AsyncRelayCommand(ProcessPaymentAsync);
            CloseCommand = new RelayCommand(CloseWindow);
        }

        private async Task ProcessPaymentAsync()
        {
            Debug.WriteLine($"DEBUG: Iban = '{Iban}', SumOfMoney = '{SumOfMoney}'");

            // Reset error message
            ErrorMessage = string.Empty;
            IsErrorVisible = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(Iban) || string.IsNullOrWhiteSpace(SumOfMoney))
            {
                Debug.WriteLine("DEBUG: Empty IBAN or Amount");
                ErrorMessage = "IBAN and Amount are required.";
                IsErrorVisible = Visibility.Visible;
                return;
            }

            if (!decimal.TryParse(SumOfMoney, out decimal amount) || amount <= 0)
            {
                Debug.WriteLine($"DEBUG: Invalid amount entered: {SumOfMoney}");
                ErrorMessage = "Invalid amount.";
                IsErrorVisible = Visibility.Visible;
                return;
            }

            Debug.WriteLine($"DEBUG: Sending money from {_transactionService.GetCurrentUserIBAN()} to {Iban}, Amount: {amount}");

            string result = await _transactionService.AddTransaction(
                _transactionService.GetCurrentUserIBAN(), Iban, amount, Details
            );

            if (result != "Transaction successful!")
            {
                Debug.WriteLine($"DEBUG: Transaction failed - {result}");
                ErrorMessage = result;
                IsErrorVisible = Visibility.Visible;
                return;
            }

            Debug.WriteLine("DEBUG: Transaction successful!");
            await ShowMessage("Payment Successful!");

            ResetFields();
        }

        private void ResetFields()
        {
            Debug.WriteLine("DEBUG: Resetting input fields...");

            Iban = string.Empty;
            OnPropertyChanged(nameof(Iban));

            SumOfMoney = string.Empty;
            OnPropertyChanged(nameof(SumOfMoney));

            Details = string.Empty;
            OnPropertyChanged(nameof(Details));

            ErrorMessage = string.Empty;
            OnPropertyChanged(nameof(ErrorMessage));

            IsErrorVisible = Visibility.Collapsed;
            OnPropertyChanged(nameof(IsErrorVisible));

            Debug.WriteLine("DEBUG: Reset complete.");
        }

        private async Task ShowMessage(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Transaction Result",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = App.MainWindow.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private void CloseWindow()
        {
            CloseAction?.Invoke();
        }
    }
}
