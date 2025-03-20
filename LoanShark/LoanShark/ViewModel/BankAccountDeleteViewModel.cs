using LoanShark.Helper;
using LoanShark.Service;
using LoanShark.View;
using Microsoft.UI.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoanShark.ViewModel
{
    class BankAccountDeleteViewModel
    {
        public ICommand NoCommand { get; }
        public ICommand YesCommand { get; }
        public Action onClose { get; set; }
        private BankAccountService service;
        private string _iban;
        public BankAccountDeleteViewModel(string IBAN)
        {
            service = new BankAccountService();
            _iban = IBAN;
            NoCommand = new RelayCommand(OnNoButtonClicked);
            YesCommand = new RelayCommand(OnYesButtonClicked);
        }

        public void OnNoButtonClicked()
        {
            Debug.WriteLine("Back button");
            CloseWindowAction();
        }

        public void OnYesButtonClicked()
        {
            Debug.WriteLine("Yes button");
            var window = new BankAccountVerifyView(_iban);
            window.Activate();
        }

        public void CloseWindowAction()
        {
            onClose?.Invoke();
        }
    }
}
