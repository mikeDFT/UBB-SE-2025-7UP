using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanShark.Domain
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public string SenderIban { get; set; }
        public string ReceiverIban { get; set; }
        public DateTime TransactionDate { get; set; }
        public string SenderCurrency { get; set; }
        public string ReceiverCurrency { get; set; }
        public double SenderAmount { get; set; }
        public double ReceiverAmount { get; set; }
        public string TransactionType { get; set; }

        public string TransactionDescription { get;  set; }

        public Transaction(int TransactionID, string SenderIban, string ReceiverIban, DateTime TransferDate,
                            string SenderCurrency, string ReceiverCurrency, double SenderAmount,
                            double ReceiverAmount, string transactionType)
        {
            this.TransactionID = TransactionID;
            this.SenderIban = SenderIban;
            this.ReceiverIban = ReceiverIban;
            this.TransactionDate = TransferDate;
            this.SenderCurrency = SenderCurrency;
            this.ReceiverCurrency = ReceiverCurrency;
            this.SenderAmount = SenderAmount;
            this.ReceiverAmount = ReceiverAmount;
            this.TransactionType = transactionType;
        }

        public Transaction(Dictionary<string, object> hashMap)
        {
            this.TransactionID = Convert.ToInt32(hashMap["transaction_id"]);
            this.SenderIban = hashMap["sender_iban"].ToString();
            this.ReceiverIban = hashMap["receiver_iban"].ToString();
            this.TransactionDate = Convert.ToDateTime(hashMap["transaction_datetime"]);
            this.SenderCurrency = hashMap["sender_currency"].ToString();
            this.ReceiverCurrency = hashMap["receiver_currency"].ToString();
            this.SenderAmount = Convert.ToDouble(hashMap["sender_amount"]);
            this.ReceiverAmount = Convert.ToDouble(hashMap["receiver_amount"]);
            this.TransactionType = hashMap["transaction_type"].ToString();
            this.TransactionDescription = hashMap["transaction_description"].ToString();
        }

        public string tostringDetailed()
        {
            return "Transaction ID: " + TransactionID + "\n" +
                   "Sender IBAN: " + SenderIban + "\n" +
                   "Receiver IBAN: " + ReceiverIban + "\n" +
                   "Transfer Date: " + TransactionDate + "\n" +
                   "Sender Currency: " + SenderCurrency + "\n" +
                   "Receiver Currency: " + ReceiverCurrency + "\n" +
                   "Sender Amount: " + SenderAmount + "\n" +
                   "Receiver Amount: " + ReceiverAmount + "\n" +
                   "Transaction Type: " + TransactionType + "\n" +
                   "Transaction Description: " + TransactionDescription + "\n";
        }

        public string tostringForMenu()
        {
            return "Sender IBAN: " + SenderIban + "\n" +
                   "Receiver IBAN: " + ReceiverIban + "\n\n" + 
                   "Sent Amount: " + SenderAmount + " " + SenderCurrency + "\n" + 
                   "Received Amount: " + ReceiverAmount + " " + ReceiverCurrency + "\n\n" +
                   "Date: " + TransactionDate + "\n\n" +
                   "Type: " + TransactionType;
        }

        public string tostringCSV()
        {
            return $"{TransactionID},{SenderIban},{ReceiverIban},{TransactionDate},{SenderCurrency},{ReceiverCurrency},{SenderAmount},{ReceiverAmount},{TransactionType},{TransactionDescription}";
        }

    }

}
