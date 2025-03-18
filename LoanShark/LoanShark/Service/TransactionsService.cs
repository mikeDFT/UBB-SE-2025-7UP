using LoanShark.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanShark.Domain;

namespace LoanShark.Service
{
    public class TransactionsService
    {
        public async Task<string> ProcessPaymentAsync(string iban, string sumOfMoney, string details)
        {
            if (string.IsNullOrWhiteSpace(iban) || string.IsNullOrWhiteSpace(sumOfMoney))
            {
                return "Please fill all required fields.";
            }

            if (!decimal.TryParse(sumOfMoney, out decimal amount) || amount <= 0)
            {
                return "Invalid amount.";
            }

            await Task.Delay(1000); // Simulate a delay for async processing

            return $"Payment of {amount} processed to {iban}.";
        }

        public async Task<List<CurrencyExchangeRate>> GetCurrencyExchangeRatesAsync()
        {
            await Task.Delay(500); // Simulate network/database delay

            var random = new Random();
            return new List<CurrencyExchangeRate>
            {
                new CurrencyExchangeRate { FromCurrency = "USD", ToCurrency = "EUR", ExchangeRate = Math.Round((decimal)(0.80 + random.NextDouble() * 0.10), 4) },
                new CurrencyExchangeRate { FromCurrency = "EUR", ToCurrency = "USD", ExchangeRate = Math.Round((decimal)(1.10 + random.NextDouble() * 0.10), 4) },
                new CurrencyExchangeRate { FromCurrency = "USD", ToCurrency = "GBP", ExchangeRate = Math.Round((decimal)(0.70 + random.NextDouble() * 0.10), 4) },
                new CurrencyExchangeRate { FromCurrency = "GBP", ToCurrency = "USD", ExchangeRate = Math.Round((decimal)(1.30 + random.NextDouble() * 0.10), 4) },
                new CurrencyExchangeRate { FromCurrency = "USD", ToCurrency = "JPY", ExchangeRate = Math.Round((decimal)(110.00 + random.NextDouble() * 10.00), 2) },
                new CurrencyExchangeRate { FromCurrency = "JPY", ToCurrency = "USD", ExchangeRate = Math.Round((decimal)(0.009 + random.NextDouble() * 0.002), 5) }
            };
        }
    }
}
