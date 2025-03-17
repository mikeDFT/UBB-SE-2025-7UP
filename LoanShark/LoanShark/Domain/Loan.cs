using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.Domain
{
    public class Loan
    {
        private static int _loanCounter = 1; // to change

        public int LoanID { get; private set; }
        public int UserID { get; set; }
        public double Amount { get; set; }
        public double AmountToPay
        {
            get { return Amount * TaxPercentage; }
        }
        public string Currency { get; set; }
        public DateTime DateTaken { get; set; }
        public DateTime DateDeadline { get; set; }
        public DateTime? DatePaid { get; set; } // Nullable for unpaid loans
        public float TaxPercentage { get; set; }
        public int NumberMonths { get; set; }
        public string State { get; private set; } // "paid" or "unpaid"


        public Loan(int userID, double amount, string currency, float taxPercentage, int numberMonths)
        {
            LoanID = _loanCounter++;
            UserID = userID;
            Amount = amount;
            Currency = currency;
            TaxPercentage = taxPercentage;
            NumberMonths = numberMonths;
            DateTaken = DateTime.Now;
            DateDeadline = DateTaken.AddMonths(numberMonths);
            State = "unpaid";
        }

        public void MarkAsPaid()
        {
            DatePaid = DateTime.Now;
            State = "paid";
        }
    }
}
