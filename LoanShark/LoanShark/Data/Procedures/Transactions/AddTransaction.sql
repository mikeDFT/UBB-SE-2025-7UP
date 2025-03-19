create or alter procedure AddTransaction
    @senderIban nvarchar(100),
    @receiverIban nvarchar(100),
    @senderCurrency nvarchar(5),
    @receiverCurrency nvarchar(5),
    @senderAmount float,
    @receiverAmount float,
    @transactionType nvarchar(100),
    @description nvarchar(255)
as
begin
    insert into transactions (
        sender_iban, receiver_iban, transaction_datetime, 
        sender_currency, receiver_currency, sender_amount, receiver_amount, 
        transaction_type, transaction_description
    )
    values (
        @SenderIBAN, @ReceiverIBAN, GETDATE(), 
        @SenderCurrency, @ReceiverCurrency, @SenderAmount, @ReceiverAmount, 
        @TransactionType, @description
    );
end