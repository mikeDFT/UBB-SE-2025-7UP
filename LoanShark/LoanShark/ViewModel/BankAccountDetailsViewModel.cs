using LoanShark.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LoanShark.Domain;
using LoanShark.Service;

namespace LoanShark.ViewModel
{
    class BankAccountDetailsViewModel : INotifyPropertyChanged
    {
        public ICommand ButtonCommand { get; }
        public Action OnClose { get; set; }
        public BankAccount BankAccount { get; set; }
        private BankAccountService service;

        public BankAccountDetailsViewModel(string IBAN)
        {
            service = new BankAccountService();
            BankAccount = service.findBankAccount(IBAN);
            ButtonCommand = new RelayCommand(OnBackButtonClicked);
        }

        public void OnBackButtonClicked()
        {
            Debug.WriteLine("Back button clicked in bank account details page");
            OnClose?.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
