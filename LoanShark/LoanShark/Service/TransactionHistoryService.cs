using LoanShark.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanShark.Domain;
using System.IO;

namespace LoanShark.Service
{
    public class TransactionHistoryService
    {

        //transactions history service class needs an iban to be passed in the constructor
        //this iban is used to filter the transactions by the sender iban or receiver iban
        private string iban;
        public TransactionHistoryRepository repo;
        public TransactionHistoryService()
        {
            this.repo = new TransactionHistoryRepository();
            this.iban = UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty;
        }

        // retrieveForMenu() returns a list of transactions formatted for the menu
        public async Task<ObservableCollection<string>> retrieveForMenu()
        {
            ObservableCollection<Transaction> Transactions = await repo.getTransactionsNormal();
            ObservableCollection<String> TransactionsForMenu = new ObservableCollection<string>();

            foreach (var transaction in Transactions)
            {
                if (transaction.SenderIban == this.iban || transaction.ReceiverIban == this.iban)
                {
                    TransactionsForMenu.Add(transaction.TostringForMenu());
                }
            }
            return TransactionsForMenu;
        }

        // FilterByTypeForMenu() returns a list of transactions formatted for the menu filtered by the transaction type
        public async Task<ObservableCollection<string>> FilterByTypeForMenu(string type)
        {
            ObservableCollection<Transaction> Transactions = await repo.getTransactionsNormal();
            ObservableCollection<String> TransactionsForMenu = new ObservableCollection<string>();

            if (string.IsNullOrWhiteSpace(type)) 
            {
                foreach (var transaction in Transactions)
                {
                    TransactionsForMenu.Add(transaction.TostringForMenu());
                }
            }
            else
            {
                foreach (var transaction in Transactions)
                {
                    if (transaction.TransactionType.Contains(type, StringComparison.OrdinalIgnoreCase))
                    {
                        TransactionsForMenu.Add(transaction.TostringForMenu());
                    }
                }
            }

            return TransactionsForMenu;
        }

        // FilterByTypeDetailed() returns a list of transactions formatted in detail filtered by the transaction type
        public async Task<ObservableCollection<string>> FilterByTypeDetailed(string type)
        {
            ObservableCollection<Transaction> Transactions = await repo.getTransactionsNormal();
            ObservableCollection<String> TransactionsDetailed = new ObservableCollection<string>();

            if (string.IsNullOrWhiteSpace(type))
            {
                foreach (var transaction in Transactions)
                {
                    TransactionsDetailed.Add(transaction.TostringDetailed());
                }
            }
            else
            {
                foreach (var transaction in Transactions)
                {
                    if (transaction.TransactionType.Contains(type, StringComparison.OrdinalIgnoreCase) && (transaction.SenderIban == this.iban || transaction.ReceiverIban == this.iban))
                    {
                        TransactionsDetailed.Add(transaction.TostringDetailed());
                    }
                }
            }

            return TransactionsDetailed;
        }

        // SortByDate() returns a list of transactions formatted for the menu sorted by date
        public async Task<ObservableCollection<string>?> SortByDate(string order)
        {

            if (order == "Ascending")
            {
                ObservableCollection<Transaction> Transactions = await repo.getTransactionsNormal();
                ObservableCollection<String> TransactionsSorted = new ObservableCollection<string>();
                foreach (var transaction in Transactions.OrderBy(x => x.TransactionDatetime))
                {
                    if (transaction.SenderIban == this.iban || transaction.ReceiverIban == this.iban)
                        TransactionsSorted.Add(transaction.TostringForMenu());
                }
                return TransactionsSorted;
            }
            else if (order == "Descending")
            {
                ObservableCollection<Transaction> Transactions = await repo.getTransactionsNormal();
                ObservableCollection<String> TransactionsSorted = new ObservableCollection<string>();
                foreach (var transaction in Transactions.OrderByDescending(x => x.TransactionDatetime))
                {
                    if (transaction.SenderIban == this.iban || transaction.ReceiverIban == this.iban)
                        TransactionsSorted.Add(transaction.TostringForMenu());
                }
                return TransactionsSorted;

            }

            else
            {
                Debug.WriteLine("Invalid order");
                return null;
            }
        }

        // CreateCSV() creates a CSV file with the transactions 
        public async Task CreateCSV()
        {
            ObservableCollection<Transaction> Transactions = await repo.getTransactionsNormal();
            StringBuilder csv = new StringBuilder();
            csv.AppendLine("Transaction ID,Sender IBAN,Receiver IBAN,Transaction Date,Sender Currency,Receiver Currency,Sender Amount,Receiver Amount,Transaction Type,Transaction Description");
            foreach (var transaction in Transactions)
            {
                if (transaction.SenderIban == this.iban || transaction.ReceiverIban == this.iban)
                    csv.AppendLine(transaction.TostringCSV());
            }
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "transactions.csv");
            System.IO.File.WriteAllText(filePath, csv.ToString());
        }

        // GetTransactionByMenuString() returns a transaction object based on the menu string
        public async Task<Transaction> GetTransactionByMenuString(string menuString)
        {
            ObservableCollection<Transaction> Transactions = await repo.getTransactionsNormal();
            return Transactions.FirstOrDefault(t => t.TostringForMenu() == menuString);
        }


        // GetTransactionTypeCounts() returns a dictionary with the transaction type counts
        public async Task<Dictionary<string, int>> GetTransactionTypeCounts()
        {
            ObservableCollection<Transaction> transactions = await repo.getTransactionsNormal();
            return transactions
                .Where(t => t.SenderIban == this.iban || t.ReceiverIban == this.iban)
                .GroupBy(t => t.TransactionType)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public static async Task UpdateTransactionDescription(int transactionId, string newDescription)
        {
            await TransactionHistoryRepository.UpdateTransactionDescription(transactionId, newDescription);
        }
    }
}
