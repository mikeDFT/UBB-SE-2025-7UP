using LoanShark.Helper;
using LoanShark.Service;
using LoanShark.View;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace LoanShark.ViewModel
{
    /// <summary>
    /// ViewModel for the bank account deletion confirmation view
    /// </summary>
    class BankAccountDeleteViewModel
    {
        /// <summary>
        /// Command for the No button to cancel deletion
        /// </summary>
        public ICommand NoCommand { get; }

        /// <summary>
        /// Command for the Yes button to proceed with deletion
        /// </summary>
        public ICommand YesCommand { get; }

        /// <summary>
        /// Action to be invoked when the view should be closed
        /// </summary>
        public Action onClose { get; set; }

        private BankAccountService service;
        private string _iban;

        /// <summary>
        /// Initializes a new instance of the BankAccountDeleteViewModel class
        /// </summary>
        /// <param name="IBAN">The IBAN of the bank account to be deleted</param>
        public BankAccountDeleteViewModel(string IBAN)
        {
            service = new BankAccountService();
            _iban = IBAN;
            NoCommand = new RelayCommand(OnNoButtonClicked);
            YesCommand = new RelayCommand(OnYesButtonClicked);
        }

        /// <summary>
        /// Handler for the No button click
        /// Cancels the deletion and closes the view
        /// </summary>
        public void OnNoButtonClicked()
        {
            Debug.WriteLine("Back button");
            CloseWindowAction();
        }

        /// <summary>
        /// Handler for the Yes button click
        /// Opens the verification view to proceed with deletion
        /// </summary>
        public void OnYesButtonClicked()
        {
            Debug.WriteLine("Yes button");
            // this is hardcoded, when merging with the main branch, the email should be retrieved from the UserSession data
            // TODO ALEX
            var window = new BankAccountVerifyView(_iban, "alupeidan218@gmail.com");
            window.Activate();
        }

        /// <summary>
        /// Closes the current view
        /// </summary>
        public void CloseWindowAction()
        {
            onClose?.Invoke();
        }
    }
}
