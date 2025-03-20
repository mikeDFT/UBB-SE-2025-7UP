using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.WebUI;
using Microsoft.UI.Xaml;
using LoanShark.Helper;
using System.Diagnostics;

namespace LoanShark.ViewModel
{
    class BankAccountCreateViewModel
    {
        public ICommand ButtonCommand { get; }

        public BankAccountCreateViewModel()
        {
            ButtonCommand = new RelayCommand(OnBackButtonClicked);
        }

        public void OnBackButtonClicked()
        {
            Debug.WriteLine("Hey");
        }
    }
}
