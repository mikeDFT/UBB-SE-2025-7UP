using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using LoanShark.Data;
using System;

namespace LoanShark.Repository
{
    public class MainPageRepository
    {
        public MainPageRepository() {}

        public async Task<DataTable> GetUserBankAccounts(int id_user)
        {
            DataTable dt = new DataTable();
            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@id_user", id_user)
            };
            dt = await DataLink.Instance.ExecuteReader("GetUserBankAccounts", sqlParams);

            return dt;
        }

        public async Task<Tuple<decimal, string>> GetBankAccountBalanceByUserIban(string iban)
        {
            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@iban", iban)
            };
            DataTable dt = await DataLink.Instance.ExecuteReader("GetBankAccountBalanceByIban", sqlParams);
            return new Tuple<decimal, string>(
                dt.Rows[0]["amount"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["amount"]) : 0,
                dt.Rows[0]["currency"] != DBNull.Value ? dt.Rows[0]["currency"].ToString() ?? string.Empty : string.Empty
            );
        }
    }

}
