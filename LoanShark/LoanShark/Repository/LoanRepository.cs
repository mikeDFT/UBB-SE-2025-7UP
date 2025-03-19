using LoanShark.Data;
using LoanShark.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace LoanShark.Repository
{
    public interface ILoanRepository
    {   // here this is like a list of functions
        Loan CreateLoan(Loan loan);
        List<Loan> GetAllLoans();
        List<Loan> GetLoansByUserId(int userId);
        Loan? GetLoanById(int loanId);
        bool UpdateLoan(Loan loan);
        bool DeleteLoan(int loanId);
        List<BankAccount> GetBankAccountsByUserId(int userId);
        List<CurrencyExchange> GetAllCurrencyExchanges();
    }

    public class LoanRepository : ILoanRepository
    {
        private readonly DataLink _dataLink;

        public LoanRepository()
        {
            _dataLink = new DataLink();
        }

        public Loan CreateLoan(Loan loan)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@id_user", loan.UserID),
                    new SqlParameter("@amount", loan.Amount),
                    new SqlParameter("@currency", loan.Currency),
                    new SqlParameter("@date_deadline", loan.DateDeadline),
                    new SqlParameter("@tax_percentage", loan.TaxPercentage),
                    new SqlParameter("@number_months", loan.NumberMonths),
                    new SqlParameter("@loan_state", "unpaid"), // New loans are always unpaid
                    
                    // Direction of Data Flow: By setting Direction = ParameterDirection.Output,
                    // you're telling ADO.NET that this parameter isn't for sending data to the database,
                    // but for receiving data back from it.
                    // This is used to get the ID of the newly created loan
                    new SqlParameter("@id_loan", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                _dataLink.ExecuteNonQuery("CreateLoan", parameters);
                
                // Get the ID of the newly created loan
                int newLoanId = (int)parameters.First(p => p.ParameterName == "@id_loan").Value;
                loan.LoanID = newLoanId;

                Debug.WriteLine("REPO: Loan created", newLoanId, loan.ToString());

                return loan;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating loan: {ex.Message}");
                return null;
            }
        }


        // LOAN FUNCTIONS
        public List<Loan> GetAllLoans()
        {
            try
            {
                DataTable dataTable = _dataLink.ExecuteReader("GetAllLoans");
                return ConvertDataTableToLoans(dataTable);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving all loans: {ex.Message}");
                return new List<Loan>();
            }
        }

        public List<Loan> GetLoansByUserId(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@id_user", userId)
                };

                DataTable dataTable = _dataLink.ExecuteReader("GetLoansByUserId", parameters);
                return ConvertDataTableToLoans(dataTable);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving loans for user {userId}: {ex.Message}");
                return new List<Loan>();
            }
        }

        public Loan? GetLoanById(int loanId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@id_loan", loanId)
                };

                DataTable dataTable = _dataLink.ExecuteReader("GetLoanById", parameters);
                
                if (dataTable.Rows.Count == 0)
                    return null;
                
                return ConvertDataRowToLoan(dataTable.Rows[0]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving loan {loanId}: {ex.Message}");
                return null;
            }
        }

        public bool UpdateLoan(Loan loan)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@id_loan", loan.LoanID),
                    new SqlParameter("@amount", loan.Amount),
                    new SqlParameter("@currency", loan.Currency),
                    new SqlParameter("@date_deadline", loan.DateDeadline),
                    new SqlParameter("@date_paid", loan.DatePaid),
                    new SqlParameter("@tax_percentage", loan.TaxPercentage),
                    new SqlParameter("@loan_state", loan.State)
                };

                int rowsAffected = _dataLink.ExecuteNonQuery("UpdateLoan", parameters);
                Debug.WriteLine("REPO: Loan updated", loan.ToString());
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating loan {loan.LoanID}: {ex.Message}");
                return false;
            }
        }

        public bool DeleteLoan(int loanId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@id_loan", loanId)
                };

                int rowsAffected = _dataLink.ExecuteNonQuery("DeleteLoan", parameters);
                Debug.WriteLine("REPO: Loan deleted", loanId);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting loan {loanId}: {ex.Message}");
                return false;
            }
        }

        // BANK ACCOUNT FUNCTIONS
        public List<BankAccount> GetBankAccountsByUserId(int userId) {
            try {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@id_user", userId)
                };

                DataTable dataTable = _dataLink.ExecuteReader("GetBankAccountsByUserId", parameters);
                return ConvertDataTableToBankAccounts(dataTable);
            }
            catch (Exception ex) {
                Debug.WriteLine($"Error retrieving bank accounts for user {userId}: {ex.Message}");
                return new List<BankAccount>();
            }
        }

        // CURRENCY EXCHANGE FUNCTIONS 
        public List<CurrencyExchange> GetAllCurrencyExchanges() {
            try {
                DataTable dataTable = _dataLink.ExecuteReader("GetAllCurrencyExchanges");
                return ConvertDataTableToCurrencyExchanges(dataTable);
            }
            catch (Exception ex) {
                Debug.WriteLine($"Error retrieving all currency exchanges: {ex.Message}");
                return new List<CurrencyExchange>();
            }
        }


        // CONVERT DATA TABLE TO LOAN FUNCTIONS
        private List<Loan> ConvertDataTableToLoans(DataTable dataTable)
        {
            List<Loan> loans = new List<Loan>();
            
            foreach (DataRow row in dataTable.Rows)
            {
                loans.Add(ConvertDataRowToLoan(row));
            }

            Debug.WriteLine("REPO: Got loans by user", loans.Count, loans);
            return loans;
        }

        private Loan ConvertDataRowToLoan(DataRow row)
        {
            return new Loan(
                Convert.ToInt32(row["id_loan"]),
                Convert.ToInt32(row["id_user"]),
                Convert.ToDecimal(row["amount"]),
                row["currency"].ToString() ?? "",
                Convert.ToDateTime(row["date_taken"]),
                row["date_paid"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(row["date_paid"]),
                Convert.ToDecimal(row["tax_percentage"]),
                Convert.ToInt32(row["number_months"]),
                row["loan_state"].ToString() ?? ""
            );
        }

        // CONVERT DATA TABLE TO BANK ACCOUNT FUNCTIONS
        private List<BankAccount> ConvertDataTableToBankAccounts(DataTable dataTable) {
            List<BankAccount> bankAccounts = new List<BankAccount>();

            foreach (DataRow row in dataTable.Rows) {
                bankAccounts.Add(ConvertDataRowToBankAccount(row));
            }

            Debug.WriteLine("REPO: Got bank accounts by user", bankAccounts.Count, bankAccounts);
            return bankAccounts;
        }

        private BankAccount ConvertDataRowToBankAccount(DataRow row) {
            return new BankAccount(
                row["iban"].ToString(),
                Convert.ToInt32(row["id_user"]),
                Convert.ToDecimal(row["amount"]),
                row["currency"].ToString(),
                row["custom_name"]?.ToString(),
                Convert.ToDecimal(row["daily_limit"]),
                Convert.ToDecimal(row["max_per_transaction"]),
                Convert.ToInt32(row["max_nr_transactions_daily"]),
                Convert.ToBoolean(row["blocked"])
            );


        }

        // CONVERT DATA TABLE TO CURRENCY EXCHANGE FUNCTIONS
        private List<CurrencyExchange> ConvertDataTableToCurrencyExchanges(DataTable dataTable) {
            List<CurrencyExchange> currencyExchanges = new List<CurrencyExchange>();
            
            foreach (DataRow row in dataTable.Rows) {
                currencyExchanges.Add(ConvertDataRowToCurrencyExchange(row));
            }

            Debug.WriteLine("REPO: Got currency exchanges by user", currencyExchanges.Count, currencyExchanges);
            return currencyExchanges;
        }

        private CurrencyExchange ConvertDataRowToCurrencyExchange(DataRow row) {
            return new CurrencyExchange(
                row["from_currency"].ToString() ?? "",
                row["to_currency"].ToString() ?? "",
                (decimal)Convert.ToDouble(row["rate"])
            );
        }
    }
}
