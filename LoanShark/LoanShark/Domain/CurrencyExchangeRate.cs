using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.Domain
{
    public class CurrencyExchangeRate
    {
        public string FromCurrency { get; set; } 
        public string ToCurrency { get; set; }  
        public decimal ExchangeRate { get; set; } 
    }
}
