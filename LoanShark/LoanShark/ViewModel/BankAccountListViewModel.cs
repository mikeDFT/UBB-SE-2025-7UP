using LoanShark.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LoanShark.Domain;
using System.ComponentModel;
using LoanShark.Service;
using LoanShark.View;

namespace LoanShark.ViewModel
{
    class BankAccountListViewModel : INotifyPropertyChanged
    {
        public Action OnClose {  get; set; }
        public ICommand MainPageCommand { get; set; }
        public ICommand SelectCommand { get; set; }
        private int userID;
        private BankAccount _selectedBankAccount;

        public BankAccount SelectedBankAccount
        {
            get { return _selectedBankAccount; }
            set
            {
                if (_selectedBankAccount != value)
                {
                    _selectedBankAccount = value;
                    OnPropertyChanged(nameof(SelectedBankAccount));
                }
            }
        }
        public ObservableCollection<BankAccount> BankAccounts { get; set; }
        private BankAccountService service;
        public BankAccountListViewModel(int userID)
        {
            this.userID = userID;
            BankAccounts = new ObservableCollection<BankAccount>();
            service = new BankAccountService(); 
            LoadData();
            MainPageCommand = new RelayCommand(ToMainPage);
            SelectCommand = new RelayCommand(ViewDetails);
        }

        public void LoadData()
        {
            foreach (var bankAccount in service.getUserBankAccounts(userID))
            {
                BankAccounts.Add(bankAccount);
            }
        }

        public void ToMainPage()
        {
            OnClose?.Invoke();
        }

        public void ViewDetails()
        {
            Debug.WriteLine(SelectedBankAccount.iban);
            if (SelectedBankAccount != null)
            {
                var window = new BankAccountDetailsView(SelectedBankAccount.iban);
                window.Activate();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
