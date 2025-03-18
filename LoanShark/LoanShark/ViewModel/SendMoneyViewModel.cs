using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;
using LoanShark.Service;
using System.Threading.Tasks;
using System;

namespace LoanShark.ViewModel
{
    public class SendMoneyViewModel : ObservableObject
    {
        private readonly Page _page;
        private readonly TransactionsService _transactionService;

        public string IBAN { get; set; }
        public string SumOfMoney { get; set; }
        public string Details { get; set; }

        public ICommand PayCommand { get; }
        public ICommand BackCommand { get; }

        public SendMoneyViewModel()
        {
            _transactionService = new TransactionsService();
        }


        public SendMoneyViewModel(Page page)
        {
            _page = page;
            _transactionService = new TransactionsService();

            PayCommand = new AsyncRelayCommand(ProcessPaymentAsync);
            BackCommand = new RelayCommand(NavigateBack);
        }

        private async Task ProcessPaymentAsync()
        {
            string result = await _transactionService.ProcessPaymentAsync(IBAN, SumOfMoney, Details);
            ShowMessage(result);
        }

        private void NavigateBack()
        {
            if (_page.Frame.CanGoBack)
            {
                _page.Frame.GoBack();
            }
        }

        private async void ShowMessage(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Transaction",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = _page.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}
