using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using LoanShark.Helper;
using LoanShark.Service;
using LoanShark.Domain;

namespace LoanShark.ViewModel
{
    /// <summary>
    /// ViewModel for verifying user credentials before deleting a bank account
    /// </summary>
    public class BankAccountVerifyViewModel : INotifyPropertyChanged
    {
        public Action OnSuccess { get; set; }
        public Action OnFailure { get; set; }

        /// <summary>
        /// Command for the back button to return to the previous view
        /// </summary>
        public ICommand BackCommand { get; }

        /// <summary>
        /// Command for the confirm button to verify credentials and delete the account
        /// </summary>
        public ICommand ConfirmCommand { get; }

        /// <summary>
        /// Action to be invoked when the view should be closed
        /// </summary>
        public Action? OnClose { get; set; }

        private BankAccountService service;
        private string iban;
        private string? passwordInput;
        private string email;

        /// <summary>
        /// The password entered by the user for verification
        /// </summary>
        public string Password
        {
            get
            {
                return passwordInput ?? string.Empty;
            }
            set
            {
                passwordInput = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        /// <summary>
        /// Initializes a new instance of the BankAccountVerifyViewModel class
        /// </summary>
        public BankAccountVerifyViewModel()
        {
            service = new BankAccountService();
            email = UserSession.Instance.GetUserData("email") ?? string.Empty;
            iban = UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty;
            BackCommand = new RelayCommand(OnBackButtonClicked);
            ConfirmCommand = new RelayCommand(OnConfirmButtonClicked);
        }

        /// <summary>
        /// Handler for the back button click
        /// Closes the current view and returns to the previous view
        /// </summary>
        public void OnBackButtonClicked()
        {
            Debug.WriteLine("Back button");
            WindowManager.ShouldReloadBankAccounts = false;
            OnClose?.Invoke();
        }

        /// <summary>
        /// Handler for the confirm button click
        /// Verifies the user credentials and removes the bank account if credentials are valid
        /// </summary>
        public async void OnConfirmButtonClicked()
        {
            Debug.WriteLine("Confirm button");
            if (Password != null && await service.VerifyUserCredentials(email, Password))
            {
                if (await service.RemoveBankAccount(iban))
                {
                    WindowManager.ShouldReloadBankAccounts = true;
                    OnSuccess?.Invoke();
                }
            }
            else
            {
                OnFailure?.Invoke();
            }
        }

        /// <summary>
        /// Event that is raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
