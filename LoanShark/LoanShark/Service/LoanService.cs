using LoanShark.Domain;
using LoanShark.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;

namespace LoanShark.Service
{
    public class LoanService
    {
        // Simulated database for demonstration purposes
        private static List<Loan> _loans = new List<Loan>();
        private readonly ILoanRepository _loanRepository;

        // Currency conversion rates (relative to EUR)
        private static readonly Dictionary<string, float> _currencyRates = new Dictionary<string, float>
        {
            { "EUR", 1.0f },
            { "USD", 1.1f },
            { "GBP", 0.85f },
            // Add more currencies as needed
        };

        public LoanService()
        {
            _loanRepository = new LoanRepository();

            //InitializeSampleData();
        }

        // Get all loans for a specific user
        public List<Loan> GetUserLoans(int userId)
        {
            Debug.WriteLine(_loans.Count);
            Debug.WriteLine(_loans.Where(loan => loan.UserID == userId).ToList().Count);

            return _loanRepository.GetLoansByUserId(userId);
        }

        // Get only unpaid loans for a user
        public List<Loan> GetUnpaidUserLoans(int userId)
        {
            return GetUserLoans(userId).Where(
                loan => loan.State == "unpaid").ToList();
        }

        // Create a new loan with the specified parameters
        public Loan TakeLoan(int userId, float amount, string currency, string accountIBAN, int months)
        {
            // Validate loan parameters
            if (ValidateLoanRequest(amount, months) != "success")
            {
                throw new ArgumentException("Invalid loan parameters");
            }

            // Calculate tax percentage
            float taxPercentage = CalculateTaxPercentage(months);

            // TODO: give money

            // Create a new loan
            var loan = new Loan(
                -1,
                userId,
                amount,
                currency,
                DateTime.Now,
                null,
                taxPercentage,
                months,
                "unpaid"
            );
            
            // Add to our "database"
            _loans.Add(loan);
            
            Debug.WriteLine($"Created loan: ID={loan.LoanID}, Amount={amount}, Currency={currency}, Months={months}");
            return loan;
        }

        // Process loan payment from specified bank account
        public string PayLoan(int userID, int loanId, string accountIBAN)
        {
            // Get the loan
            var loan = GetLoanById(loanId);
            if (loan == null || loan.State == "paid")
            {
                Debug.WriteLine($"Cannot pay loan: Loan not found or already paid. ID={loanId}");
                return "Loan not found or already paid";
            }

            var _bankAccounts = GetUserBankAccounts(userID);
            BankAccount? bankAccount = _bankAccounts.Find((bankAcc) => bankAcc.IBAN == accountIBAN);
            // Get the bank account
            if (bankAccount == null)
            {
                Debug.WriteLine($"Cannot pay loan: Bank account not found. ID={accountIBAN}");
                return "Bank account not found";
            }
            
            // Check if there are sufficient funds
            if (!CheckSufficientFunds(userID, accountIBAN, (float)loan.AmountToPay, loan.Currency))
            {
                Debug.WriteLine($"Cannot pay loan: Insufficient funds in account {accountIBAN}");
                return "Insufficient funds";
            }
            
            // Deduct from bank account
            float deductAmount = loan.Currency == bankAccount.Currency
                ? (float)loan.AmountToPay
                : ConvertCurrency((float)loan.AmountToPay, loan.Currency, bankAccount.Currency);
                
            UpdateBankAccount(userID, accountIBAN, deductAmount, bankAccount.Currency);
            
            // Mark loan as paid
            loan.MarkAsPaid();
            
            Debug.WriteLine($"Loan paid: ID={loanId}, Amount={loan.AmountToPay}, Currency={loan.Currency}");
            return "success";
        }

        // Calculate the tax percentage based on loan duration
        public float CalculateTaxPercentage(int months)
        {
            // Simple calculation: 1% per month
            return months * 1.0f;
        }

        // Calculate the total amount to be repaid
        public float CalculateAmountToPay(float amount, float taxPercentage)
        {
            return amount * (1 + taxPercentage / 100);
        }

        // Handle currency conversion
        public float ConvertCurrency(float amount, string fromCurrency, string toCurrency)
        {
            if (fromCurrency == toCurrency)
            {
                return amount;
            }
            
            // Ensure we have rates for both currencies
            if (!_currencyRates.ContainsKey(fromCurrency) || !_currencyRates.ContainsKey(toCurrency))
            {
                throw new ArgumentException("Unsupported currency");
            }
            
            // Convert to EUR first as a base currency, then to target currency
            float amountInEUR = amount / _currencyRates[fromCurrency];
            float result = amountInEUR * _currencyRates[toCurrency];
            
            Debug.WriteLine($"Currency conversion: {amount} {fromCurrency} = {result} {toCurrency}");
            return result;
        }

        // Validate loan request parameters
        public string ValidateLoanRequest(float amount, int months)
        {
            // Amount must be positive and not exceed one million
            if (amount <= 0 || amount > 1000000)
            {
                Debug.WriteLine($"Invalid loan amount: {amount}");
                return "Invalid loan amount";
            }
            
            // Months must be one of the allowed values
            var allowedMonths = new[] { 6, 12, 24, 36 };
            if (!allowedMonths.Contains(months))
            {
                Debug.WriteLine($"Invalid loan duration: {months}");
                return "Invalid loan duration";
            }
            
            return "success";
        }

        // Check if a bank account has sufficient funds
        public bool CheckSufficientFunds(int userID, string accountIBAN, float amount, string currency)
        {
            var _bankAccounts = GetUserBankAccounts(userID);
            BankAccount? bankAccount = _bankAccounts.Find((bankAcc) => bankAcc.IBAN == accountIBAN);

            if (bankAccount == null)
            {
                return false;
            }
            
            // If currencies match, simple comparison
            if (bankAccount.Currency == currency)
            {
                return bankAccount.Amount >= amount;
            }
            
            // Convert amount to account currency
            float convertedAmount = ConvertCurrency(amount, currency, bankAccount.Currency);
            return bankAccount.Amount >= convertedAmount;
        }

        // Update bank account balance
        public void UpdateBankAccount(int userID, string accountIBAN, float amount, string currency)
        {
            var _bankAccounts = GetUserBankAccounts(userID);
            BankAccount? bankAccount = _bankAccounts.Find((bankAcc) => bankAcc.IBAN == accountIBAN);
            if (bankAccount == null)
            {
                throw new ArgumentException($"Bank account not found: {accountIBAN}");
            }
            
            if (bankAccount.Currency != currency)
            {
                throw new ArgumentException("Currency mismatch");
            }

            bankAccount.Amount -= amount;
            
            Debug.WriteLine($"Bank account updated: {accountIBAN}, New balance: {bankAccount.Amount} {bankAccount.Currency}");
        }

        // Get loan details by ID
        public Loan? GetLoanById(int loanId)
        {
            return _loanRepository.GetAllLoans().FirstOrDefault(loan => loan.LoanID == loanId);
        }

        // Get all bank accounts for a user
        public List<BankAccount> GetUserBankAccounts(int userId)
        {
            return _loanRepository.GetBankAccountsByUserId(userId);
        }
        
        // Get formatted bank account strings for display
        public List<string> GetFormattedBankAccounts(int userId)
        {
            return GetUserBankAccounts(userId)
                .Select(account => $"{account.IBAN} - {account.Currency} - {account.Amount}")
                .ToList();
        }

        //// Initialize sample data for testing
        //private void InitializeSampleData()
        //{
        //    // Sample bank accounts
        //    _bankAccounts.Add("IBAN1", new BankAccount(123, "IBAN1", "EUR", 1000));
        //    _bankAccounts.Add("IBAN2", new BankAccount(123, "IBAN2", "USD", 2000));
        //    _bankAccounts.Add("IBAN3", new BankAccount(123, "IBAN3", "GBP", 3000));

        //    // Sample loans
        //    var currencies = new List<string> { "EUR", "USD", "GBP" };
        //    for (int i = 0; i < 5; i++)
        //        _loans.Add(new Loan(i, 123, (i+5)*10, currencies[i % currencies.Count], DateTime.Now, null, i+10, i+6, "unpaid"));

        //    // Mark the first loan as paid for demonstration
        //    _loans[0].MarkAsPaid();
        //}
    }
}
