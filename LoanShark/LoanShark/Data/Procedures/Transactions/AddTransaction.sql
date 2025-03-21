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
	if @senderIban is null or len(@senderIban) <> 24
    begin
        raiserror ('Invalid sender iban. It must be exactly 24 characters long.', 16, 1);
        return;
    end

	if @receiverIban is null or len(@receiverIban) <> 24
    begin
        raiserror ('Invalid receiver iban. It must be exactly 24 characters long.', 16, 1);
        return;
    end

	if @senderIban = @receiverIban
    begin
        raiserror ('Transaction error: Sender and receiver iban cannot be the same.', 16, 1);
        return;
    end

	if not exists (select 1 from currencies where currency_name = @senderCurrency)
    begin
        raiserror ('Invalid sender currency.', 16, 1);
        return;
    end

	if not exists (select 1 from currencies where currency_name = @receiverCurrency)
    begin
        raiserror ('Invalid receiver currency.', 16, 1);
        return;
    end

	if not exists (select 1 from bank_accounts where iban = @senderIban)
    begin
        raiserror ('Sender bank account does not exist.', 16, 1);
        return;
    end

	if not exists (select 1 from bank_accounts where iban = @receiverIban)
    begin
        raiserror ('Receiver bank account does not exist.', 16, 1);
        return;
    end

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

