using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;

namespace LoanShark.Domain
{
    public class BankAccount
    {
        public string iban { get; set; }
        public string currency { get; set; }
        public double amount { get; set; }
        public string id_user{ get; set; }
        public string customerName { get; set; }
        public double dailyLimit { get; set; }
        public double maxPerTransaction { get; set; }
        public int maxTransactionsPerDay { get; set; }
        public bool blocked { get; set; }

        public BankAccount(string iban, string currency, double amount, string id_user, string customerName,
                            double dailyLimit, double maxPerTransaction, int maxTransactionsPerDay, bool blocked)
        {
            this.iban = iban;
            this.currency = currency;
            this.amount = amount;
            this.id_user = id_user;
            this.customerName = customerName;
            this.dailyLimit = dailyLimit;
            this.maxPerTransaction = maxPerTransaction;
            this.blocked = blocked;
        }
    }
}
