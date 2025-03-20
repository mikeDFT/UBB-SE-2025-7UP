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
    public class TransactionsVM
    {
        string iban;
        public TransactionsVM(string iban) {

            this.iban = iban;
        }

        public ObservableCollection<string> retrieveForMenu()
        {
            ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
            ObservableCollection<String> TransactionsForMenu = new ObservableCollection<string>();

            foreach (var transaction in Transactions)
            {
                if (transaction.SenderIban == this.iban)
                {
                    TransactionsForMenu.Add(transaction.tostringForMenu());
                }
            }
            return TransactionsForMenu;
        }

        public ObservableCollection<string> FilterByTypeForMenu(string type)
        {
            ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
            ObservableCollection<String> TransactionsForMenu = new ObservableCollection<string>();

            foreach (var transaction in Repo.getTransactionsNormal())
            {
                if (transaction.TransactionType == type && transaction.SenderIban == this.iban)
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
                if (transaction.TransactionType == type && transaction.SenderIban == this.iban)
                {
                    TransactionsDetailed.Add(transaction.tostringDetailed());
                }
            }
            return TransactionsDetailed;
        }

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

        public Transaction GetTransactionByMenuString(string menuString)
        {
            ObservableCollection<Transaction> Transactions = Repo.getTransactionsNormal();
            return Transactions.FirstOrDefault(t => t.tostringForMenu() == menuString);
        }
    }
}
