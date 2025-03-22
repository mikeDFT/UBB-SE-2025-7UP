﻿using LoanShark.Helper;
using LoanShark.Service;
using LoanShark.View;
using System;
using System.Diagnostics;
using System.Windows.Input;
using LoanShark.Domain;

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
        public Action? onClose { get; set; }

        private BankAccountService service;
        private string _iban;

        /// <summary>
        /// Initializes a new instance of the BankAccountDeleteViewModel class
        /// </summary>
        public BankAccountDeleteViewModel()
        {
            service = new BankAccountService();
            _iban = UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty;
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
            WindowManager.shouldReloadBankAccounts = false;
            onClose?.Invoke();
        }

        /// <summary>
        /// Handler for the Yes button click
        /// Opens the verification view to proceed with deletion
        /// </summary>
        public void OnYesButtonClicked()
        {
            Debug.WriteLine("Yes button");
            BankAccountVerifyView window = new BankAccountVerifyView();
            window.Activate();
            WindowManager.shouldReloadBankAccounts = false;
            onClose?.Invoke();
        }
    }
}
