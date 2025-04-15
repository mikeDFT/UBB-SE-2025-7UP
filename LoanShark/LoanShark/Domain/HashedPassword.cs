using System;
using System.Linq;

namespace LoanShark.Domain
{
    public class HashedPassword
    {
        private string hashedPassword;
        private string salt;

        // this constructor will be used when a user needs to be created
        // because it automatically generates a HashedPassword if the password meets the criterias specified bellow
        // after the password is generated, use the GetSalt() and getHashedPassword() methods to extract the salt and hash
        // in order to populate the db with the correct data
        public HashedPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("Password cannot be null");
            }
            if (IsPasswordOk(password) == false)
            {
                throw new ArgumentException("Password does not meet the criteria");
            }
            this.salt = BCrypt.Net.BCrypt.GenerateSalt(10); // this will generate a 29 character string
            this.hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, this.salt); // this will hash the password with the generated salt
        }

        // this constructor will be used to create a HashedPassword object with the data retrieved
        // from the database which will allways be valid
        public HashedPassword(string someString, string salt, bool needsHashing)
        {
            this.salt = salt;
            // if this is the password data retrieved from the database, no need for hashing
            if (!needsHashing)
            {
                this.hashedPassword = someString;
            }
            // else, hash the password, this will be used for the part in which we take the password inputted from the user
            // and create a HashedPassword object with it
            else
            {
               this.hashedPassword = BCrypt.Net.BCrypt.HashPassword(someString, this.salt);
            }
        }

        public string GetSalt()
        {
            return this.salt;
        }

        public string GetHashedPassword()
        {
            return this.hashedPassword;
        }

        // This will be used for verifying the strength of the password when a user is created
        // This method will return an array of booleans that will indicate if the password meets the following criteria:
        // 1. At least 8 characters long
        // 2. Contains at least one uppercase letter
        // 3. Contains at least one lowercase letter
        // 4. Contains at least one digit
        // 5. Contains at least one special character
        public static bool[] VerifyPasswordStrength(string password)
        {
            bool lengthOk = false, upperCaseOk = false, lowerCaseOk = false, numberOk = false, specialCharOk = false;
            if (password.Length >= 8)
            {
                lengthOk = true;
            }
            if (password.Any(char.IsUpper))
            {
                upperCaseOk = true;
            }
            if (password.Any(char.IsLower))
            {
                lowerCaseOk = true;
            }
            if (password.Any(char.IsDigit))
            {
                numberOk = true;
            }
            if (!password.All(char.IsLetterOrDigit))
            {
                specialCharOk = true;
            }

            return new[] { lengthOk, upperCaseOk, lowerCaseOk, numberOk, specialCharOk };
        }

        private bool IsPasswordOk(string password)
        {
            bool[] verifications = VerifyPasswordStrength(password);
            return verifications.All(x => x == true);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("HashedPassword.Equals(): argument is null");
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            HashedPassword hashedObj = (HashedPassword)obj;

            return hashedObj.salt == this.salt && hashedObj.hashedPassword == this.hashedPassword;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
