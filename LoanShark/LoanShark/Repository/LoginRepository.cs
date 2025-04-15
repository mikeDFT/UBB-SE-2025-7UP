using System;
using System.Data;
using System.Threading.Tasks;
using LoanShark.Data;
using Microsoft.Data.SqlClient;

namespace LoanShark.Repository
{
    public class LoginRepository
    {
        public LoginRepository()
        {
        }

        // Returns a DataTable with the user credentials, they will be accessible from dt.Rows[0]["hashed_password"] and dt.Rows[0]["passowrd_salt"]
        // If the user with the given email is not found, an exception will be thrown
        public async Task<DataTable> GetUserCredentials(string email)
        {
            DataTable dataTable = new DataTable();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@email", email)
            };
            dataTable = await DataLink.Instance.ExecuteReader("GetUserCredentials", sqlParameters);

            if (dataTable.Rows.Count == 0)
            {
                throw new Exception($"User with the email {email} does NOT exist.");
            }

            return dataTable;
        }

        public async Task<DataTable> GetUserInfoAfterLogin(string email)
        {
            DataTable dataTable = new DataTable();
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@email", email)
            };
            dataTable = await DataLink.Instance.ExecuteReader("GetUserInfoAfterLogin", sqlParameters);

            if (dataTable.Rows.Count == 0)
            {
                throw new Exception($"User with the email {email} does NOT exist.");
            }

            return dataTable;
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
    }
}
