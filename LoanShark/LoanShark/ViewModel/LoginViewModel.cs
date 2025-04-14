using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
            get => this.email;
            set
            {
                if (this.email != value)
                {
                    this.email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get => this.errorMessage;
            set
            {
                if (this.errorMessage != value)
                {
                    this.errorMessage = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public bool IsErrorVisible
        {
            get => this.isErrorVisible;
            set
            {
                if (this.isErrorVisible != value)
                {
                    this.isErrorVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task<bool> ValidateCredentials(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                this.ErrorMessage = "Email or password cannot be empty";
                this.IsErrorVisible = true;
                return false;
            }

            this.IsErrorVisible = false;
            bool isValid = await this.loginService.ValidateUserCredentials(email, password);

            if (!isValid)
            {
                this.ErrorMessage = "Invalid email or password";
                this.IsErrorVisible = true;
            }
            else
            {
                this.ErrorMessage = string.Empty;
                this.IsErrorVisible = false;
            }

            return isValid;
        }

        public async Task InstantiateUserSessionAfterLogin(string email)
        {
            await this.loginService.InstantiateUserSessionAfterLogin(email);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
