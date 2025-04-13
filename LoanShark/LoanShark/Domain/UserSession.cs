using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LoanShark.Domain
{
    public class UserSession
    {
        // a UserSession will have the following keys stored in the dictionary:
        // id_user, cnp, first_name, last_name, email, phone_number, current_bank_account_iban
        // for the hashed_password and password_salt, we will allways fetch from the database for security reasons
        private Dictionary<string, string?>? userData;
        private static UserSession? instance;
        private static readonly object LockObject = new object();
        public static UserSession Instance
        {
            get
            {
                lock (LockObject)
                {
                    if (instance == null)
                    {
                        instance = new UserSession();
                        Debug.Print("Empty UserSession instance created");
                    }
                    return instance;
                }
            }
        }
        // Make constructors private to prevent external instantiation
        private UserSession()
        {
            this.userData = new Dictionary<string, string?>();
            this.userData.Add("id_user", null);
            this.userData.Add("cnp", null);
            this.userData.Add("first_name", null);
            this.userData.Add("last_name", null);
            this.userData.Add("email", null);
            this.userData.Add("phone_number", null);
            this.userData.Add("current_bank_account_iban", null);
        }

        // Create a method to initialize user data
        public void Initialize(string id_user, string cnp, string first_name, string last_name, string email, string phone_number, string iban)
        {
            if (this.userData == null)
            {
                this.userData = new Dictionary<string, string?>();
            }

            this.userData["id_user"] = id_user;
            this.userData["cnp"] = cnp;
            this.userData["first_name"] = first_name;
            this.userData["last_name"] = last_name;
            this.userData["email"] = email;
            this.userData["phone_number"] = phone_number;
            this.userData["current_bank_account_iban"] = iban;
            Debug.Print("UserSession instantiated with given values");
        }

        public void SetUserData(string key, string value)
        {
            if (this.userData == null)
            {
                throw new Exception("UserSession is null");
            }
            // Check for the key because we initialize the object with all the possible keys
            if (!this.userData.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Cannot set the key {key} because it is invalid");
            }
            this.userData[key] = value;
            Debug.Print($"UserSession {key} was set to {value}");
        }

        public string? GetUserData(string key)
        {
            if (this.userData == null)
            {
                throw new Exception("UserSession is null");
            }
            if (!this.userData.ContainsKey(key))
            {
                throw new KeyNotFoundException("Key not found in UserSession");
            }
            return this.userData[key];
        }

        public void InvalidateUserSession()
        {
            // Instead of setting to null, create an empty dictionary
            // This way, any code that accesses it won't crash with null references
            this.userData = new Dictionary<string, string?>();
            this.userData.Add("id_user", null);
            this.userData.Add("cnp", null);
            this.userData.Add("first_name", null);
            this.userData.Add("last_name", null);
            this.userData.Add("email", null);
            this.userData.Add("phone_number", null);
            this.userData.Add("current_bank_account_iban", null);

            Debug.Print("UserSession invalidated safely");
        }
    }
}
