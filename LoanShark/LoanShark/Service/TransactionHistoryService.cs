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
        string iban;
        public TransactionHistoryService(string iban)
        {

            this.iban = iban;
        }

        // retrieveForMenu() returns a list of transactions formatted for the menu
        public ObservableCollection<string> retrieveForMenu()
        {
            ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
            ObservableCollection<String> TransactionsForMenu = new ObservableCollection<string>();

            foreach (var transaction in Transactions)
            {
                if (transaction.SenderIban == this.iban || transaction.ReceiverIban == this.iban)
                {
                    TransactionsForMenu.Add(transaction.tostringForMenu());
                }
            }
            return TransactionsForMenu;
        }

        // FilterByTypeForMenu() returns a list of transactions formatted for the menu filtered by the transaction type
        public ObservableCollection<string> FilterByTypeForMenu(string type)
        {
            ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
            ObservableCollection<String> TransactionsForMenu = new ObservableCollection<string>();

            foreach (var transaction in Repo.getTransactionsNormal())
            {
                if (transaction.TransactionType == type && (transaction.SenderIban == this.iban || transaction.ReceiverIban == this.iban))
                {
                    TransactionsForMenu.Add(transaction.tostringForMenu());
                }
            }

            return TransactionsForMenu;
        }

        // FilterByTypeDetailed() returns a list of transactions formatted in detail filtered by the transaction type
        public ObservableCollection<string> FilterByTypeDetailed(string type)
        {
            ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
            ObservableCollection<String> TransactionsDetailed = new ObservableCollection<string>();

            foreach (var transaction in Transactions)
            {
                if (transaction.TransactionType == type && (transaction.SenderIban == this.iban || transaction.ReceiverIban == this.iban))
                {
                    TransactionsDetailed.Add(transaction.tostringDetailed());
                }
            }
            return TransactionsDetailed;
        }

        // SortByDate() returns a list of transactions formatted for the menu sorted by date
        public ObservableCollection<string> SortByDate(string order)
        {

            if (order == "Ascending")
            {
                ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
                ObservableCollection<String> TransactionsSorted = new ObservableCollection<string>();
                foreach (var transaction in Repo.getTransactionsNormal().OrderBy(x => x.TransactionDate))
                {
                    if (transaction.SenderIban == this.iban)
                        TransactionsSorted.Add(transaction.tostringForMenu());
                }
                return TransactionsSorted;
            }
            else if (order == "Descending")
            {
                ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
                ObservableCollection<String> TransactionsSorted = new ObservableCollection<string>();
                foreach (var transaction in Repo.getTransactionsNormal().OrderByDescending(x => x.TransactionDate))
                {
                    if (transaction.SenderIban == this.iban)
                        TransactionsSorted.Add(transaction.tostringForMenu());
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
        public void CreateCSV()
        {
            ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
            StringBuilder csv = new StringBuilder();
            csv.AppendLine("Transaction ID,Sender IBAN,Receiver IBAN,Transaction Date,Sender Currency,Receiver Currency,Sender Amount,Receiver Amount,Transaction Type,Transaction Description");
            foreach (var transaction in Transactions)
            {
                if (transaction.SenderIban == this.iban)
                    csv.AppendLine(transaction.tostringCSV());
            }
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "transactions.csv");
            System.IO.File.WriteAllText(filePath, csv.ToString());
        }

        // GetTransactionByMenuString() returns a transaction object based on the menu string
        public Transaction GetTransactionByMenuString(string menuString)
        {
            ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
            return Transactions.FirstOrDefault(t => t.tostringForMenu() == menuString);
        }


            // GetTransactionTypeCounts() returns a dictionary with the transaction type counts
            public Dictionary<string, int> GetTransactionTypeCounts()
            {
                ObservableCollection<Transaction> transactions = Repo.getTransactionsNormal();
                return transactions
                    .Where(t => t.SenderIban == this.iban)
                    .GroupBy(t => t.TransactionType)
                    .ToDictionary(g => g.Key, g => g.Count());
            }
        }
}
