using System.Collections.Generic;
using System.Diagnostics;

namespace LoanShark.Domain
{
    class UserSession
    {
        // a UserSession will have the following keys stored in the dictionary:
        // id_user, cnp, first_name, last_name, email, phone_number
        // for the hashed_password and password_salt, we will allways fetch from the database for security reasons
        private Dictionary<string, string?> userData;
        
        public UserSession()
        {
            this.userData = new Dictionary<string, string?>();
            this.userData.Add("id_user", null);
            this.userData.Add("cnp", null);
            this.userData.Add("first_name", null);
            this.userData.Add("last_name", null);
            this.userData.Add("email", null);
            this.userData.Add("phone_number", null);
            Debug.Print("Null UserSession created");
        }

        public UserSession(string id_user, string cnp, string first_name, string last_name, string email, string phone_number)
        {
            this.userData = new Dictionary<string, string?>();
            this.userData.Add("id_user", id_user);
            this.userData.Add("cnp", cnp);
            this.userData.Add("first_name", first_name);
            this.userData.Add("last_name", last_name);
            this.userData.Add("email", email);
            this.userData.Add("phone_number", phone_number);
            Debug.Print("UserSession instantiated with given values");
        }

        public void SetUserData(string key, string value)
        {
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
            if (!this.userData.ContainsKey(key))
            {
                throw new KeyNotFoundException("Key not found in UserSession");
            }
            return this.userData[key];
        }
    }
}
