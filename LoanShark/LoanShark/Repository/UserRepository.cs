using LoanShark.Data;
using LoanShark.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserDataTasks;

namespace LoanShark.Repository
{
    public interface IUserRepository
    {
        User CreateUser(User user);
        User? GetUserById(int id_user);
    }

    public class UserRepository: IUserRepository
    {
        private readonly DataLink _dataLink;
        public UserRepository()
        {
            _dataLink = new DataLink();
        }

        public User CreateUser(User user)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@cnp", user.CNP.ToString()),
                    new SqlParameter("@first_name", user.FirstName),
                    new SqlParameter("@last_name", user.LastName),
                    new SqlParameter("@email", user.Email.ToString()),
                    new SqlParameter("@phone_number", user.PhoneNumber.ToString()),
                    new SqlParameter("@hashed_password", user.HashedPassword.GetHashedPassword()),
                    new SqlParameter("@password_salt", user.HashedPassword.GetSalt()),
                    new SqlParameter("@id_user", SqlDbType.Int) { Direction = ParameterDirection.Output}
                };
                _dataLink.ExecuteNonQuery("CreateUser", parameters);
                int newUserId = (int)parameters.First(p => p.ParameterName == "@id_user").Value;
                user.UserID = newUserId;
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"REPO: Error creating user: {ex.Message}");
                return null;
            }
        }

        public User? GetUserById(int id_user)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@id_user", id_user)
                };
                DataTable dt = _dataLink.ExecuteReader("GetUserById", parameters);
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                DataRow dr = dt.Rows[0];
                return new User(
                    id_user,
                    new CNP(dr["cnp"].ToString() ?? ""),
                    dr["first_name"].ToString() ?? "",
                    dr["last_name"].ToString() ?? "",
                    new Email(dr["email"].ToString() ?? ""),
                    new PhoneNumber(dr["phone_number"].ToString() ?? ""),
                    new HashedPassword(dr["hashed_password"].ToString() ?? "", dr["password_salt"].ToString() ?? "", false)
                    );
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"REPO: Error getting user by id: {ex.Message}");
                return null;
            }
        }
    }
}
