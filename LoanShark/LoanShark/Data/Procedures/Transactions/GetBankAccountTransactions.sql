create or alter procedure GetBankAccountTransactions
	@iban varchar(100)
as
begin
	if @iban is null or len(@iban) <> 24
    begin
        raiserror ('Invalid iban. It must be exactly 24 characters long.', 16, 1);
        return;
    end

	if not exists (select 1 from bank_accounts where iban = @iban)
    begin
        raiserror ('Bank account not found for the provided iban.', 16, 1);
        return;
    end

	select * 
	from transactions 
	where sender_iban = @iban or receiver_iban = @iban
end
