using LoanShark.Domain;
using LoanShark.Repository;
using System;
using System.Data;
using System.Threading.Tasks;

namespace LoanShark.Service
{
    class LoginService
    {
        public LoginRepository repo;

        public LoginService()
        {
            this.repo = new LoginRepository();
        }

        public async Task<bool> ValidateUserCredentials(string email, string password)
        {
            try
            {
                DataTable dt = await this.repo.GetUserCredentials(email);

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

        public async Task InstantiateUserSessionAfterLogin(string email) 
        {
            DataTable dt_user_info = await this.repo.GetUserInfoAfterLogin(email);
            DataTable dt_bank_accounts = await this.repo.GetUserBankAccounts(int.Parse(dt_user_info.Rows[0]["id_user"]?.ToString() ?? string.Empty));

            UserSession.Instance.Initialize(
                dt_user_info.Rows[0]["id_user"]?.ToString() ?? string.Empty,
                dt_user_info.Rows[0]["cnp"]?.ToString() ?? string.Empty,
                dt_user_info.Rows[0]["first_name"]?.ToString() ?? string.Empty,
                dt_user_info.Rows[0]["last_name"]?.ToString() ?? string.Empty,
                dt_user_info.Rows[0]["email"]?.ToString() ?? string.Empty,
                dt_user_info.Rows[0]["phone_number"]?.ToString() ?? string.Empty,
                dt_bank_accounts.Rows[0]["iban"]?.ToString() ?? string.Empty
            );
        }
    }
}

