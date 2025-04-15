using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using LoanShark.Domain;
using LoanShark.Service;
using LoanShark.View;

namespace LoanShark.ViewModel
{
    /// <summary>
    /// ViewModel for displaying and managing a list of bank accounts
    /// </summary>
    public class BankAccountListViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Action to be invoked when the view should be closed
        /// </summary>
        public Action? OnClose { get; set; }

        public Action? OnSuccess { get; set; }

        /// <summary>
        /// Command to navigate back to the main page
        /// </summary>
        public ICommand MainPageCommand { get; set; }

        /// <summary>
        /// Command to select a bank account and view its details
        /// </summary>
        public ICommand SelectCommand { get; set; }

        private int userID;
        private BankAccount? selectedBankAccount;

        /// <summary>
        /// The currently selected bank account
        /// </summary>
        public BankAccount SelectedBankAccount
        {
            get => selectedBankAccount;
            set
            {
                if (selectedBankAccount != value)
                {
                    selectedBankAccount = value;
                    OnPropertyChanged(nameof(SelectedBankAccount));
                }
            }
        }

        /// <summary>
        /// Collection of bank accounts belonging to the user
        /// </summary>
        public ObservableCollection<BankAccount> BankAccounts { get; set; }

        private BankAccountService service;

        /// <summary>
        /// Initializes a new instance of the BankAccountListViewModel class
        /// </summary>
        /// <param name="userID">The ID of the user whose bank accounts to display</param>
        public BankAccountListViewModel()
        {
            this.userID = int.Parse(UserSession.Instance.GetUserData("id_user") ?? "0");
            BankAccounts = new ObservableCollection<BankAccount>();
            service = new BankAccountService();
            _ = LoadData(); // Start loading data but don't await it
            MainPageCommand = new RelayCommand(ToMainPage);
            SelectCommand = new RelayCommand(ViewDetails);
        }

        /// <summary>
        /// Loads the user's bank accounts from the service
        /// </summary>
        public async Task LoadData()
        {
            foreach (var bankAccount in await service.GetUserBankAccounts(userID))
            {
                BankAccounts.Add(bankAccount);
            }
        }

        /// <summary>
        /// Navigates back to the main page
        /// </summary>
        public void ToMainPage()
        {
            OnClose?.Invoke();
        }

        /// <summary>
        /// Opens the details view for the selected bank account
        /// </summary>
        public void ViewDetails()
        {
            Debug.WriteLine(SelectedBankAccount.Iban);
            if (SelectedBankAccount != null)
            {
                var window = new BankAccountDetailsView();
                window.Activate();
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
