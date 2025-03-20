using LoanShark.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LoanShark.ViewModel
{
    class BankAccountVerifyViewModel
    {
        public ICommand BackCommand { get; }
        public ICommand ConfirmCommand { get; }

        public BankAccountVerifyViewModel()
        {
            BackCommand = new RelayCommand(OnBackButtonClicked);
            ConfirmCommand = new RelayCommand(OnConfirmButtonClicked);
        }

        public void OnBackButtonClicked()
        {
            Debug.WriteLine("Back button");
        }

        public void OnConfirmButtonClicked()
        {
            Debug.WriteLine("Confirm button");
        }
    }
}
