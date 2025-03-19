create or alter procedure UpdateBankAccountBalance
	@iban varchar(34),
	@amount float
as
begin
	update bank_accounts
	set amount = @amount
	where iban = @iban
end

