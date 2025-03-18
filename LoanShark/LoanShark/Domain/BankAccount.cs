namespace LoanShark.Domain
{
    public class BankAccount
    {
        public int UserID { get; private set; }
        public string IBAN { get; private set; }
        public string Currency { get; private set; }
        public float Balance { get; set; }

        public BankAccount(int userId, string iban, string currency, float balance)
        {
            UserID = userId;
            IBAN = iban;
            Currency = currency;
            Balance = balance;
        }
    }
}