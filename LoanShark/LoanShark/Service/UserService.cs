using LoanShark.Domain;
using LoanShark.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public User? GetUserInformation(int userId)
        {
            return _userRepository.GetUserById(userId);
        }    

        public bool UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);
        }

        public string DeleteUser(int UserID, string password)
        {
            User? user = GetUserInformation(UserID);
            if (user == null)
            {
                return "User doesn't exist";
            }
            // gets the hashed password and salt from the database for the current user
            // creates a hashed password with the sald from the database for the user inputed password
            // if the two passwords match, proceeds to delete the user and its bank accounts from the database
            var userInputedPassword = new HashedPassword(password, user.HashedPassword.GetSalt(), true);
            var dataBasePassword = user.HashedPassword;

            // sa il intreb pe alex ce fel de salt imi trebuie


            if (userInputedPassword.Equals(dataBasePassword))
            {
                _userRepository.DeleteUser(UserID);

                // after the user is deleted from the database, he should be logged out of the session
                // for the moment, it just just exits the whole app
                // TODO Alex : log out functionality
                Environment.Exit(0);
                return "Succes";
            }
            else
            {
                Debug.WriteLine("Wrong password");
                return "Wrong password";
            }
        }
    }
}
