using LoanShark.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoanShark.Service
{
    public class LoanService
    {
        // Simulated database for demonstration purposes
        private static List<Loan> _loans = new List<Loan>();
        private static Dictionary<string, BankAccount> _bankAccounts = new Dictionary<string, BankAccount>();
        
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
            InitializeSampleData();
        }

        // Get all loans for a specific user
        public List<Loan> GetUserLoans(int userId)
        {
            Debug.WriteLine(_loans.Count);
            Debug.WriteLine(_loans.Where(loan => loan.UserID == userId).ToList().Count);

            return _loans.Where(loan => loan.UserID == userId).ToList();
        }

        // Get only unpaid loans for a user
        public List<Loan> GetUnpaidLoans(int userId)
        {
            return _loans.Where(loan => loan.UserID == userId && loan.State == "unpaid").ToList();
        }

        // Create a new loan with the specified parameters
        public Loan TakeLoan(int userId, float amount, string currency, int months)
        {
            // Validate loan parameters
            if (ValidateLoanRequest(amount, months) != "success")
            {
                throw new ArgumentException("Invalid loan parameters");
            }

            // Calculate tax percentage
            float taxPercentage = CalculateTaxPercentage(months);
            
            // Create a new loan
            var loan = new Loan(
                userId,
                amount,
                currency,
                1 + taxPercentage / 100,  // Tax factor (e.g., 1.05 for 5%)
                months
            );
            
            // Add to our "database"
            _loans.Add(loan);
            
            Debug.WriteLine($"Created loan: ID={loan.LoanID}, Amount={amount}, Currency={currency}, Months={months}");
            return loan;
        }

        // Process loan payment from specified bank account
        public string PayLoan(int loanId, string bankAccountId)
        {
            // Get the loan
            var loan = GetLoanById(loanId);
            if (loan == null || loan.State == "paid")
            {
                Debug.WriteLine($"Cannot pay loan: Loan not found or already paid. ID={loanId}");
                return "Loan not found or already paid";
            }
            
            // Get the bank account
            if (!_bankAccounts.TryGetValue(bankAccountId, out var bankAccount))
            {
                Debug.WriteLine($"Cannot pay loan: Bank account not found. ID={bankAccountId}");
                return "Bank account not found";
            }
            
            // Check if there are sufficient funds
            if (!CheckSufficientFunds(bankAccountId, (float)loan.AmountToPay, loan.Currency))
            {
                Debug.WriteLine($"Cannot pay loan: Insufficient funds in account {bankAccountId}");
                return "Insufficient funds";
            }
            
            // Deduct from bank account
            float deductAmount = loan.Currency == bankAccount.Currency
                ? (float)loan.AmountToPay
                : ConvertCurrency((float)loan.AmountToPay, loan.Currency, bankAccount.Currency);
                
            UpdateBankAccount(bankAccountId, deductAmount, bankAccount.Currency, true);
            
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
        public bool CheckSufficientFunds(string bankAccountId, float amount, string currency)
        {
            if (!_bankAccounts.TryGetValue(bankAccountId, out var bankAccount))
            {
                return false;
            }
            
            // If currencies match, simple comparison
            if (bankAccount.Currency == currency)
            {
                return bankAccount.Balance >= amount;
            }
            
            // Convert amount to account currency
            float convertedAmount = ConvertCurrency(amount, currency, bankAccount.Currency);
            return bankAccount.Balance >= convertedAmount;
        }

        // Update bank account balance
        public void UpdateBankAccount(string bankAccountId, float amount, string currency, bool isDebit)
        {
            if (!_bankAccounts.TryGetValue(bankAccountId, out var bankAccount))
            {
                throw new ArgumentException($"Bank account not found: {bankAccountId}");
            }
            
            if (bankAccount.Currency != currency)
            {
                throw new ArgumentException("Currency mismatch");
            }
            
            if (isDebit)
            {
                bankAccount.Balance -= amount;
            }
            else
            {
                bankAccount.Balance += amount;
            }
            
            Debug.WriteLine($"Bank account updated: {bankAccountId}, New balance: {bankAccount.Balance} {bankAccount.Currency}");
        }

        // Get loan details by ID
        public Loan GetLoanById(int loanId)
        {
            return _loans.FirstOrDefault(loan => loan.LoanID == loanId);
        }

        // Get all bank accounts for a user
        public List<BankAccount> GetUserBankAccounts(int userId)
        {
            return _bankAccounts.Values.Where(account => account.UserID == userId).ToList();
        }
        
        // Get formatted bank account strings for display
        public List<string> GetFormattedBankAccounts(int userId)
        {
            return GetUserBankAccounts(userId)
                .Select(account => $"{account.IBAN} - {account.Currency} - {account.Balance}")
                .ToList();
        }

        // Initialize sample data for testing
        private void InitializeSampleData()
        {
            // Sample bank accounts
            _bankAccounts.Add("IBAN1", new BankAccount(123, "IBAN1", "EUR", 1000));
            _bankAccounts.Add("IBAN2", new BankAccount(123, "IBAN2", "USD", 2000));
            _bankAccounts.Add("IBAN3", new BankAccount(123, "IBAN3", "GBP", 3000));

            // Sample loans
            var currencies = new List<string> { "EUR", "USD", "GBP" };
            for (int i = 0; i < 5; i++)
                _loans.Add(new Loan(123, (i+5)*10, currencies[i % currencies.Count], i+10, i+6));

            // Mark the first loan as paid for demonstration
            _loans[0].MarkAsPaid();
        }
    }
}
