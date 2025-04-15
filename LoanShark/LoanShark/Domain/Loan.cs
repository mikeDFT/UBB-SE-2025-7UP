using System;

namespace LoanShark.Domain
{
    public class Loan
    {
        public int LoanID { get; set; }
        public int UserID { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountToPay
        {
            get { return Amount * (1 + (TaxPercentage / 100)); }
        }
        public string Currency { get; set; }
        public DateTime DateTaken { get; set; }
        public DateTime DateDeadline { get; set; }
        public DateTime? DatePaid { get; set; } // Nullable for unpaid loans
        public decimal TaxPercentage { get; set; }
        public int NumberMonths { get; set; }
        public string State { get; private set; } // "paid" or "unpaid"

        public Loan(int loanID, int userID, decimal amount,
            string currency, DateTime dateTaken, DateTime? datePaid,
            decimal taxPercentage, int numberMonths, string state)
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
