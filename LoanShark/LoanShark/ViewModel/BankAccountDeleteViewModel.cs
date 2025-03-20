using LoanShark.Helper;
using LoanShark.View;
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
        public BankAccountDeleteViewModel()
        {
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
            var window = new BankAccountVerifyView();
            window.Activate();
        }

        public void CloseWindowAction()
        {
            onClose?.Invoke();
        }
    }
}
