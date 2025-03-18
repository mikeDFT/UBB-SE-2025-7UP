using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
