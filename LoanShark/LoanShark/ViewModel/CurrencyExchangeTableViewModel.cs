using CommunityToolkit.Mvvm.ComponentModel;
using LoanShark.Service;
using LoanShark.Domain;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;

namespace LoanShark.ViewModel
{
    public class CurrencyExchangeTableViewModel : ObservableObject
    {
        private readonly TransactionsService _transactionService;

        public ObservableCollection<CurrencyExchange> ExchangeRates { get; } = new ObservableCollection<CurrencyExchange>();

        public ICommand CloseCommand { get; }

        public Action CloseAction { get; set; }

        public CurrencyExchangeTableViewModel()
        {
            _transactionService = new TransactionsService();
            LoadExchangeRatesAsync();

            CloseCommand = new RelayCommand(CloseWindow);
        }

        private async void LoadExchangeRatesAsync()
        {
            var rates = await _transactionService.GetAllCurrencyExchangeRates();

            ExchangeRates.Clear();
            foreach (var rate in rates)
            {
                ExchangeRates.Add(rate);
            }
        }

        private void CloseWindow()
        {
            CloseAction?.Invoke();
        }
    }
}
