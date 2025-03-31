using LoanShark.Domain;
using LoanShark.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.Service
{
    /// <summary>
    /// Service class for handling bank account operations
    /// </summary>
    class BankAccountService
    {
        private IBankAccountRepository _bankAccountRepository;

        /// <summary>
        /// Initializes a new instance of the BankAccountService class
        /// </summary>
        public BankAccountService()
        {
            _bankAccountRepository = new BankAccountRepository();
        }

        /// <summary>
        /// Retrieves all bank accounts belonging to a specific user
        /// </summary>
        /// <param name="userID">The ID of the user</param>
        /// <returns>A list of BankAccount objects belonging to the user</returns>
        public async Task<List<BankAccount>?> GetUserBankAccounts(int userID)
        {
            return await _bankAccountRepository.GetBankAccountsByUserId(userID);
        }

        /// <summary>
        /// Finds a bank account by its IBAN
        /// </summary>
        /// <param name="IBAN">The IBAN of the bank account to find</param>
        /// <returns>The bank account with the specified IBAN, or null if not found</returns>
        public async Task<BankAccount?> FindBankAccount(string IBAN)
        {
            return await _bankAccountRepository.GetBankAccountByIBAN(IBAN);
        }

        /// <summary>
        /// Creates a new bank account for the specified user with the given currency
        /// </summary>
        /// <param name="userID">The ID of the user who will own the account</param>
        /// <param name="currency">The currency for the new bank account</param>
        /// <returns>True if the bank account was created successfully, false otherwise</returns>
        public async Task<bool> CreateBankAccount(int userID, string customName, string currency)
        {
            string IBAN = await GenerateIBAN();
            BankAccount newBankAccount = new BankAccount(IBAN,
                currency,
                0.0m,
                false,
                userID,
                customName,
                1000.0m,
                200.0m,
                10
                );
            return await _bankAccountRepository.AddBankAccount(newBankAccount);
        }

        /// <summary>
        /// Removes a bank account with the specified IBAN
        /// </summary>
        /// <param name="IBAN">The IBAN of the bank account to remove</param>
        /// <returns>True if the bank account was removed successfully, false otherwise</returns>
        public async Task<bool> RemoveBankAccount(string IBAN)
        {
            return await _bankAccountRepository.RemoveBankAccount(IBAN);
        }

        /// <summary>
        /// Checks if a bank account with the specified IBAN exists
        /// </summary>
        /// <param name="IBAN">The IBAN to check</param>
        /// <returns>True if a bank account with the IBAN exists, false otherwise</returns>
        public async Task<bool> CheckIBANExists(string IBAN)
        {
            List<string> listIBAN = (await _bankAccountRepository.GetAllBankAccounts())?.Select(account => account.iban).ToList() ?? new List<string>();
            return listIBAN.Any(x => x == IBAN);
        }

        /// <summary>
        /// Generates a unique IBAN for a new bank account
        /// </summary>
        /// <returns>A unique IBAN string</returns>
        public async Task<string> GenerateIBAN()
        {
            Random random = new Random();
            const string countryCode = "RO";
            const string swiftBIC = "SEUP";
            const int accountNumberLength = 16;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            while (true)
            {
                // Generate random account number
                string accountNumber = new string(Enumerable.Range(0, accountNumberLength)
                    .Select(_ => chars[random.Next(chars.Length)]).ToArray());

                // Create temporary IBAN with placeholder checksum
                string tempIban = countryCode + "00" + swiftBIC + accountNumber;
                string formattedIban = swiftBIC + accountNumber + "RO00";

                // Convert letters to numbers
                StringBuilder sb = new StringBuilder();
                foreach (char ch in formattedIban)
                {
                    sb.Append(char.IsLetter(ch) ? (ch - 'A' + 10).ToString() : ch.ToString());
                }

                // Compute MOD 97 checksum
                BigInteger ibanNumber = BigInteger.Parse(sb.ToString());
                int checksum = 98 - (int)(ibanNumber % 97);

                // Return final IBAN
                string finalIBAN = countryCode + checksum.ToString().PadLeft(2, '0') + swiftBIC + accountNumber;
                if (!await CheckIBANExists(finalIBAN))
                    return finalIBAN;
            }
        }


        /// <summary>
        /// Gets a list of all available currencies
        /// </summary>
        /// <returns>A list of currency names as strings</returns>
        public async Task<List<string>> GetCurrencies()
        {
            return await _bankAccountRepository.GetCurrencies();
        }

        /// <summary>
        /// Verifies user credentials for authentication
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="password">The user's password</param>
        /// <returns>True if the credentials are valid, false otherwise</returns>
        public async Task<bool> verifyUserCredentials(string email, string password)
        {
            Debug.WriteLine("Debug - verify credentials");
            List<string> credentials = await _bankAccountRepository.GetCredentials(email);
            HashedPassword inputHashedPassword = new HashedPassword(password, credentials[1], true);
            HashedPassword hashedPassword = new HashedPassword(credentials[0], credentials[1], false);
            return inputHashedPassword.Equals(hashedPassword);
        }

        //generates a default bank account with the attributes to be updated and passes it to repository
        // to update the database
        public async Task<bool> UpdateBankAccount(string IBAN , string name, decimal daily_limit, decimal max_per_trans, int max_nr_trans, bool blocked)
        {
            var NBA = new BankAccount(IBAN, "RON", 0, blocked, 123, name, daily_limit, max_per_trans, max_nr_trans);
            return await _bankAccountRepository.UpdateBankAccount(IBAN, NBA);
        }
    }
}
