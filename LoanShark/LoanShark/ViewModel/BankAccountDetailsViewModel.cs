using LoanShark.Helper;
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
    class BankAccountDetailsViewModel
    {
        public ICommand ButtonCommand { get; }

        public BankAccountDetailsViewModel()
        {
            ButtonCommand = new RelayCommand(OnBackButtonClicked);
        }

        public void OnBackButtonClicked()
        {
            Debug.WriteLine("Hey");
        }
    }
}
