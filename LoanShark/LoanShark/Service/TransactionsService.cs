using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.Repository;

namespace LoanShark.Service
{
    public class TransactionsService
    {
        private static readonly TransactionsRepository _transactionsRepository = new TransactionsRepository();

        public TransactionsService() {}

        public async Task<string> AddTransaction(string senderIban, string receiverIban, decimal amount, string transactionDescription = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(senderIban) || string.IsNullOrWhiteSpace(receiverIban))
                {
                    return "Receiver IBANs must be provided.";
                }

                if (amount <= 0)
                    return "Invalid transaction amount. Must be greater than zero.";

                if (senderIban == receiverIban)
                {
                    return "Cannot send money to the same account.";
                }

                Debug.WriteLine($"DEBUG: Processing transaction from {senderIban} to {receiverIban}, Amount: {amount}");

                BankAccount? senderAccount = await _transactionsRepository.GetBankAccountByIBAN(senderIban);
                BankAccount? receiverAccount = await _transactionsRepository.GetBankAccountByIBAN(receiverIban);

                if (senderAccount == null) return "Sender account does not exist.";
                if (receiverAccount == null) return "Receiver account does not exist.";

                if (senderAccount.blocked) return "Sender account is blocked.";
                if (receiverAccount.blocked) return "Receiver account is blocked.";

                if (senderAccount.balance < amount)
                {
                    return "Insufficient funds.";
                }

                if (amount > senderAccount.maximumPerTransaction)
                {
                    return $"Transaction exceeds maximum limit per transaction ({senderAccount.maximumPerTransaction}).";
                }

                decimal receiverAmount = amount;

                if (senderAccount.currency != receiverAccount.currency)
                {
                    decimal exchangeRate = await _transactionsRepository.GetExchangeRate(senderAccount.currency, receiverAccount.currency);
                    if (exchangeRate == -1)
                    {
                        return "Exchange rate not available.";
                    }

                    receiverAmount = Math.Round(amount * exchangeRate, 2);
                }

                Transaction transaction = new Transaction
                {
                    SenderIban = senderIban,
                    ReceiverIban = receiverIban,
                    TransactionDatetime = DateTime.UtcNow,
                    SenderCurrency = senderAccount.currency,
                    ReceiverCurrency = receiverAccount.currency,
                    SenderAmount = amount,
                    ReceiverAmount = receiverAmount,
                    TransactionType = "user to user",
                    TransactionDescription = transactionDescription
                };

                await _transactionsRepository.AddTransaction(transaction);
                await _transactionsRepository.UpdateBankAccountBalance(senderIban, senderAccount.balance - amount);
                await _transactionsRepository.UpdateBankAccountBalance(receiverIban, receiverAccount.balance + receiverAmount);

                return "Transaction successful!";
            }
            catch (Exception ex)
            {
               return $"Error processing transaction: {ex.Message}";
            }

        }


        public static async Task<string> TakeLoanTransaction(string iban, decimal loanAmount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(iban))
                {
                    return "IBAN must be provided.";
                }

                if (loanAmount <= 0)
                    return "Invalid loan amount. Must be greater than zero.";

                BankAccount? userAccount = await _transactionsRepository.GetBankAccountByIBAN(iban);

                if (userAccount == null) return "Bank account does not exist.";
                if (userAccount.blocked) return "Bank account is blocked.";

                await _transactionsRepository.UpdateBankAccountBalance(iban, userAccount.balance + loanAmount);

                Transaction transaction = new Transaction
                {
                    SenderIban = "RO90BANK0000000000000005",
                    ReceiverIban = iban,
                    TransactionDatetime = DateTime.UtcNow,
                    SenderCurrency = userAccount.currency,
                    ReceiverCurrency = userAccount.currency,
                    SenderAmount = loanAmount,
                    ReceiverAmount = loanAmount,
                    TransactionType = "loan",
                    TransactionDescription = "Loan credited to account"
                };

                await _transactionsRepository.AddTransaction(transaction);

                return "Loan credited to your account!";
            }
            catch (Exception ex)
            {
                return $"Error processing loan transaction: {ex.Message}";
            }
        }


        public static async Task<string> PayLoanTransaction(string iban, decimal paymentAmount)
        {
            try
            {
                const string bankIban = "RO90BANK0000000000000005";

                if (string.IsNullOrWhiteSpace(iban))
                    return "IBAN must be provided.";

                if (paymentAmount <= 0)
                    return "Invalid payment amount. Must be greater than zero.";

                BankAccount? userAccount = await _transactionsRepository.GetBankAccountByIBAN(iban);
                BankAccount? bankAccount = await _transactionsRepository.GetBankAccountByIBAN(bankIban);

                if (userAccount == null) return "User account does not exist.";
                if (userAccount.blocked) return "User account is blocked.";
                if (userAccount.balance < paymentAmount) return "Insufficient funds.";
                if (bankAccount == null) return "Bank account does not exist.";

                await _transactionsRepository.UpdateBankAccountBalance(iban, userAccount.balance - paymentAmount);

                Transaction transaction = new Transaction
                {
                    SenderIban = iban,
                    ReceiverIban = bankIban,  
                    TransactionDatetime = DateTime.UtcNow,
                    SenderCurrency = userAccount.currency,
                    ReceiverCurrency = userAccount.currency,
                    SenderAmount = paymentAmount,
                    ReceiverAmount = paymentAmount,
                    TransactionType = "loan",
                    TransactionDescription = "Loan payment deducted from account"
                };

                await _transactionsRepository.AddTransaction(transaction);

                return "Loan payment successful!";
            }
            catch (Exception ex)
            {
                return $"Error processing loan payment: {ex.Message}";
            }
        }



        public async Task<List<CurrencyExchange>> GetAllCurrencyExchangeRates()
        {
            try
            {
                return await _transactionsRepository.GetAllCurrencyExchangeRates();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving currency exchange rates: {ex.Message}", ex);
            }
        }
    }
}
