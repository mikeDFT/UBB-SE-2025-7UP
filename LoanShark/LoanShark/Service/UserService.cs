using LoanShark.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using LoanShark.Domain;

namespace LoanShark.Service
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public void DeleteUser(int UserID, string password)
        {
            // gets the hashed password and salt from the database for the current user
            // creates a hashed password with the sald from the database for the user inputed password
            // if the two passwords match, proceeds to delete the user and its bank accounts from the database
            string[] listParameters = _userRepository.GetUserPasswordHashSalt(UserID);
            var userInputedPassword = new HashedPassword(password, listParameters[1], true);
            var dataBasePassword = new HashedPassword(listParameters[0], listParameters[1], false);


            if( userInputedPassword == dataBasePassword)
                _userRepository.DeleteUser(UserID);

        }
    }
}