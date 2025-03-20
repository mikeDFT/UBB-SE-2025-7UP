using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using LoanShark.Service;
using LoanShark.Helper;

namespace LoanShark.ViewModel
{
    class BankAccountCreateViewModel : INotifyPropertyChanged
    {
        public Action onClose;
        public ObservableCollection<CurrencyItem> Currencies { get; set; } = new ObservableCollection<CurrencyItem>();
        public ICommand CancelCommand { get; }
        public ICommand ConfirmCommand { get; }
        public int userID;

        private CurrencyItem _selectedItem;
        public CurrencyItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    if (_selectedItem != null) _selectedItem.IsChecked = false;
                    _selectedItem = value;
                    if (_selectedItem != null) _selectedItem.IsChecked = true;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private BankAccountService service;

        public BankAccountCreateViewModel(int userID)
        {
            this.userID = userID;
            service = new BankAccountService();
            LoadData();
            ConfirmCommand = new RelayCommand(OnConfirmButtonClicked);
            CancelCommand = new RelayCommand(OnCancelButtonClicked);
        }

        public void OnConfirmButtonClicked()
        {
            if (SelectedItem != null)
            {
                service.createBankAccount(userID, SelectedItem.Name);
                Debug.WriteLine($"Pressed create confirm bank account: {SelectedItem.Name}");
            }
            else
            {
                Debug.WriteLine("Bank account creation failed because no currency was selected");
            }
        }

        public void OnCancelButtonClicked()
        {
            Debug.WriteLine("Pressed cancel create bank account");
            onClose?.Invoke();
        }

        public void LoadData()
        {
            List<string> currencyList = service.getCurrencies();
            foreach (var c in currencyList)
            {
                Currencies.Add(new CurrencyItem { Name = c });
            }

            if (Currencies.Count > 0)
            {
                SelectedItem = Currencies.FirstOrDefault(c => c.Name == "Dolar") ?? Currencies[0];
                OnPropertyChanged(nameof(SelectedItem));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class CurrencyItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}