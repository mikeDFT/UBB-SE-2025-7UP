using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using LoanShark.Data;

namespace LoanShark.Repository
{
    public class MainPageRepository
    {
        public MainPageRepository()
        {
        }

        public async Task<DataTable> GetUserBankAccounts(int id_user)
        {
            DataTable dataTable = new DataTable();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@id_user", id_user)
            };
            dataTable = await DataLink.Instance.ExecuteReader("GetUserBankAccounts", sqlParameters);

            return dataTable;
        }

        public async Task<Tuple<decimal, string>> GetBankAccountBalanceByUserIban(string iban)
        {
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@iban", iban)
            };
            DataTable dataTable = await DataLink.Instance.ExecuteReader("GetBankAccountBalanceByIban", sqlParameters);
            return new Tuple<decimal, string>(
                dataTable.Rows[0]["amount"] != DBNull.Value ? Convert.ToDecimal(dataTable.Rows[0]["amount"]) : 0,
                dataTable.Rows[0]["currency"] != DBNull.Value ? dataTable.Rows[0]["currency"].ToString() ?? string.Empty : string.Empty);
        }
    }
}
