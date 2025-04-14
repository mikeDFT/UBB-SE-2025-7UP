using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.Repository;

namespace LoanShark.Service
{
    public class UserService
    {
        private readonly IUserRepository userRepository;

        public UserService()
        {
            userRepository = new UserRepository();
        }

        // if cnp is valid and not used by another user returns null
        // otherwise returns an error message
        public async Task<string?> CheckCnp(string cnp)
        {
            if (cnp.Length != 13)
            {
                return "CNP must have 13 characters";
            }
            if (!cnp.All(char.IsDigit))
            {
                return "CNP must contain only digits";
            }
            User? user = await userRepository.GetUserByCnp(cnp);
            if (user != null && user.UserID != int.Parse(UserSession.Instance.GetUserData("id_user") ?? "0"))
            {
                return "CNP already in use";
            }
            return null;
        }

        // if email is valid and not used by another user returns null
        // otherwise returns an error message
        public async Task<string?> CheckEmail(string email)
        {
            if (!Email.IsValid(email))
            {
                return "Invalid email address";
            }
            User? user = await userRepository.GetUserByEmail(email);
            if (user != null && user.UserID != int.Parse(UserSession.Instance.GetUserData("id_user") ?? "0"))
            {
                return "Email already in use";
            }
            return null;
        }

        // if phoneNumber is valid and not used by another user returns null
        // otherwise returns an error message
        public async Task<string?> CheckPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length != 10)
            {
                return "Phone number must have 10 characters";
            }
            if (!phoneNumber.All(char.IsDigit))
            {
                return "Phone number must contain only digits";
            }
            if (!phoneNumber.StartsWith("07"))
            {
                return "Phone number must start with 07";
            }
            User? user = await userRepository.GetUserByPhoneNumber(phoneNumber);
            if (user != null && user.UserID != int.Parse(UserSession.Instance.GetUserData("id_user") ?? "0"))
            {
                return "Phone number already in use";
            }
            return null;
        }

        // creates a new user
        public async Task CreateUser(string cnp, string firstName, string lastName, string email, string phoneNumber, string password)
        {
            User user = new User(
                -1,
                new Cnp(cnp),
                firstName,
                lastName,
                new Email(email),
                new PhoneNumber(phoneNumber),
                new HashedPassword(password));
            await userRepository.CreateUser(user);
        }

        // on success returns the given user with the given userId
        // on failure returns null
        public async Task<User?> GetUserInformation()
        {
            return await userRepository.GetUserById(int.Parse(UserSession.Instance.GetUserData("id_user") ?? "0"));
        }

        // updates the information of the user
        // on success returns true
        // on failure returns false
        public async Task<bool> UpdateUser(User user)
        {
            return await userRepository.UpdateUser(user);
        }

        public async Task<string> DeleteUser(string password)
        {
            User? user = await GetUserInformation();
            if (user == null)
            {
                return "User doesn't exist";
            }

            // gets the hashed password and salt from the database for the current user
            // creates a hashed password with the salt from the database for the user inputed password
            // if the two passwords match, proceeds to delete the user and its bank accounts from the database
            var userInputedPassword = new HashedPassword(password, user.HashedPassword.GetSalt(), true);
            var dataBasePassword = user.HashedPassword;

            if (userInputedPassword.Equals(dataBasePassword))
            {
                await userRepository.DeleteUser();

                // after the user is deleted from the database, he should be logged out of the session
                return "Succes";
            }
            else
            {
                Debug.WriteLine("Wrong password");
                return "Wrong password";
            }
        }

        public async Task<string[]> GetUserPasswordHashSalt()
        {
            return await userRepository.GetUserPasswordHashSalt();
        }
    }
}
