namespace LoanShark.Domain
{
    public class CurrencyExchange
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal ExchangeRate { get; set; }

        public CurrencyExchange(string fromCurrency, string toCurrency, decimal exchangeRate)
        {
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
            ExchangeRate = exchangeRate;
        }
    }
}
