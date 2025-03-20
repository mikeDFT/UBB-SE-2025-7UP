using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using LoanShark.Data;

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
    }
}
