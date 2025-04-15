using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LoanShark.Data;
using LoanShark.Domain;
using Microsoft.Data.SqlClient;

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

    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {
        }
        private readonly DataLink dataLink;
        public UserRepository(DataLink dataLink)
        {
            dataLink = dataLink;
        }

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
                    new SqlParameter("@id_user", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };
                await DataLink.Instance.ExecuteNonQuery("CreateUser", parameters);
                int newUserId = (int)parameters.First(p => p.ParameterName == "@id_user").Value;
                user.UserID = newUserId;
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"REPO: Error creating user: {ex.Message}");
                throw;
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
                DataTable dataTable = await DataLink.Instance.ExecuteReader("GetUserById", parameters);
                if (dataTable.Rows.Count == 0)
                {
                    return null;
                }
                DataRow dataRow = dataTable.Rows[0];
                return new User(
                    userId,
                    new Cnp(dataRow["cnp"].ToString() ?? string.Empty),
                    dataRow["first_name"].ToString() ?? string.Empty,
                    dataRow["last_name"].ToString() ?? string.Empty,
                    new Email(dataRow["email"].ToString() ?? string.Empty),
                    new PhoneNumber(dataRow["phone_number"].ToString() ?? string.Empty),
                    new HashedPassword(dataRow["hashed_password"].ToString() ?? string.Empty, dataRow["password_salt"].ToString() ?? string.Empty, false));
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
                DataTable dataTable = await DataLink.Instance.ExecuteReader("GetUserByCnp", parameters);
                if (dataTable.Rows.Count == 0)
                {
                    return null;
                }
                DataRow dataRow = dataTable.Rows[0];
                return new User(
                    Convert.ToInt32(dataRow["id_user"]),
                    new Cnp(dataRow["cnp"].ToString() ?? string.Empty),
                    dataRow["first_name"].ToString() ?? string.Empty,
                    dataRow["last_name"].ToString() ?? string.Empty,
                    new Email(dataRow["email"].ToString() ?? string.Empty),
                    new PhoneNumber(dataRow["phone_number"].ToString() ?? string.Empty),
                    new HashedPassword(dataRow["hashed_password"].ToString() ?? string.Empty, dataRow["password_salt"].ToString() ?? string.Empty, false));
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
                DataTable dataTable = await DataLink.Instance.ExecuteReader("GetUserByEmail", parameters);
                if (dataTable.Rows.Count == 0)
                {
                    return null;
                }
                DataRow dataRow = dataTable.Rows[0];
                return new User(
                    Convert.ToInt32(dataRow["id_user"]),
                    new Cnp(dataRow["cnp"].ToString() ?? string.Empty),
                    dataRow["first_name"].ToString() ?? string.Empty,
                    dataRow["last_name"].ToString() ?? string.Empty,
                    new Email(dataRow["email"].ToString() ?? string.Empty),
                    new PhoneNumber(dataRow["phone_number"].ToString() ?? string.Empty),
                    new HashedPassword(dataRow["hashed_password"].ToString() ?? string.Empty, dataRow["password_salt"].ToString() ?? string.Empty, false));
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
                DataTable dataTable = await DataLink.Instance.ExecuteReader("GetUserByPhoneNumber", parameters);
                if (dataTable.Rows.Count == 0)
                {
                    return null;
                }
                DataRow dataRow = dataTable.Rows[0];
                return new User(
                    Convert.ToInt32(dataRow["id_user"]),
                    new Cnp(dataRow["cnp"].ToString() ?? string.Empty),
                    dataRow["first_name"].ToString() ?? string.Empty,
                    dataRow["last_name"].ToString() ?? string.Empty,
                    new Email(dataRow["email"].ToString() ?? string.Empty),
                    new PhoneNumber(dataRow["phone_number"].ToString() ?? string.Empty),
                    new HashedPassword(dataRow["hashed_password"].ToString() ?? string.Empty, dataRow["password_salt"].ToString() ?? string.Empty, false));
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
                   new SqlParameter("@id_user", userID),
                   new SqlParameter("@hashed_password", System.Data.SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output },
                   new SqlParameter("@password_salt", System.Data.SqlDbType.VarChar, 32) { Direction = ParameterDirection.Output }
               };
            await DataLink.Instance.ExecuteNonQuery("GetHashedPassword", parameterList);
            string passwordHash = (string)parameterList.First(parameter => parameter.ParameterName == "@hashed_password").Value;
            string salt = (string)parameterList.First(parameter => parameter.ParameterName == "@password_salt").Value;

            string[] list = { passwordHash, salt };
            return list;
        }
    }
}
