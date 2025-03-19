using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.Domain
{
    public class Loan
    {
        public int LoanID { get; set; }
        public int UserID { get; set; }
        public double Amount { get; set; }
        public double AmountToPay
        {
            get { return Amount * (1 + TaxPercentage/100); }
        }
        public string Currency { get; set; }
        public DateTime DateTaken { get; set; }
        public DateTime DateDeadline { get; set; }
        public DateTime? DatePaid { get; set; } // Nullable for unpaid loans
        public float TaxPercentage { get; set; }
        public int NumberMonths { get; set; }
        public string State { get; private set; } // "paid" or "unpaid"


        public Loan(int loanID, int userID, double amount,
            string currency, DateTime dateTaken, DateTime? datePaid,
            float taxPercentage, int numberMonths, string state)
        {
            LoanID = loanID;
            UserID = userID;
            Amount = amount;
            Currency = currency;
            DateTaken = dateTaken;
            DateDeadline = DateTaken.AddMonths(numberMonths);
            DatePaid = datePaid;
            TaxPercentage = taxPercentage;
            NumberMonths = numberMonths;
            State = state;
        }

        public void MarkAsPaid()
        {
            DatePaid = DateTime.Now;
            State = "paid";
        }
    }
}
