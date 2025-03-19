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

namespace LoanShark.ViewModel
{
    class TransactionsVM
    {
        public TransactionsVM() { }

        public ObservableCollection<string> FilterByTypeForMenu(string type)
        {
            ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
            ObservableCollection<String> TransactionsForMenu = new ObservableCollection<string>();

            foreach (var transaction in Repo.getTransactionsNormal())
            {
                if (transaction.TransactionType == type)
                {
                    TransactionsForMenu.Add(transaction.tostringForMenu());
                }
            }

            return TransactionsForMenu;
        }

        public ObservableCollection<string> FilterByTypeDetailed(string type)
        {
            ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
            ObservableCollection<String> TransactionsDetailed = new ObservableCollection<string>();

            foreach (var transaction in Repo.getTransactionsNormal())
            {
                if (transaction.TransactionType == type)
                {
                    TransactionsDetailed.Add(transaction.tostringDetailed());
                }
            }
            return TransactionsDetailed;
        }

        public ObservableCollection<string> SortByDate(string order)
        {

            if (order == "ascending")
            {
                ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
                ObservableCollection<String> TransactionsSorted = new ObservableCollection<string>();
                foreach (var transaction in Repo.getTransactionsNormal().OrderBy(x => x.TransactionDate))
                {
                    TransactionsSorted.Add(transaction.tostringForMenu());
                }
                return TransactionsSorted;
            }
            else if (order == "descending")
            {
                ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
                ObservableCollection<String> TransactionsSorted = new ObservableCollection<string>();
                foreach (var transaction in Repo.getTransactionsNormal().OrderByDescending(x => x.TransactionDate))
                {
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

        public void CreateCSV()
        {
            ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
            StringBuilder csv = new StringBuilder();
            csv.AppendLine("Transaction ID,Sender IBAN,Receiver IBAN,Transaction Date,Sender Currency,Receiver Currency,Sender Amount,Receiver Amount,Transaction Type,Transaction Description");
            foreach (var transaction in Transactions)
            {
                csv.AppendLine(transaction.tostringCSV());
            }
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "transactions.csv");
            System.IO.File.WriteAllText(filePath, csv.ToString());
        }
    }
}
