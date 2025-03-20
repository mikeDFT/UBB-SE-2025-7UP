using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LoanShark.Service;
using System.Windows.Input;
using System.Threading.Tasks;
using System;

namespace LoanShark.ViewModel
{
    public class SendMoneyViewModel : ObservableObject
    {
        private readonly TransactionsService _transactionService;

        public string IBAN { get; set; }
        public string SumOfMoney { get; set; }
        public string Details { get; set; }

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
            decimal amount;
            try
            {
                amount = Convert.ToDecimal(SumOfMoney);
                string result = await _transactionService.AddTransaction(_transactionService.GetCurrentUserIBAN(), IBAN, amount, Details);
                await ShowMessage(result);
            }
            catch (Exception)
            {
                await ShowMessage("Invalid amount.");
                return;
            }
        }

        private async Task ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        private void CloseWindow()
        {
            CloseAction?.Invoke();
        }
    }
}
