using LoanShark.Helper;
using LoanShark.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoanShark.ViewModel
{
    class BankAccountVerifyViewModel : INotifyPropertyChanged
    {
        public ICommand BackCommand { get; }
        public ICommand ConfirmCommand { get; }
        public Action OnClose { get; set; }
        private BankAccountService service;
        private string _iban;
        private string _emailInput;
        private string _passwordInput;

        public string Password
        {
            get
            {
                return _passwordInput;
            }
            set
            {
                _passwordInput = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Email
        {
            get
            {
                return _emailInput;
            }
            set
            {
                _emailInput = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public BankAccountVerifyViewModel(string IBAN)
        {
            service = new BankAccountService();
            _iban = IBAN;
            BackCommand = new RelayCommand(OnBackButtonClicked);
            ConfirmCommand = new RelayCommand(OnConfirmButtonClicked);
        }

        public void OnBackButtonClicked()
        {
            Debug.WriteLine("Back button");
            CloseWindowAction();
        }

        public void OnConfirmButtonClicked()
        {
            Debug.WriteLine("Confirm button");
            if (service.verifyUserCredentials(Email, Password))
            {
                service.removeBankAccount(_iban);
                Debug.WriteLine("User entered correct credentials");
            }
            else
                Debug.WriteLine("User entered wrong credentials");
        }

        public void CloseWindowAction()
        {
            OnClose?.Invoke();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));   
    }
}
