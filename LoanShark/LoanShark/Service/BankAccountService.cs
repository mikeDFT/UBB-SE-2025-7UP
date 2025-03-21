using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.Repository;

namespace LoanShark.Service
{
    class BankAccountService
    {
        private IBankAccountRepository _bankAccountRepository;

        public BankAccountService()
        {
            _bankAccountRepository = new BankAccountRepository();
        }


        public List<BankAccount> getUserBankAccounts(int userID)
        {
            return _bankAccountRepository.getBankAccountsByUserId(userID);
        }
        public BankAccount findBankAccount(string IBAN)
        {
            return _bankAccountRepository.getBankAccountByIBAN(IBAN);
        }

        public bool createBankAccount(int userID, string currency)
        {
            string IBAN = generateIBAN();
            BankAccount newBankAccount = new BankAccount(IBAN,
                currency,
                0.0,
                false,
                userID,
                "",
                1000.0,
                200.0,
                10
                );
            return _bankAccountRepository.addBankAccount(newBankAccount);
        }

        public bool removeBankAccount(string IBAN)
        {
            return _bankAccountRepository.removeBankAccount(IBAN);
        }

        public bool checkIBANExists(string IBAN)
        {
            List<string> listIBAN = _bankAccountRepository.getAllBankAccounts().Select(account => account.iban).ToList();
            return listIBAN.Any(x => x == IBAN);
        } 

        public string generateIBAN()
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
                if (!checkIBANExists(finalIBAN))
                    return finalIBAN;
            }
        }
        //generates a default bank account with the attributes to be updated and passes it to repository
        // to update the database
        public bool updateBankAccount(string IBAN , string name, float daily_limit, float max_per_trans, int max_nr_trans, bool blocked)
        {
            var NBA = new BankAccount(IBAN, "RON", 0, blocked, 123, name, daily_limit, max_per_trans, max_nr_trans);
            return _bankAccountRepository.updateBankAccount(IBAN, NBA);
        }
    }
}
