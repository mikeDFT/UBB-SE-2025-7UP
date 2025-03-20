using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.Domain
{
    class BankAccount
    {
        public string iban {  get; set; }
        public string currency { get; set; }
        public double sold {  get; set; }
        public bool blocked { get; set; }
        public int userID { get; set; }
        public string name {  get; set; }
        public double dailyLimit {  get; set; }
        public double maximumPerTransaction {  get; set; }
        public int maximumNrTransactions { get; set; }

        public BankAccount(string iban, string currency, double sold, bool blocked, int userID, string name, double dailyLimit, double maximumPerTransaction, int maximumNrTransactions)
        {
            this.iban = iban;
            this.currency = currency;
            this.sold = sold;
            this.blocked = blocked;
            this.userID = userID;
            this.name = name;
            this.dailyLimit = dailyLimit;
            this.maximumPerTransaction = maximumPerTransaction;
            this.maximumNrTransactions = maximumNrTransactions;
        }

        public void toggleBlocked()
        {
            blocked = !blocked;
        }
    }
}
