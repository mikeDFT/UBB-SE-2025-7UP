using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using LoanShark.Helper;
using LoanShark.Service;
using LoanShark.Domain;

namespace LoanShark.ViewModel
{
    /// <summary>
    /// ViewModel for creating a new bank account
    /// </summary>
    public class BankAccountCreateViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Action to be invoked when the view should be closed
        /// </summary>
        public Action? OnClose { get; set; }

        /// <summary>
        /// Action to be invoked when the bank account creation was successful
        /// </summary>
        public Action? OnSuccess { get; set; }

        /// <summary>
        /// Collection of available currencies for the new bank account
        /// </summary>
        public ObservableCollection<CurrencyItem> Currencies { get; set; } = new ObservableCollection<CurrencyItem>();

        /// <summary>
        /// Command to cancel the bank account creation
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Command to confirm the bank account creation
        /// </summary>
        public ICommand ConfirmCommand { get; }

        /// <summary>
        /// ID of the user who will own the new bank account
        /// </summary>
        public int UserID;

        private CurrencyItem? selectedItem;

        /// <summary>
        /// The currently selected currency for the new bank account
        /// </summary>
        public CurrencyItem? SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem != value)
                {
                    if (selectedItem != null)
                    {
                        selectedItem.IsChecked = false;
                    }
                    selectedItem = value;
                    if (selectedItem != null)
                    {
                        selectedItem.IsChecked = true;
                    }
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private string? customName;
        public string? CustomName
        {
            get
            {
                return customName ?? string.Empty;
            }
            set
            {
                customName = value;
                OnPropertyChanged(nameof(CustomName));
            }
        }

        private BankAccountService service;

        /// <summary>
        /// Initializes a new instance of the BankAccountCreateViewModel class
        /// </summary>
        public BankAccountCreateViewModel()
        {
            this.UserID = int.Parse(UserSession.Instance.GetUserData("id_user") ?? "0");
            service = new BankAccountService();
            LoadData();
            ConfirmCommand = new RelayCommand(OnConfirmButtonClicked);
            CancelCommand = new RelayCommand(OnCancelButtonClicked);
        }

        /// <summary>
        /// Handler for the confirm button click
        /// Creates a new bank account with the selected currency
        /// </summary>
        public async void OnConfirmButtonClicked()
        {
            Debug.WriteLine($"Pressed create confirm bank account: {SelectedItem?.Name}");
            if (SelectedItem != null)
            {
                await service.CreateBankAccount(UserID, CustomName ?? string.Empty, SelectedItem.Name);
                WindowManager.ShouldReloadBankAccounts = true;
                OnSuccess?.Invoke();
            }
            else
            {
                Debug.WriteLine("Bank account creation failed because no currency was selected");
            }
        }

        /// <summary>
        /// Handler for the cancel button click
        /// Closes the view without creating a bank account
        /// </summary>
        public void OnCancelButtonClicked()
        {
            Debug.WriteLine("Pressed cancel create bank account");
            WindowManager.ShouldReloadBankAccounts = false;
            OnClose?.Invoke();
        }

        /// <summary>
        /// Loads currency data from the service
        /// </summary>
        public async void LoadData()
        {
            List<string> currencyList = await service.GetCurrencies();
            foreach (var c in currencyList)
            {
                Currencies.Add(new CurrencyItem { Name = c });
            }

            if (Currencies.Count > 0)
            {
                SelectedItem = Currencies.FirstOrDefault(c => c.Name == "RON") ?? Currencies[0];
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        /// <summary>
        /// Event that is raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Represents a currency item in the currency selection list
    /// </summary>
    public class CurrencyItem : INotifyPropertyChanged
    {
        /// <summary>
        /// The name of the currency
        /// </summary>
        public string Name { get; set; } = string.Empty;

        private bool isChecked;

        /// <summary>
        /// Indicates whether this currency is currently selected
        /// </summary>
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        /// <summary>
        /// Event that is raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}