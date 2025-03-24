using LoanShark.Domain;
using LoanShark.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoanShark.ViewModel
{
    public class UserRegistrationViewModel: INotifyPropertyChanged
    {
        private UserService _userService = new UserService();
        private string _firstName = "";
        private string _lastName = "";
        private string _cnp = "";
        private string _phoneNumber = "";
        private string _email = "";
        private string _password = "";
        private string _confirmPassword = "";
        private string _firstNameError = "";
        private string _lastNameError = "";
        private string _cnpError = "";
        private string _phoneNumberError = "";
        private string _emailError = "";
        private string _passwordError = "";
        private string _confirmPasswordError = "";
        private string _errorMessage = "";

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                    FirstNameError = string.Empty;
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                    LastNameError = string.Empty;
                }
            }
        }

        public string Cnp
        {
            get => _cnp;
            set
            {
                if (_cnp != value)
                {
                    _cnp = value;
                    OnPropertyChanged(nameof(Cnp));
                    CnpError = string.Empty;
                }
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                    PhoneNumberError = string.Empty;
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                    EmailError = string.Empty;
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                    PasswordError = string.Empty;
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    OnPropertyChanged(nameof(ConfirmPassword));
                    ConfirmPasswordError = string.Empty;
                }
            }
        }

        public string FirstNameError
        {
            get => _firstNameError;
            set
            {
                if (_firstNameError != value)
                {
                    _firstNameError = value;
                    OnPropertyChanged(nameof(FirstNameError));
                }
            }
        }

        public string LastNameError
        {
            get => _lastNameError;
            set
            {
                if (_lastNameError != value)
                {
                    _lastNameError = value;
                    OnPropertyChanged(nameof(LastNameError));
                }
            }
        }

        public string CnpError
        {
            get => _cnpError;
            set
            {
                if (_cnpError != value)
                {
                    _cnpError = value;
                    OnPropertyChanged(nameof(CnpError));
                }
            }
        }

        public string PhoneNumberError
        {
            get => _phoneNumberError;
            set
            {
                if (_phoneNumberError != value)
                {
                    _phoneNumberError = value;
                    OnPropertyChanged(nameof(PhoneNumberError));
                }
            }
        }

        public string EmailError
        {
            get => _emailError;
            set
            {
                if (_emailError != value)
                {
                    _emailError = value;
                    OnPropertyChanged(nameof(EmailError));
                }
            }
        }

        public string PasswordError
        {
            get => _passwordError;
            set
            {
                if (_passwordError != value)
                {
                    _passwordError = value;
                    OnPropertyChanged(nameof(PasswordError));
                }
            }
        }

        public string ConfirmPasswordError
        {
            get => _confirmPasswordError;
            set
            {
                if (_confirmPasswordError != value)
                {
                    _confirmPasswordError = value;
                    OnPropertyChanged(nameof(ConfirmPasswordError));
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
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

            //return [lengthOk, uppserCaseOk, lowerCaseOk, numberOk, specialCharOk];
            bool[] result = HashedPassword.VerifyPasswordStrength(Password);
            PasswordError = "";
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

            string? cnpError = await _userService.CheckCnp(Cnp);
            if (cnpError != null)
            {
                CnpError = cnpError;
            }

            string? emailError = await _userService.CheckEmail(Email);
            if (emailError != null)
            {
                EmailError = emailError;
            }

            string? phoneNumberError = await _userService.CheckPhoneNumber(PhoneNumber);
            if (phoneNumberError != null)
            {
                PhoneNumberError = phoneNumberError;
            }

            List<string> errors = new List<string> {
                FirstNameError,
                LastNameError,
                CnpError,
                PhoneNumberError,
                EmailError,
                PasswordError,
                ConfirmPasswordError
            };
            bool hasErrors = errors.Any(x => x != "");
            if (hasErrors)
            {
                return;
            }

            // Create user
            try
            {
                await _userService.CreateUser(Cnp, FirstName, LastName, Email, PhoneNumber, Password);
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
