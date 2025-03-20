using System;

namespace LoanShark.Domain
{
    public class BankAccount
    {
        public string IBAN { get; set; } 
        public int UserId { get; set; } 
        public decimal Balance { get; set; } 
        public string Currency { get; set; } 
        public string? CustomName { get; set; }  
        public decimal DailyLimit { get; set; } 
        public decimal MaxPerTransaction { get; set; } 
        public int MaxNrTransactionsDaily { get; set; }
        public bool Blocked { get; set; }

        // public BankAccount() {} careful with this, when merging

        public BankAccount(string iban, int userId, decimal balance, string currency,
                           string? customName, decimal dailyLimit, decimal maxPerTransaction,
                           int maxNrTransactionsDaily, bool blocked)
        {
            IBAN = iban;
            UserId = userId;
            Balance = balance;
            Currency = currency;
            CustomName = customName;
            DailyLimit = dailyLimit;
            MaxPerTransaction = maxPerTransaction;
            MaxNrTransactionsDaily = maxNrTransactionsDaily;
            Blocked = blocked;
        }
    }

    public class BankAccountMessage : BankAccount // polymorphism to handle the case when the user has no bank accounts in the flip view
    {
        public BankAccountMessage(string iban, string customName)
            : base(iban, 0, 0, "EUR", customName, 0, 0, 0, false)
        {
            IBAN = iban;
            CustomName = customName;
        }
    }
}
