using LoanShark.Domain;
using LoanShark.Repository;
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
        private readonly ILoanRepository _loanRepository;

        public LoanService()
        {
            _loanRepository = new LoanRepository();
        }

        // Get all loans for a specific user
        public async Task<List<Loan>> GetUserLoans(int userId)
        {
            return await _loanRepository.GetLoansByUserId(userId);
        }

        // Get only unpaid loans for a user
        public async Task<List<Loan>> GetUnpaidUserLoans(int userId)
        {
            return (await GetUserLoans(userId)).Where(
                loan => loan.State == "unpaid").ToList();
        }

        // Create a new loan with the specified parameters
        public async Task<Loan?> TakeLoanAsync(int userId, decimal amount, string currency, string accountIBAN, int months)
        {
            // Validate loan parameters
            if (ValidateLoanRequest(amount, months) != "success")
            {
                throw new ArgumentException("Invalid loan parameters");
            }

            // Calculate tax percentage
            decimal taxPercentage = CalculateTaxPercentage(months);

            try {
                await TransactionsService.TakeLoanTransaction(accountIBAN, amount);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error taking loan: {ex.Message}");
                return null;
            }

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

            // returns the loan with its id from the db, initially it was -1, check above
            loan = await _loanRepository.CreateLoan(loan);
            
            Debug.WriteLine($"Created loan: ID={loan?.LoanID}, Amount={amount}, Currency={currency}, Months={months}");
            return loan;
        }

        // Process loan payment from specified bank account
        public async Task<string> PayLoanAsync(int userID, int loanId, string accountIBAN)
        {
            // Get the loan
            var loan = await GetLoanById(loanId);
            if (loan == null || loan.State == "paid")
            {
                Debug.WriteLine($"Cannot pay loan: Loan not found or already paid. ID={loanId}");
                return "Loan not found or already paid";
            }

            // Get the bank account
            BankAccount? bankAccount = await _loanRepository.GetBankAccountByIBAN(accountIBAN);
            if (bankAccount == null)
            {
                Debug.WriteLine($"Cannot pay loan: Bank account not found. IBAN={accountIBAN}");
                return "Bank account not found";
            }
            
            // Check if there are sufficient funds
            if (!await CheckSufficientFunds(userID, accountIBAN, loan.AmountToPay, loan.Currency))
            {
                Debug.WriteLine($"Cannot pay loan: Insufficient funds in account {accountIBAN}");
                return "Insufficient funds";
            }
            
            // Deduct from bank account
            decimal deductAmount = loan.Currency == bankAccount.Currency
                ? loan.AmountToPay
                : await ConvertCurrency(loan.AmountToPay, loan.Currency, bankAccount.Currency);
                
            try {
                await UpdateBankAccount(userID, accountIBAN, deductAmount, bankAccount.Currency);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating bank account balance: {ex.Message}");
                return "Error updating bank account balance";
            }

            try {
                await TransactionsService.PayLoanTransaction(accountIBAN, deductAmount);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error paying loan: {ex.Message}");
                return "Error paying loan";
            }
            
            // Mark loan as paid
            loan.MarkAsPaid();
            bool success = await _loanRepository.UpdateLoan(loan);
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
        public async Task<decimal> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            if (fromCurrency == toCurrency)
            {
                return amount;
            }

            List<CurrencyExchange> currencyRates = await _loanRepository.GetAllCurrencyExchanges();
            CurrencyExchange? nullableCurrencyRate = GetCurrencyRate(currencyRates, fromCurrency, toCurrency);
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
        public async Task<bool> CheckSufficientFunds(int userID, string accountIBAN, decimal amount, string currency)
        {
            var _bankAccounts = await GetUserBankAccounts(userID);
            BankAccount? bankAccount = _bankAccounts.Find((bankAcc) => bankAcc.Iban == accountIBAN);

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
            decimal convertedAmount = await ConvertCurrency(amount, currency, bankAccount.Currency);
            return bankAccount.Balance >= convertedAmount;
        }

        // Update bank account balance
        public async Task UpdateBankAccount(int userID, string accountIBAN, decimal amount, string currency)
        {
            BankAccount? bankAccount = await _loanRepository.GetBankAccountByIBAN(accountIBAN);
            if (bankAccount == null)
            {
                throw new ArgumentException($"Bank account not found: {accountIBAN}");
            }
            
            if (bankAccount.Currency != currency)
            {
                throw new ArgumentException("Currency mismatch");
            }

            if (! await _loanRepository.UpdateBankAccountBalance(accountIBAN, bankAccount.Balance - amount))
            {
                throw new Exception("Failed to update bank account balance");
            }
        }

        // Get loan details by ID
        public async Task<Loan?> GetLoanById(int loanId)
        {
            return (await _loanRepository.GetAllLoans()).FirstOrDefault(loan => loan.LoanID == loanId);
        }

        // Get all bank accounts for a user
        public async Task<List<BankAccount>> GetUserBankAccounts(int userId)
        {
            return await _loanRepository.GetBankAccountsByUserId(userId);
        }
        
        // Get formatted bank account strings for display
        public async Task<List<string>> GetFormattedBankAccounts(int userId)
        {
            return (await GetUserBankAccounts(userId))
                .Select(account => $"{account.Iban} - {account.Currency} - {account.Balance}")
                .ToList();
        }
    }
}
