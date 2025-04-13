namespace LoanShark.Domain
{
    public class BankAccount
    {
        /// <summary>
        /// The International Bank Account Number (IBAN) of the account
        /// </summary>
        public string Iban { get; set; }

        /// <summary>
        /// The currency of the account (e.g., USD, EUR, RON)
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The account balance (amount of money in the account)
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// Indicates whether the account is blocked/frozen
        /// </summary>
        public bool Blocked { get; set; }

        /// <summary>
        /// The ID of the user who owns the account
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// The custom name assigned to the account
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The maximum amount that can be transacted in a day
        /// </summary>
        public decimal DailyLimit { get; set; }

        /// <summary>
        /// The maximum amount that can be transacted in a single transaction
        /// </summary>
        public decimal MaximumPerTransaction { get; set; }

        /// <summary>
        /// The maximum number of transactions allowed per day
        /// </summary>
        public int MaximumNrTransactions { get; set; }

        /// <summary>
        /// Initializes a new instance of the BankAccount class with the specified parameters
        /// </summary>
        /// <param name="iban">The IBAN of the account</param>
        /// <param name="currency">The currency of the account</param>
        /// <param name="balance">The initial balance</param>
        /// <param name="blocked">Whether the account is blocked</param>
        /// <param name="userID">The ID of the user who owns the account</param>
        /// <param name="name">The custom name of the account</param>
        /// <param name="dailyLimit">The daily transaction limit</param>
        /// <param name="maximumPerTransaction">The maximum amount per transaction</param>
        /// <param name="maximumNrTransactions">The maximum number of transactions per day</param>
        public BankAccount(string iban, string currency, decimal balance, bool blocked, int userID, string name, decimal dailyLimit, decimal maximumPerTransaction, int maximumNrTransactions)
        {
            this.Iban = iban;
            this.Currency = currency;
            this.Balance = balance;
            this.Blocked = blocked;
            this.UserID = userID;
            this.Name = name;
            this.DailyLimit = dailyLimit;
            this.MaximumPerTransaction = maximumPerTransaction;
            this.MaximumNrTransactions = maximumNrTransactions;
        }

        /// <summary>
        /// Toggles the blocked status of the account
        /// </summary>
        public void ToggleBlocked()
        {
            Blocked = !Blocked;
        }
    }

    public class BankAccountMessage : BankAccount // polymorphism to handle the case when the user has no bank accounts in the flip view
    {
        public BankAccountMessage(string iban, string name)
            : base(iban, "EUR", 0, false, 0, name, 0, 0, 0)
        {
            this.Iban = iban;
            this.Name = name;
        }
    }
}
