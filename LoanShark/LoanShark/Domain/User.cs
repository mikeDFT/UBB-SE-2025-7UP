using System;
using System.Linq;

namespace LoanShark.Domain
{
    public class Email
    {
        private string emailAddress;

        public Email(string emailAddress)
        {
            if (!IsValid(emailAddress))
            {
                throw new ArgumentException("Invalid email address");
            }
            this.emailAddress = emailAddress;
        }

        // checks if a string is a valid email address
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
            return emailAddress;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj is Email email &&
                   emailAddress == email.emailAddress;
        }

        public override int GetHashCode()
        {
            return emailAddress.GetHashCode();
        }
    }

    public class Cnp
    {
        private string cnp;

        public Cnp(string cnp)
        {
            if (!IsValid(cnp))
            {
                throw new ArgumentException("Invalid CNP");
            }
            this.cnp = cnp;
        }

        // checks if a string is a valid cnp
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
            return cnp;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj is Cnp cnpp &&
                   cnp == cnpp.cnp;
        }

        public override int GetHashCode()
        {
            return cnp.GetHashCode();
        }
    }

    public class PhoneNumber
    {
        private string phoneNumber;

        public PhoneNumber(string phoneNumber)
        {
            if (!IsValid(phoneNumber))
            {
                throw new ArgumentException("Invalid phone number");
            }
            this.phoneNumber = phoneNumber;
        }

        // checks if a string is a valid phone number
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
            return phoneNumber.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj is PhoneNumber phoneNumberr &&
                   phoneNumber == phoneNumberr.phoneNumber;
        }

        public override int GetHashCode()
        {
            return phoneNumber.GetHashCode();
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
