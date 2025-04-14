using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LoanShark.Domain;
using LoanShark.Helper;
using LoanShark.Service;
using LoanShark.View;

namespace LoanShark.ViewModel
{
    public class UserInformationViewModel : INotifyPropertyChanged
    {
        private UserService userService = new UserService();
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string phoneNumber = string.Empty;
        private string email = string.Empty;
        private string currentPassword = string.Empty;
        private string newPassword = string.Empty;
        private string confirmNewPassword = string.Empty;
        private string firstNameError = string.Empty;
        private string lastNameError = string.Empty;
        private string phoneNumberError = string.Empty;
        private string emailError = string.Empty;
        private string currentPasswordError = string.Empty;
        private string newPasswordError = string.Empty;
        private string confirmNewPasswordError = string.Empty;
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

        public string CurrentPassword
        {
            get => currentPassword;
            set
            {
                if (currentPassword != value)
                {
                    currentPassword = value;
                    OnPropertyChanged(nameof(CurrentPassword));
                    CurrentPasswordError = string.Empty;
                }
            }
        }

        public string NewPassword
        {
            get => newPassword;
            set
            {
                if (newPassword != value)
                {
                    newPassword = value;
                    OnPropertyChanged(nameof(NewPassword));
                    NewPasswordError = string.Empty;
                }
            }
        }

        public string ConfirmNewPassword
        {
            get => confirmNewPassword;
            set
            {
                if (confirmNewPassword != value)
                {
                    confirmNewPassword = value;
                    OnPropertyChanged(nameof(ConfirmNewPassword));
                    ConfirmNewPasswordError = string.Empty;
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

        public string CurrentPasswordError
        {
            get => currentPasswordError;
            set
            {
                if (currentPasswordError != value)
                {
                    currentPasswordError = value;
                    OnPropertyChanged(nameof(CurrentPasswordError));
                }
            }
        }

        public string NewPasswordError
        {
            get => newPasswordError;
            set
            {
                if (newPasswordError != value)
                {
                    newPasswordError = value;
                    OnPropertyChanged(nameof(NewPasswordError));
                }
            }
        }

        public string ConfirmNewPasswordError
        {
            get => confirmNewPasswordError;
            set
            {
                if (confirmNewPasswordError != value)
                {
                    confirmNewPasswordError = value;
                    OnPropertyChanged(nameof(ConfirmNewPasswordError));
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
            FirstName = UserSession.Instance.GetUserData("first_name") ?? string.Empty;
            LastName = UserSession.Instance.GetUserData("last_name") ?? string.Empty;
            PhoneNumber = UserSession.Instance.GetUserData("phone_number") ?? string.Empty;
            Email = UserSession.Instance.GetUserData("email") ?? string.Empty;
        }

        // validates all fields and updates user information
        public async Task UpdateUser()
        {
            if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                CurrentPasswordError = "Password required in order to update user information";
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

            string[] userCredentialsFromDB = await userService.GetUserPasswordHashSalt();
            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError = "Email is required";
            }
            else
            {
                HashedPassword hashedPassword = new HashedPassword(CurrentPassword, userCredentialsFromDB[1], true);
                HashedPassword hashedPasswordFromDB = new HashedPassword(userCredentialsFromDB[0], userCredentialsFromDB[1], false);
                if (!hashedPassword.Equals(hashedPasswordFromDB))
                {
                    CurrentPasswordError = "Invalid password";
                    return;
                }
            }

            if (NewPassword != string.Empty)
            {
                // [lengthOk, uppserCaseOk, lowerCaseOk, numberOk, specialCharOk];
                bool[] result = HashedPassword.VerifyPasswordStrength(NewPassword);
                NewPasswordError = string.Empty;
                if (!result[0])
                {
                    NewPasswordError += "Password must have at least 8 characters\n";
                }
                if (!result[1])
                {
                    NewPasswordError += "Password must contain at least one uppercase letter\n";
                }
                if (!result[2])
                {
                    NewPasswordError += "Password must contain at least one lowercase letter\n";
                }
                if (!result[3])
                {
                    NewPasswordError += "Password must contain at least one number\n";
                }
                if (!result[4])
                {
                    NewPasswordError += "Password must contain at least one special character\n";
                }
            }
            if (NewPassword != ConfirmNewPassword)
            {
                ConfirmNewPasswordError = "Passwords do not match";
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
                PhoneNumberError,
                EmailError,
                CurrentPasswordError,
                NewPasswordError,
                ConfirmNewPasswordError
            };

            bool hasErrors = errors.Any(x => x != string.Empty);

            if (hasErrors)
            {
                return;
            }

            // Update user
            try
            {
                User user;
                if (string.IsNullOrWhiteSpace(NewPassword))
                {
                    // if new password is not provided, use the old one
                    user = new User(
                        int.Parse(UserSession.Instance.GetUserData("id_user") ?? "0"),
                        new Cnp(UserSession.Instance.GetUserData("cnp") ?? string.Empty),
                        FirstName,
                        LastName,
                        new Email(Email),
                        new PhoneNumber(PhoneNumber),
                        new HashedPassword(userCredentialsFromDB[0], userCredentialsFromDB[1], false));
                }
                else
                {
                    // if new password is provided, use the new one
                    user = new User(
                        int.Parse(UserSession.Instance.GetUserData("id_user") ?? "0"),
                        new Cnp(UserSession.Instance.GetUserData("cnp") ?? string.Empty),
                        FirstName,
                        LastName,
                        new Email(Email),
                        new PhoneNumber(PhoneNumber),
                        new HashedPassword(NewPassword));
                }

                bool ok = await userService.UpdateUser(user);
                if (!ok)
                {
                    ErrorMessage = "Failed to update user information";
                    return;
                }
                // update user session
                UserSession.Instance.SetUserData("first_name", FirstName);
                UserSession.Instance.SetUserData("last_name", LastName);
                UserSession.Instance.SetUserData("email", Email);
                UserSession.Instance.SetUserData("phone_number", PhoneNumber);
                WindowManager.ShouldReloadWelcomeText = true;
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
            DeleteAccountView deleteAccountWindow = new DeleteAccountView();
            deleteAccountWindow.Activate();
        }
    }
}
