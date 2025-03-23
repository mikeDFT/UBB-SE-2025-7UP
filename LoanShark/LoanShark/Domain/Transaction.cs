using System;
using System.Collections.Generic;

namespace LoanShark.Domain
{
    public class Transaction
    {
        public int TransactionId { get; set; } 
        public string SenderIban { get; set; } 
        public string ReceiverIban { get; set; }
        public DateTime TransactionDatetime { get; set; } 
        public string SenderCurrency { get; set; } 
        public string ReceiverCurrency { get; set; }
        public decimal SenderAmount { get; set; }  
        public decimal ReceiverAmount { get; set; } 
        public string TransactionType { get; set; } 
        public string TransactionDescription { get; set; }

        public Transaction() { }

        public Transaction(int transactionId, string senderIban, string receiverIban, DateTime transactionDatetime,
                           string senderCurrency, string receiverCurrency, decimal senderAmount, decimal receiverAmount,
                           string transactionType, string transactionDescription)
        {
            TransactionId = transactionId;
            SenderIban = senderIban;
            ReceiverIban = receiverIban;
            TransactionDatetime = transactionDatetime;
            SenderCurrency = senderCurrency;
            ReceiverCurrency = receiverCurrency;
            SenderAmount = senderAmount;
            ReceiverAmount = receiverAmount;
            TransactionType = transactionType;
            TransactionDescription = transactionDescription;
        }

        public override string ToString()
        {
            return $"Transaction ID: {TransactionId} | From: {SenderIban} | To: {ReceiverIban} | Amount: {SenderAmount} {SenderCurrency} → {ReceiverAmount} {ReceiverCurrency} | Type: {TransactionType} | Date: {TransactionDatetime}";
        }

        //tostringDetailed() and tostringForMenu() format the transaction to be displayed in the UI
        public string tostringDetailed()
        {
            return "Transaction ID: " + TransactionId + "\n" +
                   "Sender IBAN: " + SenderIban + "\n" +
                   "Receiver IBAN: " + ReceiverIban + "\n" +
                   "Transfer Date: " + TransactionDatetime + "\n" +
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
                   "Date: " + TransactionDatetime + "\n\n" +
                   "Type: " + TransactionType;
        }


        // this formats the transaction to be written to a CSV file
        public string tostringCSV()
        {
            return $"{TransactionId},{SenderIban},{ReceiverIban},{TransactionDatetime},{SenderCurrency},{ReceiverCurrency},{SenderAmount},{ReceiverAmount},{TransactionType},{TransactionDescription}";
        }

        // Constructor that accepts a Dictionary mapping column names to values
        public Transaction(Dictionary<string, object> data)
        {
            TransactionId = Convert.ToInt32(data["TransactionID"]);
            SenderIban = data["SenderIBAN"].ToString() ?? string.Empty;
            ReceiverIban = data["ReceiverIBAN"].ToString() ?? string.Empty;
            TransactionDatetime = Convert.ToDateTime(data["TransactionDatetime"]);
            SenderCurrency = data["SenderCurrency"].ToString() ?? string.Empty;
            ReceiverCurrency = data["ReceiverCurrency"].ToString() ?? string.Empty;
            SenderAmount = Convert.ToDecimal(data["SenderAmount"]);
            ReceiverAmount = Convert.ToDecimal(data["ReceiverAmount"]);
            TransactionType = data["TransactionType"].ToString() ?? string.Empty;
            TransactionDescription = data["TransactionDescription"].ToString() ?? string.Empty;
        }
    }
}
