create or alter procedure UpdateBankAccountBalance
	@iban varchar(100),
	@amount float
as
begin
	if @iban is NULL OR len(@iban) <> 24
    begin
        RAISERROR ('Invalid IBAN. It must be exactly 24 characters long.', 16, 1);
        return;
    end

	if @amount < 0
    begin
        raiserror ('Invalid balance. Amount cannot be negative.', 16, 1);
        return;
    end

	if NOT EXISTS (select 1 from bank_accounts where iban = @iban)
    begin
        raiserror ('Bank account with the provided IBAN does not exist.', 16, 1);
        return;
    end

	update bank_accounts
	set amount = @amount
	where iban = @iban
end

