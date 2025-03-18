using LoanShark.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.Repository
{
    class LoginRepository
    {
        private readonly DataLink dataLink;

        public LoginRepository()
        {
            this.dataLink = new DataLink();
        }

        // Returns a DataTable with the user credentials, they will be accessible from dt.Rows[0]["hashed_password"] and dt.Rows[0]["passowrd_salt"]
        // If the user with the given email is not found, an exception will be thrown
        public DataTable GetUserCredentials(string email)
        {
            DataTable dt = new DataTable();
            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@email", email)
            };
            dt = dataLink.ExecuteReader("GetUserCredentials", sqlParams);

            if (dt.Rows.Count == 0)
            {
                throw new Exception($"User with the email {email} does NOT exist.");
            }

            return dt;
        } 

    }
}
