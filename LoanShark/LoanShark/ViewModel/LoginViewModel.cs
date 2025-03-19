using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LoanShark.Service;

namespace LoanShark.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly LoginService loginService;
        private string email;
        private string errorMessage;
        private bool isErrorVisible;
        
        public event PropertyChangedEventHandler? PropertyChanged;

        public LoginViewModel()
        {
            this.email = string.Empty;
            this.errorMessage = string.Empty;
            this.isErrorVisible = false;
            this.loginService = new LoginService();
        }

        public string Email
        {
            get => email;
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        public bool IsErrorVisible
        {
            get => isErrorVisible;
            set
            {
                if (isErrorVisible != value)
                {
                    isErrorVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool ValidateCredentials(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ErrorMessage = "Email or password cannot be empty";
                IsErrorVisible = true;
                return false;
            }
            
            IsErrorVisible = false;
            bool isValid = this.loginService.ValidateUserCredentials(email, password);
            
            if (!isValid)
            {
                ErrorMessage = "Invalid email or password";
                IsErrorVisible = true;
            }
            else
            {
                ErrorMessage = string.Empty;
                IsErrorVisible = false;
            }
            
            return isValid;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
