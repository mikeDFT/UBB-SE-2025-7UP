using LoanShark.Domain;
using LoanShark.Repository;
using System;
using System.Data;

namespace LoanShark.Service
{
    class LoginService
    {
        public LoginRepository repo;

        public LoginService()
        {
            this.repo = new LoginRepository();
        }

        public bool ValidateUserCredentials(string email, string password)
        {
            try
            {
                DataTable dt = this.repo.GetUserCredentials(email);

                //if exception is not thorwn, then the user exists and we continue with the validation
                string hashedPassword = dt.Rows[0]["hashed_password"]?.ToString() ?? string.Empty;
                string passwordSalt = dt.Rows[0]["password_salt"]?.ToString() ?? string.Empty;

                HashedPassword userPassword = new HashedPassword(hashedPassword, passwordSalt, false);
                HashedPassword inputPassword = new HashedPassword(password, passwordSalt, true);

                return userPassword.Equals(inputPassword);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

