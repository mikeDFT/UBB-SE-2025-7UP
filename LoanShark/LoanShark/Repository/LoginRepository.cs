﻿using LoanShark.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace LoanShark.Repository
{
    class LoginRepository
    {
        public LoginRepository(){}

        // Returns a DataTable with the user credentials, they will be accessible from dt.Rows[0]["hashed_password"] and dt.Rows[0]["passowrd_salt"]
        // If the user with the given email is not found, an exception will be thrown
        public async Task<DataTable> GetUserCredentials(string email)
        {
            DataTable dt = new DataTable();
            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@email", email)
            };
            dt = await DataLink.Instance.ExecuteReader("GetUserCredentials", sqlParams);

            if (dt.Rows.Count == 0)
            {
                throw new Exception($"User with the email {email} does NOT exist.");
            }

            return dt;
        } 

        public async Task<DataTable> GetUserInfoAfterLogin(string email)
        {
            DataTable dt = new DataTable();
            SqlParameter[] sqlParams = 
            {
                new SqlParameter("@email", email)
            };
            dt = await DataLink.Instance.ExecuteReader("GetUserInfoAfterLogin", sqlParams);

            if (dt.Rows.Count == 0)
            {
                throw new Exception($"User with the email {email} does NOT exist.");
            }

            return dt;
        }

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
