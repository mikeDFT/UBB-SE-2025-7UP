using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.Domain
{
    public class Email
    {
        private string _emailAddress;

        public Email(string emailAddress)
        {
            if (!IsValid(emailAddress))
            {
                throw new ArgumentException("Invalid email address");
            }
            _emailAddress = emailAddress;
        }

        public static bool IsValid(string emailAddress)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(emailAddress);
                return addr.Address == emailAddress;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return _emailAddress;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj is Email email &&
                   _emailAddress == email._emailAddress;
        }

        public override int GetHashCode()
        {
            return _emailAddress.GetHashCode();
        }
    }

    public class Cnp
    {
        private string _cnp;

        public Cnp(string cnp)
        {
            if (!IsValid(cnp))
            {
                throw new ArgumentException("Invalid CNP");
            }
            _cnp = cnp;
        }

        public static bool IsValid(string cnp)
        {
            if (cnp.Length != 13)
            {
                return false;
            }
            if (!cnp.All(char.IsDigit))
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return _cnp;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj is Cnp cnp &&
                   _cnp == cnp._cnp;
        }

        public override int GetHashCode()
        {
            return _cnp.GetHashCode();
        }
    }

    public class PhoneNumber
    {
        private string _phoneNumber;

        public PhoneNumber(string phoneNumber)
        {
            if (!IsValid(phoneNumber))
            {
                throw new ArgumentException("Invalid phone number");
            }
            _phoneNumber = phoneNumber;
        }

        public static bool IsValid(string phoneNumber)
        {
            if (phoneNumber.Length != 10)
            {
                return false;
            }
            if (!phoneNumber.All(char.IsDigit))
            {
                return false;
            }
            if (!phoneNumber.StartsWith("07"))
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return _phoneNumber.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj is PhoneNumber phoneNumber &&
                   _phoneNumber == phoneNumber._phoneNumber;
        }

        public override int GetHashCode()
        {
            return _phoneNumber.GetHashCode();
        }
    }

    public class User
    {
        public int UserID { get; set; }
        public Cnp Cnp { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Email Email { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
        public HashedPassword HashedPassword { get; set; }

        public User(int userID, Cnp cnp, string firstName, string lastName, Email email, PhoneNumber phoneNumber, HashedPassword hashedPassword)
        {
            UserID = userID;
            Cnp = cnp;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            HashedPassword = hashedPassword;
        }
    }
}
