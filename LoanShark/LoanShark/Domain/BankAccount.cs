namespace LoanShark.Domain
{
    public class BankAccount
    {
        /// <summary>
        /// The International Bank Account Number (IBAN) of the account
        /// </summary>
        public string iban { get; set; }

        /// <summary>
        /// The currency of the account (e.g., USD, EUR, RON)
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// The account balance (amount of money in the account)
        /// </summary>
        public double sold { get; set; }

        /// <summary>
        /// Indicates whether the account is blocked/frozen
        /// </summary>
        public bool blocked { get; set; }

        /// <summary>
        /// The ID of the user who owns the account
        /// </summary>
        public int userID { get; set; }

        /// <summary>
        /// The custom name assigned to the account
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The maximum amount that can be transacted in a day
        /// </summary>
        public double dailyLimit { get; set; }

        /// <summary>
        /// The maximum amount that can be transacted in a single transaction
        /// </summary>
        public double maximumPerTransaction { get; set; }

        /// <summary>
        /// The maximum number of transactions allowed per day
        /// </summary>
        public int maximumNrTransactions { get; set; }

        /// <summary>
        /// Initializes a new instance of the BankAccount class with the specified parameters
        /// </summary>
        /// <param name="iban">The IBAN of the account</param>
        /// <param name="currency">The currency of the account</param>
        /// <param name="sold">The initial balance</param>
        /// <param name="blocked">Whether the account is blocked</param>
        /// <param name="userID">The ID of the user who owns the account</param>
        /// <param name="name">The custom name of the account</param>
        /// <param name="dailyLimit">The daily transaction limit</param>
        /// <param name="maximumPerTransaction">The maximum amount per transaction</param>
        /// <param name="maximumNrTransactions">The maximum number of transactions per day</param>
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

        /// <summary>
        /// Toggles the blocked status of the account
        /// </summary>
        public void toggleBlocked()
        {
            blocked = !blocked;
        }

    }
}
