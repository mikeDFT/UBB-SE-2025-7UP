using System;

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
    }
}
