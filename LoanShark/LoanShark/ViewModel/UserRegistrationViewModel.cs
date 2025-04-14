using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LoanShark.Domain;
using LoanShark.Service;

namespace LoanShark.ViewModel
{
    public class UserRegistrationViewModel : INotifyPropertyChanged
    {
        private UserService userService = new UserService();
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string cnp = string.Empty;
        private string phoneNumber = string.Empty;
        private string email = string.Empty;
        private string password = string.Empty;
        private string confirmPassword = string.Empty;
        private string firstNameError = string.Empty;
        private string lastNameError = string.Empty;
        private string cnpError = string.Empty;
        private string phoneNumberError = string.Empty;
        private string emailError = string.Empty;
        private string passwordError = string.Empty;
        private string confirmPasswordError = string.Empty;
        private string errorMessage = string.Empty;

        public string FirstName
        {
            get => firstName;
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                    FirstNameError = string.Empty;
                }
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    OnPropertyChanged(nameof(LastName));
                    LastNameError = string.Empty;
                }
            }
        }

        public string Cnp
        {
            get => cnp;
            set
            {
                if (cnp != value)
                {
                    cnp = value;
                    OnPropertyChanged(nameof(Cnp));
                    CnpError = string.Empty;
                }
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (phoneNumber != value)
                {
                    phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                    PhoneNumberError = string.Empty;
                }
            }
        }

        public string Email
        {
            get => email;
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                    EmailError = string.Empty;
                }
            }
        }

        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                    PasswordError = string.Empty;
                }
            }
        }

        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                if (confirmPassword != value)
                {
                    confirmPassword = value;
                    OnPropertyChanged(nameof(ConfirmPassword));
                    ConfirmPasswordError = string.Empty;
                }
            }
        }

        public string FirstNameError
        {
            get => firstNameError;
            set
            {
                if (firstNameError != value)
                {
                    firstNameError = value;
                    OnPropertyChanged(nameof(FirstNameError));
                }
            }
        }

        public string LastNameError
        {
            get => lastNameError;
            set
            {
                if (lastNameError != value)
                {
                    lastNameError = value;
                    OnPropertyChanged(nameof(LastNameError));
                }
            }
        }

        public string CnpError
        {
            get => cnpError;
            set
            {
                if (cnpError != value)
                {
                    cnpError = value;
                    OnPropertyChanged(nameof(CnpError));
                }
            }
        }

        public string PhoneNumberError
        {
            get => phoneNumberError;
            set
            {
                if (phoneNumberError != value)
                {
                    phoneNumberError = value;
                    OnPropertyChanged(nameof(PhoneNumberError));
                }
            }
        }

        public string EmailError
        {
            get => emailError;
            set
            {
                if (emailError != value)
                {
                    emailError = value;
                    OnPropertyChanged(nameof(EmailError));
                }
            }
        }

        public string PasswordError
        {
            get => passwordError;
            set
            {
                if (passwordError != value)
                {
                    passwordError = value;
                    OnPropertyChanged(nameof(PasswordError));
                }
            }
        }

        public string ConfirmPasswordError
        {
            get => confirmPasswordError;
            set
            {
                if (confirmPasswordError != value)
                {
                    confirmPasswordError = value;
                    OnPropertyChanged(nameof(ConfirmPasswordError));
                }
            }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        public ICommand CloseCommand { get; }
        public ICommand RegisterCommand { get; }
        public Action? CloseAction { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserRegistrationViewModel()
        {
            RegisterCommand = new RelayCommand(async () => await CreateUser());
            CloseCommand = new RelayCommand(() => CloseAction?.Invoke());
        }

        // validates all fields and creates a new user
        public async Task CreateUser()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                FirstNameError = "First name is required";
            }
            if (string.IsNullOrWhiteSpace(LastName))
            {
                LastNameError = "Last name is required";
            }
            if (string.IsNullOrWhiteSpace(Cnp))
            {
                CnpError = "CNP is required";
            }
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                PhoneNumberError = "Phone number is required";
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Email is required";
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "Password is required";
            }
            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ConfirmPasswordError = "Confirm password is required";
            }
            if (Password != ConfirmPassword)
            {
                ConfirmPasswordError = "Passwords do not match";
            }

            // return [lengthOk, uppserCaseOk, lowerCaseOk, numberOk, specialCharOk];
            bool[] result = HashedPassword.VerifyPasswordStrength(Password);
            PasswordError = string.Empty;
            if (!result[0])
            {
                PasswordError += "Password must have at least 8 characters\n";
            }
            if (!result[1])
            {
                PasswordError += "Password must contain at least one uppercase letter\n";
            }
            if (!result[2])
            {
                PasswordError += "Password must contain at least one lowercase letter\n";
            }
            if (!result[3])
            {
                PasswordError += "Password must contain at least one number\n";
            }
            if (!result[4])
            {
                PasswordError += "Password must contain at least one special character\n";
            }

            string? cnpError = await userService.CheckCnp(Cnp);
            if (cnpError != null)
            {
                CnpError = cnpError;
            }

            string? emailError = await userService.CheckEmail(Email);
            if (emailError != null)
            {
                EmailError = emailError;
            }

            string? phoneNumberError = await userService.CheckPhoneNumber(PhoneNumber);
            if (phoneNumberError != null)
            {
                PhoneNumberError = phoneNumberError;
            }

            List<string> errors = new List<string>
            {
                FirstNameError,
                LastNameError,
                CnpError,
                PhoneNumberError,
                EmailError,
                PasswordError,
                ConfirmPasswordError
            };
            bool hasErrors = errors.Any(x => x != string.Empty);
            if (hasErrors)
            {
                return;
            }

            // Create user
            try
            {
                await userService.CreateUser(Cnp, FirstName, LastName, Email, PhoneNumber, Password);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            // Close the window
            CloseCommand.Execute(null);
        }
    }
}
