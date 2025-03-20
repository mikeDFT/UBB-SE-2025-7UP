using LoanShark.Domain;
using LoanShark.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.Service
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public string? CheckCnp(int userId, string cnp)
        {
            if (cnp.Length != 13)
            {
                return "CNP must have 13 characters";
            }
            if (!cnp.All(char.IsDigit))
            {
                return "CNP must contain only digits";
            }
            User? user = _userRepository.GetUserByCnp(new Cnp(cnp));
            if (user != null && user.UserID != userId)
            {
                return "CNP already in use";
            }
            return null;
        }

        public string? CheckEmail(int userId, string email)
        {
            if (!Email.IsValid(email))
            {
                return "Invalid email address";
            }
            User? user = _userRepository.GetUserByEmail(new Email(email));
            if (user != null && user.UserID != userId)
            {
                return "Email already in use";
            }
            return null;
        }

        public string? CheckPhoneNumber(int userId, string phoneNumber)
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
            User? user = _userRepository.GetUserByPhoneNumber(new PhoneNumber(phoneNumber));
            if (user != null && user.UserID != userId)
            {
                return "Phone number already in use";
            }
            return null;
        }

        public void CreateUser(string cnp, string firstName, string lastName, string email, string phoneNumber, string password)
        {
            User user = new User(
                -1,
                new Cnp(cnp),
                firstName,
                lastName,
                new Email(email),
                new PhoneNumber(phoneNumber),
                new HashedPassword(password)
            );
            _userRepository.CreateUser(user);
        }
    }
}
