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
        private readonly ILoanRepository _loanRepository;

        public LoanService()
        {
            _loanRepository = new LoanRepository();
        }

        // Get all loans for a specific user
        public List<Loan> GetUserLoans(int userId)
        {
            return _loanRepository.GetLoansByUserId(userId);
        }

        // Get only unpaid loans for a user
        public List<Loan> GetUnpaidUserLoans(int userId)
        {
            return GetUserLoans(userId).Where(
                loan => loan.State == "unpaid").ToList();
        }

        // Create a new loan with the specified parameters
        public Loan TakeLoan(int userId, decimal amount, string currency, string accountIBAN, int months)
        {
            // Validate loan parameters
            if (ValidateLoanRequest(amount, months) != "success")
            {
                throw new ArgumentException("Invalid loan parameters");
            }

            // Calculate tax percentage
            decimal taxPercentage = CalculateTaxPercentage(months);

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

            // gives it a valid unique ID
            loan = _loanRepository.CreateLoan(loan);
            
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

            // Get the bank account
            var _bankAccounts = GetUserBankAccounts(userID);
            BankAccount? bankAccount = _bankAccounts.Find((bankAcc) => bankAcc.IBAN == accountIBAN);
            if (bankAccount == null)
            {
                Debug.WriteLine($"Cannot pay loan: Bank account not found. ID={accountIBAN}");
                return "Bank account not found";
            }
            
            // Check if there are sufficient funds
            if (!CheckSufficientFunds(userID, accountIBAN, loan.AmountToPay, loan.Currency))
            {
                Debug.WriteLine($"Cannot pay loan: Insufficient funds in account {accountIBAN}");
                return "Insufficient funds";
            }
            
            // Deduct from bank account
            decimal deductAmount = loan.Currency == bankAccount.Currency
                ? loan.AmountToPay
                : ConvertCurrency(loan.AmountToPay, loan.Currency, bankAccount.Currency);
                
            UpdateBankAccount(userID, accountIBAN, deductAmount, bankAccount.Currency);
            
            // Mark loan as paid
            loan.MarkAsPaid();
            bool success = _loanRepository.UpdateLoan(loan);
            if(!success)
            {
                Debug.WriteLine("Update loan failed");
                return "Update loan failed";
            }
            
            Debug.WriteLine($"Loan paid: ID={loanId}, Amount={loan.AmountToPay}, Currency={loan.Currency}");
            return "success";
        }

        // Calculate the tax percentage based on loan duration
        public decimal CalculateTaxPercentage(int months)
        {
            // Simple calculation: 1% per month
            return months;
        }

        // Calculate the total amount to be repaid
        public decimal CalculateAmountToPay(decimal amount, decimal taxPercentage)
        {
            return amount * (1 + taxPercentage / 100);
        }

        // Handle currency conversion
        public decimal ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            if (fromCurrency == toCurrency)
            {
                return amount;
            }

            CurrencyExchange? nullableCurrencyRate = GetCurrencyRate(_loanRepository.GetAllCurrencyExchanges(), fromCurrency, toCurrency);
            if (nullableCurrencyRate == null) 
                throw new ArgumentException("Can't find currencies: " + fromCurrency + " to " + toCurrency);

            CurrencyExchange currencyRate = nullableCurrencyRate;

            decimal result = amount * currencyRate.ExchangeRate;

            Debug.WriteLine($"Currency conversion: {amount} {fromCurrency} = {result} {toCurrency}");
            return result;
        }

        // Helps find the needed currency rate
        public CurrencyExchange? GetCurrencyRate(List<CurrencyExchange> CurrencyRates, string fromCurrency, string toCurrency)
        {
            foreach (CurrencyExchange rate in CurrencyRates)
            {
                if (rate.FromCurrency == fromCurrency && rate.ToCurrency == toCurrency)
                    return rate;
            }

            return null;
        }

        // Validate loan request parameters
        public string ValidateLoanRequest(decimal amount, int months)
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
        public bool CheckSufficientFunds(int userID, string accountIBAN, decimal amount, string currency)
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
                return bankAccount.Balance >= amount;
            }
            
            // Convert amount to account currency
            decimal convertedAmount = ConvertCurrency(amount, currency, bankAccount.Currency);
            return bankAccount.Balance >= convertedAmount;
        }

        // Update bank account balance
        public void UpdateBankAccount(int userID, string accountIBAN, decimal amount, string currency)
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

            bankAccount.Balance -= amount;
            
            Debug.WriteLine($"Bank account updated: {accountIBAN}, New balance: {bankAccount.Balance} {bankAccount.Currency}");
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
                .Select(account => $"{account.IBAN} - {account.Currency} - {account.Balance}")
                .ToList();
        }
    }
}
