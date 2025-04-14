using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.Service;
using LoanShark.View;

namespace LoanShark.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string? welcomeText;
        private ObservableCollection<BankAccount> userBankAccounts;
        private string balanceButtonContent;
        private readonly MainPageService service;
        public event PropertyChangedEventHandler? PropertyChanged;

        public MainPageViewModel()
        {
            this.service = new MainPageService();
            userBankAccounts = new ObservableCollection<BankAccount>();
            this.balanceButtonContent = "Check Balance";
            InitializeWelcomeText();
            LoadUserBankAccounts();
        }

        public string WelcomeText
        {
            get => this.welcomeText ?? "Welcome, user";
            set
            {
                if (this.welcomeText != value)
                {
                    this.welcomeText = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<BankAccount> UserBankAccounts
        {
            get => userBankAccounts;
            set
            {
                if (userBankAccounts != value)
                {
                    userBankAccounts = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BalanceButtonContent
        {
            get => this.balanceButtonContent;
            set
            {
                if (this.balanceButtonContent != value)
                {
                    this.balanceButtonContent = value;
                    OnPropertyChanged();
                }
            }
        }

        public void InitializeWelcomeText()
        {
            try
            {
                string? firstName = UserSession.Instance.GetUserData("first_name");
                this.WelcomeText = firstName != null ? $"Welcome back, {firstName}" : "Welcome, user";
            }
            catch (Exception ex)
            {
                this.WelcomeText = "Welcome, user";
                Debug.Print($"Error getting user data: {ex.Message}");
            }
        }

        public async Task LoadUserBankAccounts()
        {
            try
            {
                string? userId = UserSession.Instance.GetUserData("id_user");
                if (string.IsNullOrEmpty(userId))
                {
                    Debug.Print("User ID is null or empty");
                    return;
                }

                int idUser = int.Parse(userId);

                try
                {
                    UserBankAccounts = await this.service.GetUserBankAccounts(idUser);

                    if (UserBankAccounts.Count == 0)
                    {
                        // Add a placeholder or default message if no accounts found
                        UserBankAccounts.Add(new BankAccountMessage("No accounts found", "Please add a bank account"));
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print($"Error loading bank accounts from service: {ex.Message}");
                    // Add a placeholder or error message
                    userBankAccounts.Clear();
                    userBankAccounts.Add(new BankAccountMessage("Error", "Could not load bank accounts"));
                    OnPropertyChanged(nameof(UserBankAccounts));
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"Error in LoadUserBankAccounts: {ex.Message}");
                // Add a placeholder or error message
                userBankAccounts.Clear();
                userBankAccounts.Add(new BankAccountMessage("Error", "Could not load bank accounts"));
                OnPropertyChanged(nameof(UserBankAccounts));
            }
        }

        public void AccountSettingsButtonHandler()
        {
            UserInformationView userInformationView = new UserInformationView();
            userInformationView.Activate();
        }

        public async Task CheckBalanceButtonHandler()
        {
            try
            {
                if (this.BalanceButtonContent == "Check Balance")
                {
                    string? currentBankAccountIban = UserSession.Instance.GetUserData("current_bank_account_iban");
                    if (string.IsNullOrEmpty(currentBankAccountIban))
                    {
                        Debug.Print("Current bank account IBAN is null or empty");
                        return;
                    }
                    if (currentBankAccountIban == "No accounts found" || currentBankAccountIban == "Error")
                    {
                        // if the current bank account is not found, we will show the message "Check Balance" as if the button was not pressed
                        Debug.Print("There are no accounts for the current user");
                        this.BalanceButtonContent = "Check Balance";
                    }
                    else
                    {
                        Tuple<decimal, string> result = await this.service.GetBankAccountBalanceByUserIban(currentBankAccountIban);
                        decimal balance = result.Item1;
                        string currency = result.Item2;
                        string balanceString = balance.ToString("0.00");
                        this.BalanceButtonContent = $"{balanceString} {currency}";
                        Debug.Print($"Balance: {balanceString}");
                    }
                    this.OnPropertyChanged(nameof(BalanceButtonContent));
                }
                else
                {
                    this.BalanceButtonContent = "Check Balance";
                    this.OnPropertyChanged(nameof(BalanceButtonContent));
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"Error in CheckBalanceButtonHandler: {ex.Message}");
            }
        }

        public async Task<string?> TransactionButtonHandler()
        {
            if (UserBankAccounts.Count == 1 && UserBankAccounts[0] is BankAccountMessage)
            {
                return "Please create a bank account to initiate any type of transaction";
            }
            TransactionsView transactionView = new TransactionsView();
            transactionView.Activate();
            return null;
        }

        public async Task<string?> TransactionHistoryButtonHandler()
        {
            if (UserBankAccounts.Count == 1 && UserBankAccounts[0] is BankAccountMessage)
            {
                return "Transaction history is not available, please create a bank account first";
            }

            TransactionHistoryView transactionHistoryView = new TransactionHistoryView();
            transactionHistoryView.Activate();
            return null;
        }

        public async Task<string?> BankAccountDetailsButtonHandler()
        {
            if (UserBankAccounts.Count == 1 && UserBankAccounts[0] is BankAccountMessage)
            {
                return "Please create a bank account to view details";
            }
            BankAccountDetailsView bankAccountDetailsView = new BankAccountDetailsView();
            bankAccountDetailsView.Activate();
            return null;
        }

        public async Task<string?> BankAccountSettingsButtonHandler()
        {
            if (UserBankAccounts.Count == 1 && UserBankAccounts[0] is BankAccountMessage)
            {
                return "Please create a bank account to update settings";
            }
            BankAccountUpdateView bankAccountUpdateView = new BankAccountUpdateView();
            bankAccountUpdateView.Activate();
            return null;
        }

        public async Task<string?> LoanButtonHandler()
        {
            if (UserBankAccounts.Count == 1 && UserBankAccounts[0] is BankAccountMessage)
            {
                return "Please create a bank account to initiate a loan";
            }
            LoanView loanView = new LoanView();
            loanView.Activate();
            return null;
        }

        public void BankAccountCreateButtonHandler()
        {
            BankAccountCreateView bankAccountCreateView = new BankAccountCreateView();
            bankAccountCreateView.Activate();
        }

        public void ResetBalanceButtonContent()
        {
            if (this.BalanceButtonContent != "Check Balance")
            {
                this.BalanceButtonContent = "Check Balance";
                this.OnPropertyChanged(nameof(BalanceButtonContent));
            }
        }

        public async Task RefreshBankAccounts()
        {
            await LoadUserBankAccounts();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
