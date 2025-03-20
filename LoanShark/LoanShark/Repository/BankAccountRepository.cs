using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanShark.Data;
using LoanShark.Domain;
using Microsoft.Data.SqlClient;

namespace LoanShark.Repository
{
    interface IBankAccountRepository
    {
        List<BankAccount> getAllBankAccounts();
        List<BankAccount> getBankAccountsByUserId(int userID);

        BankAccount getBankAccountByIBAN(string IBAN);

        bool addBankAccount(BankAccount bankAccount);
        bool removeBankAccount(string IBAN);
        bool updateBankAccount(string IBAN, BankAccount NBA);
    }

    class BankAccountRepository : IBankAccountRepository
    {
        private readonly DataLink dataLink;
        
        public BankAccountRepository()
        {
            dataLink = new DataLink();
        }

        public List<BankAccount> getAllBankAccounts()
        {
            try
            {
                DataTable dataTable = dataLink.ExecuteReader("GetAllBankAccounts");
                return ConvertDataTableToBankAccountList(dataTable);
            } catch(Exception)
            {
                Debug.WriteLine("Oopsie on get all bank accounts repo");
                return null;
            }
        }

        public List<BankAccount> getBankAccountsByUserId(int userID)
        {
            try
            {
                var sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@id_user", userID)
                };
                DataTable dataTable = dataLink.ExecuteReader("GetBankAccountsByUser", sqlParams);
                return ConvertDataTableToBankAccountList(dataTable);
            }
            catch (Exception)
            {
                Debug.WriteLine("Oopsie on get bank accounts by user id repo");
                return null;
            }
        }

        public BankAccount getBankAccountByIBAN(string IBAN)
        {
            try
            {
                var sqlParams = new SqlParameter[] { new SqlParameter("@iban", IBAN) };
                DataTable dataTable = dataLink.ExecuteReader("GetBankAccountByIBAN", sqlParams);
                if (dataTable != null)
                    return ConvertDataTableRowToBankAccount(dataTable.Rows[0]);
                else
                    return null;
            }
            catch (Exception)
            {
                Debug.WriteLine("Oopsie on get bank account by IBAN repo");
                return null;
            }
        }

        public bool addBankAccount(BankAccount bankAccount)
        {
            try
            {
                var sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@iban", bankAccount.iban),
                    new SqlParameter("@currency", bankAccount.currency),
                    new SqlParameter("@amount", bankAccount.sold),
                    new SqlParameter("@id_user", bankAccount.userID),
                    new SqlParameter("@custom_name",                bankAccount.name),
                    new SqlParameter("@daily_limit",                bankAccount.dailyLimit),
                    new SqlParameter("@max_per_transaction",        bankAccount.maximumPerTransaction),
                    new SqlParameter("@max_nr_transactions_daily",  bankAccount.maximumNrTransactions),
                    new SqlParameter("@blocked",                    bankAccount.blocked)
                };
                dataLink.ExecuteNonQuery("AddBankAccount", sqlParams);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Oopsie on add bank account repo");
                return false;
            }
        }
        
        public bool removeBankAccount(string IBAN)
        {
            try
            {
                var sqlParams = new SqlParameter[] 
                { 
                    new SqlParameter("@iban", IBAN) 
                };
                dataLink.ExecuteNonQuery("RemoveBankAccount", sqlParams);
                return true;
            }
            catch (Exception)
            {
                Debug.WriteLine("Oopsie on remove bank account repo");
                return false;
            }
        }


        public BankAccount ConvertDataTableRowToBankAccount(DataRow row)
        {
            return new BankAccount(
                         Convert.ToString(row["iban"]) ?? "",
                         Convert.ToString(row["currency"]) ?? "",
                         Convert.ToDouble(row["amount"]),
                         Convert.ToBoolean(row["blocked"]),
                         Convert.ToInt32(row["id_user"]),
                         Convert.ToString(row["custom_name"]) ?? "",
                         Convert.ToDouble(row["daily_limit"]),
                         Convert.ToDouble(row["max_per_transaction"]),
                         Convert.ToInt32(row["max_nr_transactions_daily"])
                    );
        }
        public List<BankAccount> ConvertDataTableToBankAccountList(DataTable dataTable) {
            List<BankAccount> bankAccounts = new List<BankAccount>();
            foreach (DataRow row in dataTable.Rows)
            {
                bankAccounts.Add(ConvertDataTableRowToBankAccount(row));
            }
            return bankAccounts;
        }

        public bool updateBankAccount(string IBAN, BankAccount NBA)
        {
            try
            {
                var sqlParams = new SqlParameter[]
                {
                             new SqlParameter("@iban", IBAN),
                             new SqlParameter("@custom_name", NBA.name),
                             new SqlParameter("@daily_limit", NBA.dailyLimit),
                             new SqlParameter("@max_per_transaction",NBA.maximumPerTransaction),
                             new SqlParameter("@max_nr_transactions_daily",NBA.maximumNrTransactions),
                             new SqlParameter("@blocked",NBA.blocked)
                };
                dataLink.ExecuteNonQuery("UpdateBankAccount", sqlParams);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not update bank account");
                return false;
            }
        }
    }
}