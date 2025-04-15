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

namespace LoanShark.Service
{
    public class TransactionHistoryService
    {
        // transactions history service class needs an iban to be passed in the constructor
        // this iban is used to filter the transactions by the sender iban or receiver iban
        private string iban;
        public TransactionHistoryRepository Repo;
        public TransactionHistoryService()
        {
            this.Repo = new TransactionHistoryRepository();
            this.iban = UserSession.Instance.GetUserData("current_bank_account_iban") ?? string.Empty;
        }

        // retrieveForMenu() returns a list of transactions formatted for the menu
        public async Task<ObservableCollection<string>> RetrieveForMenu()
        {
            ObservableCollection<Transaction> transactions = await Repo.GetTransactionsNormal();
            ObservableCollection<string> transactionsForMenu = new ObservableCollection<string>();

            foreach (var transaction in transactions)
            {
                if (transaction.SenderIban == this.iban || transaction.ReceiverIban == this.iban)
                {
                    transactionsForMenu.Add(transaction.TostringForMenu());
                }
            }
            return transactionsForMenu;
        }

        // FilterByTypeForMenu() returns a list of transactions formatted for the menu filtered by the transaction type
        public async Task<ObservableCollection<string>> FilterByTypeForMenu(string type)
        {
            ObservableCollection<Transaction> transactions = await Repo.GetTransactionsNormal();
            ObservableCollection<string> transactionsForMenu = new ObservableCollection<string>();

            if (string.IsNullOrWhiteSpace(type))
            {
                foreach (var transaction in transactions)
                {
                    transactionsForMenu.Add(transaction.TostringForMenu());
                }
            }
            else
            {
                foreach (var transaction in transactions)
                {
                    if (transaction.TransactionType.Contains(type, StringComparison.OrdinalIgnoreCase))
                    {
                        transactionsForMenu.Add(transaction.TostringForMenu());
                    }
                }
            }

            return transactionsForMenu;
        }

        // FilterByTypeDetailed() returns a list of transactions formatted in detail filtered by the transaction type
        public async Task<ObservableCollection<string>> FilterByTypeDetailed(string type)
        {
            ObservableCollection<Transaction> transactions = await Repo.GetTransactionsNormal();
            ObservableCollection<string> transactionsDetailed = new ObservableCollection<string>();

            if (string.IsNullOrWhiteSpace(type))
            {
                foreach (var transaction in transactions)
                {
                    transactionsDetailed.Add(transaction.TostringDetailed());
                }
            }
            else
            {
                foreach (var transaction in transactions)
                {
                    if (transaction.TransactionType.Contains(type, StringComparison.OrdinalIgnoreCase) && (transaction.SenderIban == this.iban || transaction.ReceiverIban == this.iban))
                    {
                        transactionsDetailed.Add(transaction.TostringDetailed());
                    }
                }
            }

            return transactionsDetailed;
        }

        // SortByDate() returns a list of transactions formatted for the menu sorted by date
        public async Task<ObservableCollection<string>?> SortByDate(string order)
        {
            if (order == "Ascending")
            {
                ObservableCollection<Transaction> transactions = await Repo.GetTransactionsNormal();
                ObservableCollection<string> transactionsSorted = new ObservableCollection<string>();
                foreach (var transaction in transactions.OrderBy(x => x.TransactionDatetime))
                {
                    if (transaction.SenderIban == this.iban || transaction.ReceiverIban == this.iban)
                    {
                        transactionsSorted.Add(transaction.TostringForMenu());
                    }
                }
                return transactionsSorted;
            }
            else if (order == "Descending")
            {
                ObservableCollection<Transaction> transactions = await Repo.GetTransactionsNormal();
                ObservableCollection<string> transactionsSorted = new ObservableCollection<string>();
                foreach (var transaction in transactions.OrderByDescending(x => x.TransactionDatetime))
                {
                    if (transaction.SenderIban == this.iban || transaction.ReceiverIban == this.iban)
                    {
                        transactionsSorted.Add(transaction.TostringForMenu());
                    }
                }
                return transactionsSorted;
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
            ObservableCollection<Transaction> transactions = await Repo.GetTransactionsNormal();
            StringBuilder csv = new StringBuilder();
            csv.AppendLine("Transaction ID,Sender IBAN,Receiver IBAN,Transaction Date,Sender Currency,Receiver Currency,Sender Amount,Receiver Amount,Transaction Type,Transaction Description");
            foreach (var transaction in transactions)
            {
                if (transaction.SenderIban == this.iban || transaction.ReceiverIban == this.iban)
                {
                    csv.AppendLine(transaction.TostringCSV());
                }
            }
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "transactions.csv");
            System.IO.File.WriteAllText(filePath, csv.ToString());
        }

        // GetTransactionByMenuString() returns a transaction object based on the menu string
        public async Task<Transaction> GetTransactionByMenuString(string menuString)
        {
            ObservableCollection<Transaction> transactions = await Repo.GetTransactionsNormal();
            return transactions.FirstOrDefault(t => t.TostringForMenu() == menuString);
        }

        // GetTransactionTypeCounts() returns a dictionary with the transaction type counts
        public async Task<Dictionary<string, int>> GetTransactionTypeCounts()
        {
            ObservableCollection<Transaction> transactions = await Repo.GetTransactionsNormal();
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
