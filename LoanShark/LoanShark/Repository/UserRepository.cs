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
using Windows.System;
using User = LoanShark.Domain.User;

namespace LoanShark.Repository
{
    public interface IUserRepository
    {
        Task<User?> CreateUser(User user);
        Task<User?> GetUserById(int userId);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser();
        Task<User?> GetUserByCnp(string cnp);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserByPhoneNumber(string phoneNumber);
        Task<string[]> GetUserPasswordHashSalt();
    }

    public class UserRepository: IUserRepository
    {
        public UserRepository() {}

        // saves a new user in the database
        // on success returns the user with the corresponding userId from the database
        // on failure returns null
        public async Task<User?> CreateUser(User user)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@cnp", user.Cnp.ToString()),
                    new SqlParameter("@first_name", user.FirstName),
                    new SqlParameter("@last_name", user.LastName),
                    new SqlParameter("@email", user.Email.ToString()),
                    new SqlParameter("@phone_number", user.PhoneNumber.ToString()),
                    new SqlParameter("@hashed_password", user.HashedPassword.GetHashedPassword()),
                    new SqlParameter("@password_salt", user.HashedPassword.GetSalt()),
                    new SqlParameter("@id_user", SqlDbType.Int) { Direction = ParameterDirection.Output}
                };
                await DataLink.Instance.ExecuteNonQuery("CreateUser", parameters);
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

        // on success returns the user with the given userId
        // on failure returns null
        public async Task<User?> GetUserById(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@id_user", userId)
                };
                DataTable dt = await DataLink.Instance.ExecuteReader("GetUserById", parameters);
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                DataRow dr = dt.Rows[0];
                return new User(
                    userId,
                    new Cnp(dr["cnp"].ToString() ?? ""),
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

        // updates the information of the user
        // on success returns true
        // on failure returns false
        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@id_user", user.UserID),
                    new SqlParameter("@cnp", user.Cnp.ToString()),
                    new SqlParameter("@first_name", user.FirstName),
                    new SqlParameter("@last_name", user.LastName),
                    new SqlParameter("@email", user.Email.ToString()),
                    new SqlParameter("@phone_number", user.PhoneNumber.ToString()),
                    new SqlParameter("@hashed_password", user.HashedPassword.GetHashedPassword()),
                    new SqlParameter("@password_salt", user.HashedPassword.GetSalt())
                };
                await DataLink.Instance.ExecuteNonQuery("UpdateUser", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"REPO: Error updating user: {ex.Message}");
                return false;
            }
        }

        // deletes the user with the given userId
        // on success returns true
        // on failure returns false
        public async Task<bool> DeleteUser()
        {
            int userId = int.Parse(UserSession.Instance.GetUserData("id_user") ?? "0");
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@id_user", userId)
                };
                await DataLink.Instance.ExecuteNonQuery("DeleteUser", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"REPO: Error deleting user: {ex.Message}");
                return false;
            }
        }

        // on success returns the user with the given cnp
        // on failure returns null
        public async Task<User?> GetUserByCnp(string cnp)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@cnp", cnp.ToString())
                };
                DataTable dt = await DataLink.Instance.ExecuteReader("GetUserByCnp", parameters);
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                DataRow dr = dt.Rows[0];
                return new User(
                    Convert.ToInt32(dr["id_user"]),
                    new Cnp(dr["cnp"].ToString() ?? ""),
                    dr["first_name"].ToString() ?? "",
                    dr["last_name"].ToString() ?? "",
                    new Email(dr["email"].ToString() ?? ""),
                    new PhoneNumber(dr["phone_number"].ToString() ?? ""),
                    new HashedPassword(dr["hashed_password"].ToString() ?? "", dr["password_salt"].ToString() ?? "", false)
                    );
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"REPO: Error getting user by cnp: {ex.Message}");
                return null;
            }
        }

        // on success returns the user with the given email
        // on failure returns null
        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@email", email.ToString())
                };
                DataTable dt = await DataLink.Instance.ExecuteReader("GetUserByEmail", parameters);
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                DataRow dr = dt.Rows[0];
                return new User(
                    Convert.ToInt32(dr["id_user"]),
                    new Cnp(dr["cnp"].ToString() ?? ""),
                    dr["first_name"].ToString() ?? "",
                    dr["last_name"].ToString() ?? "",
                    new Email(dr["email"].ToString() ?? ""),
                    new PhoneNumber(dr["phone_number"].ToString() ?? ""),
                    new HashedPassword(dr["hashed_password"].ToString() ?? "", dr["password_salt"].ToString() ?? "", false)
                    );
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"REPO: Error getting user by email: {ex.Message}");
                return null;
            }
        }

        // on success returns the user with the given phone number
        // on failure returns null
        public async Task<User?> GetUserByPhoneNumber(string phoneNumber)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@phone_number", phoneNumber.ToString())
                };
                DataTable dt = await DataLink.Instance.ExecuteReader("GetUserByPhoneNumber", parameters);
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                DataRow dr = dt.Rows[0];
                return new User(
                    Convert.ToInt32(dr["id_user"]),
                    new Cnp(dr["cnp"].ToString() ?? ""),
                    dr["first_name"].ToString() ?? "",
                    dr["last_name"].ToString() ?? "",
                    new Email(dr["email"].ToString() ?? ""),
                    new PhoneNumber(dr["phone_number"].ToString() ?? ""),
                    new HashedPassword(dr["hashed_password"].ToString() ?? "", dr["password_salt"].ToString() ?? "", false)
                    );
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"REPO: Error getting user by phone number: {ex.Message}");
                return null;
            }
        }
        public async Task<string[]> GetUserPasswordHashSalt()
        {
            int userID = int.Parse(UserSession.Instance.GetUserData("id_user") ?? "0");
            // grabs the hashed password and the salt for the user from the database
            var parameterList = new SqlParameter[]
            {
                   new SqlParameter("@id_user",userID) ,
                   new SqlParameter("@hashed_password", System.Data.SqlDbType.VarChar,255){Direction = ParameterDirection.Output},
                   new SqlParameter("@password_salt", System.Data.SqlDbType.VarChar,32){Direction = ParameterDirection.Output}
               };
            await DataLink.Instance.ExecuteNonQuery("GetHashedPassword", parameterList);
            string passwordHash = (string)parameterList.First(x=>x.ParameterName == "@hashed_password").Value;
            string salt = (string)parameterList.First(x => x.ParameterName == "@password_salt").Value;

            string[] list = { passwordHash, salt };
            return list;

        }

    }
}
