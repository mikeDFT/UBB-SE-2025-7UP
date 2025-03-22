using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.Repository;
using LoanShark.Service;

namespace LoanShark.ViewModel
{

    //transactions View Model class needs an iban to be passed in the constructor
    //this iban is used to filter the transactions by the sender iban or receiver iban
    public class TransactionsHistoryViewModel
    {
        private TransactionHistoryService service;

        public TransactionsHistoryViewModel(TransactionHistoryService service) {

            this.service = service;
        }

        // retrieveForMenu() returns a list of transactions formatted for the menu
        public ObservableCollection<string> retrieveForMenu()
        {
            return service.retrieveForMenu();
        }

        // FilterByTypeForMenu() returns a list of transactions formatted for the menu filtered by the transaction type
        public ObservableCollection<string> FilterByTypeForMenu(string type)
        {
            return service.FilterByTypeForMenu(type);
        }

        // FilterByTypeDetailed() returns a list of transactions formatted in detail filtered by the transaction type
        public ObservableCollection<string> FilterByTypeDetailed(string type)
        {
            return service.FilterByTypeDetailed(type);
        }

        // SortByDate() returns a list of transactions formatted for the menu sorted by date
        public ObservableCollection<string> SortByDate(string order)
        {
            return service.SortByDate(order);
        }

        // CreateCSV() creates a CSV file with the transactions 
        public void CreateCSV()
        {
            service.CreateCSV();
        }

        // GetTransactionByMenuString() returns a transaction object based on the menu string
        public Transaction GetTransactionByMenuString(string menuString)
        {
            return service.GetTransactionByMenuString(menuString);
        }

        // UpdateTransactionDescription() updates the transaction description
        public static void UpdateTransactionDescription(int transactionId, string newDescription)
        {
            TransactionHistoryRepository.UpdateTransactionDescription(transactionId, newDescription);
        }


        // GetTransactionTypeCounts() returns a dictionary with the transaction type counts
        public Dictionary<string, int> GetTransactionTypeCounts()
        {
            return service.GetTransactionTypeCounts();
        }
    }
}
