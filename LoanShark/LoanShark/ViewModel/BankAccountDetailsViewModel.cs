using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.Helper;
using LoanShark.Service;

namespace LoanShark.ViewModel
{
    /// <summary>
    /// ViewModel for displaying bank account details
    /// </summary>
    public class BankAccountDetailsViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Command for the back button to return to the previous view
        /// </summary>
        public ICommand ButtonCommand { get; }

        /// <summary>
        /// Action to be invoked when the view should be closed
        /// </summary>
        public Action? OnClose { get; set; }

        /// <summary>
        /// The bank account whose details are being displayed
        /// </summary>
        private BankAccount? bankAccount;
        public BankAccount? BankAccount
        {
            get => bankAccount;
            set
            {
                bankAccount = value;
                OnPropertyChanged(nameof(BankAccount));
            }
        }

        public string BankAccountStatus
        {
            get
            {
                if (BankAccount?.Blocked == true)
                {
                    return "Blocked";
                }
                else
                {
                    return "Active";
                }
            }
        }

        private BankAccountService service;

        /// <summary>
        /// Initializes a new instance of the BankAccountDetailsViewModel class
        /// </summary>
        /// <param name="IBAN">The IBAN of the bank account to display</param>
        public BankAccountDetailsViewModel()
        {
            service = new BankAccountService();
            ButtonCommand = new RelayCommand(OnBackButtonClicked);
            // Start loading data but don't await it
            _ = LoadBankAccountAsync();
        }

        private async Task LoadBankAccountAsync()
        {
            BankAccount = await service.FindBankAccount(
                UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty);
        }

        /// <summary>
        /// Handler for the back button click
        /// Closes the current view and returns to the previous view
        /// </summary>
        public void OnBackButtonClicked()
        {
            Debug.WriteLine("Back button clicked in bank account details page");
            WindowManager.ShouldReloadBankAccounts = false;
            OnClose?.Invoke();
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
