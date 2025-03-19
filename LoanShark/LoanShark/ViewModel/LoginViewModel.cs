using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LoanShark.Service;
using Microsoft.UI.Xaml;

namespace LoanShark.ViewModel
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private readonly LoginService loginService;
        private string email;
        private string errorMessage;
        private bool isErrorVisible;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel()
        {
            this.loginService = new LoginService();
            this.isErrorVisible = false;
        }

        public string GetEmail()
        {
            return this.email;
        }

        public void SetEmail(string value)
        {
            if (this.email != value)
            {
                this.email = value;
                OnPropertyChanged(nameof(GetEmail));
            }
        }


        public string GetErrorMessage()
        {
            return this.errorMessage;
        }

        public void SetErrorMessage(string value)
        {
            if (this.errorMessage != value)
            {
                this.errorMessage = value;
                OnPropertyChanged(nameof(GetErrorMessage));
            }
        }

        public bool GetIsErrorVisible()
        {
            return this.isErrorVisible;
        }

        public void SetIsErrorVisible(bool value)
        {
            if (this.isErrorVisible != value)
            {
                this.isErrorVisible = value;
                OnPropertyChanged(nameof(GetIsErrorVisible));
            }
        }
        
        public bool ValidateCredentials(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                this.errorMessage = "Email or password cannot be empty";
                return false;
            }
            
            this.isErrorVisible = false;
            bool isValid = this.loginService.ValidateUserCredentials(email, password);
            
            if (!isValid)
            {
                this.errorMessage = "Invalid email or password";
            }
            
            return isValid;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
