using LoanShark.Domain;
using LoanShark.Service;
using LoanShark.View;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoanShark.ViewModel
{
    public class UserInformationViewModel : INotifyPropertyChanged
    {
        private UserService _userService = new UserService();
        private User? user;
        private string _firstName = "";
        private string _lastName = "";
        private string _phoneNumber = "";
        private string _email = "";
        private string _currentPassword = "";
        private string _newPassword = "";
        private string _confirmNewPassword = "";
        private string _firstNameError = "";
        private string _lastNameError = "";
        private string _phoneNumberError = "";
        private string _emailError = "";
        private string _currentPasswordError = "";
        private string _newPasswordError = "";
        private string _confirmNewPasswordError = "";
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

        public string CurrentPassword
        {
            get => _currentPassword;
            set
            {
                if (_currentPassword != value)
                {
                    _currentPassword = value;
                    OnPropertyChanged(nameof(CurrentPassword));
                    CurrentPasswordError = string.Empty;
                }
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                if (_newPassword != value)
                {
                    _newPassword = value;
                    OnPropertyChanged(nameof(NewPassword));
                    NewPasswordError = string.Empty;
                }
            }
        }

        public string ConfirmNewPassword
        {
            get => _confirmNewPassword;
            set
            {
                if (_confirmNewPassword != value)
                {
                    _confirmNewPassword = value;
                    OnPropertyChanged(nameof(ConfirmNewPassword));
                    ConfirmNewPasswordError = string.Empty;
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

        public string CurrentPasswordError
        {
            get => _currentPasswordError;
            set
            {
                if (_currentPasswordError != value)
                {
                    _currentPasswordError = value;
                    OnPropertyChanged(nameof(CurrentPasswordError));
                }
            }
        }

        public string NewPasswordError
        {
            get => _newPasswordError;
            set
            {
                if (_newPasswordError != value)
                {
                    _newPasswordError = value;
                    OnPropertyChanged(nameof(NewPasswordError));
                }
            }
        }

        public string ConfirmNewPasswordError
        {
            get => _confirmNewPasswordError;
            set
            {
                if (_confirmNewPasswordError != value)
                {
                    _confirmNewPasswordError = value;
                    OnPropertyChanged(nameof(ConfirmNewPasswordError));
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

        public ICommand CloseCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        public ICommand DeleteCommand { get; set; }
        public Action? CloseAction { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserInformationViewModel()
        {
            UpdateCommand = new RelayCommand(async () => await UpdateUser());
            DeleteCommand = new RelayCommand(DeleteUser);
            CloseCommand = new RelayCommand(() => CloseAction?.Invoke());
            FirstName = UserSession.Instance.GetUserData("first_name") ?? "";
            LastName = UserSession.Instance.GetUserData("last_name") ?? "";
            PhoneNumber = UserSession.Instance.GetUserData("phone_number") ?? "";
            Email = UserSession.Instance.GetUserData("email") ?? "";
        }

        // validates all fields and updates user information
        public async Task UpdateUser()
        {
            if (user  == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                FirstNameError = "First name is required";
            }
            if (string.IsNullOrWhiteSpace(LastName))
            {
                LastNameError = "Last name is required";
            }
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                PhoneNumberError = "Phone number is required";
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Email is required";
            }
            if (CurrentPassword == null || CurrentPassword.Length == 0)
            {
                CurrentPasswordError = "Enter your current password";
            }
            else
            {
                HashedPassword hashedPassword = new HashedPassword(CurrentPassword, user.HashedPassword.GetSalt(), true);
                if (!hashedPassword.Equals(user.HashedPassword))
                {
                    CurrentPasswordError = "Wrong password";
                }
            }
            if (NewPassword != "")
            {
                // [lengthOk, uppserCaseOk, lowerCaseOk, numberOk, specialCharOk];
                bool[] result = HashedPassword.VerifyPasswordStrength(NewPassword);
                if (!result[0])
                {
                    NewPasswordError = "Password must have at least 8 characters";
                }
                if (!result[1])
                {
                    NewPasswordError = "Password must contain at least one uppercase letter";
                }
                if (!result[2])
                {
                    NewPasswordError = "Password must contain at least one lowercase letter";
                }
                if (!result[3])
                {
                    NewPasswordError = "Password must contain at least one number";
                }
                if (!result[4])
                {
                    NewPasswordError = "Password must contain at least one special character";
                }
            }
            if (NewPassword != ConfirmNewPassword)
            {
                ConfirmNewPasswordError = "Passwords do not match";
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
                PhoneNumberError,
                EmailError,
                CurrentPasswordError,
                NewPasswordError,
                ConfirmNewPasswordError
            };

            bool hasErrors = errors.Any(x => x != "");

            if (hasErrors)
            {
                return;
            }

            // Update user
            try
            {
                user.FirstName = FirstName;
                user.LastName = LastName;
                user.PhoneNumber = new PhoneNumber(PhoneNumber);
                user.Email = new Email(Email);
                if (NewPassword != "")
                {
                    user.HashedPassword = new HashedPassword(NewPassword);
                }
                bool ok = await _userService.UpdateUser(user);
                if (!ok)
                {
                    ErrorMessage = "Failed to update user information";
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }

            // Close the window
            CloseCommand.Execute(null);
        }

        // opens the delete user confirmation window
        public void DeleteUser()
        {
            if (user == null)
            {
                return;
            }
            DeleteAccountView m_window = new DeleteAccountView(user.UserID);
            m_window.Activate();
        }
    }
}
