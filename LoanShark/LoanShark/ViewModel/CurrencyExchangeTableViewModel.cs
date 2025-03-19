using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LoanShark.Service;
using LoanShark.Domain;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LoanShark.Domain;

namespace LoanShark.ViewModel
{
    public class CurrencyExchangeTableViewModel : ObservableObject
    {
        private readonly Page _page;
        private readonly TransactionsService _transactionService;

        public ObservableCollection<CurrencyExchange> ExchangeRates { get; } = new ObservableCollection<CurrencyExchange>();

        public ICommand BackCommand { get; }

        public CurrencyExchangeTableViewModel(Page page)
        {
            _page = page;
            _transactionService = new TransactionsService();
            LoadExchangeRatesAsync();

            BackCommand = new RelayCommand(NavigateBack);
        }

        private async void LoadExchangeRatesAsync()
        {
            var rates = await _transactionService.GetCurrencyExchangeRatesAsync();

            ExchangeRates.Clear();
            foreach (var rate in rates)
            {
                ExchangeRates.Add(rate);
            }
        }

        private void NavigateBack()
        {
            if (_page.Frame.CanGoBack)
            {
                _page.Frame.GoBack();
            }
        }
    }
}
