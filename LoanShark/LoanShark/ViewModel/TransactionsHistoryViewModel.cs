using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LoanShark.Domain;
using LoanShark.Service;

namespace LoanShark.ViewModel
{
    // transactions View Model class needs an iban to be passed in the constructor
    // this iban is used to filter the transactions by the sender iban or receiver iban
    public class TransactionsHistoryViewModel
    {
        private TransactionHistoryService service;

        public TransactionsHistoryViewModel()
        {
            this.service = new TransactionHistoryService();
        }

        // retrieveForMenu() returns a list of transactions formatted for the menu
        public async Task<ObservableCollection<string>> RetrieveForMenu()
        {
            return await service.RetrieveForMenu();
        }

        // FilterByTypeForMenu() returns a list of transactions formatted for the menu filtered by the transaction type
        public async Task<ObservableCollection<string>> FilterByTypeForMenu(string type)
        {
            return await service.FilterByTypeForMenu(type);
        }

        // FilterByTypeDetailed() returns a list of transactions formatted in detail filtered by the transaction type
        public async Task<ObservableCollection<string>> FilterByTypeDetailed(string type)
        {
            return await service.FilterByTypeDetailed(type);
        }

        // SortByDate() returns a list of transactions formatted for the menu sorted by date
        public async Task<ObservableCollection<string>> SortByDate(string order)
        {
            return await service.SortByDate(order);
        }

        // CreateCSV() creates a CSV file with the transactions
        public void CreateCSV()
        {
            service.CreateCSV();
        }

        // GetTransactionByMenuString() returns a transaction object based on the menu string
        public async Task<Transaction> GetTransactionByMenuString(string menuString)
        {
            return await service.GetTransactionByMenuString(menuString);
        }

        // UpdateTransactionDescription() updates the transaction description
        public static async Task UpdateTransactionDescription(int transactionId, string newDescription)
        {
            await TransactionHistoryService.UpdateTransactionDescription(transactionId, newDescription);
        }

        // GetTransactionTypeCounts() returns a dictionary with the transaction type counts
        public async Task<Dictionary<string, int>> GetTransactionTypeCounts()
        {
            return await service.GetTransactionTypeCounts();
        }
    }
}
