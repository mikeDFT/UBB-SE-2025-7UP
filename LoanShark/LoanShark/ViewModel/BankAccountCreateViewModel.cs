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
using System.Collections.ObjectModel;
using LoanShark.Service;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.ComponentModel;

namespace LoanShark.ViewModel
{
    class BankAccountCreateViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Currencies { get; set; }
        public ICommand ConfirmCommand { get; }
        private string _selectedItem = "Dolar";
        public int userID;
        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                if(_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }
        private BankAccountService service;
        public BankAccountCreateViewModel(int userID)
        {
            this.userID = userID;
            service = new BankAccountService();
            Currencies = new ObservableCollection<string>();

            LoadData();

            ConfirmCommand = new RelayCommand(OnConfirmButtonClicked);
        }

        public void OnConfirmButtonClicked()
        {
            if (SelectedItem != null)
            {
                service.createBankAccount(userID, SelectedItem);
                Debug.WriteLine("Pressed create confirm bank account");
            }else
            {
                Debug.WriteLine("Bank account creation failed because no currency was selected");
            }
        }

        public void LoadData()
        {
            List<string> currencyList = service.getCurrencies();
            foreach (var c in currencyList)
            {

                Currencies.Add(c);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
