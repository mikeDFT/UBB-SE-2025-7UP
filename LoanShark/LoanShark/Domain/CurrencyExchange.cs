using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.Domain
{
    public class CurrencyExchange
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public float ExchangeRate { get; set; }

        public CurrencyExchange(string fromCurrency, string toCurrency, float exchangeRate)
        {
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
            ExchangeRate = exchangeRate;
        }
    }
}
