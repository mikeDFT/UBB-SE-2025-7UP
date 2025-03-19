create or alter procedure GetBankAccountByIban
	@iban nvarchar(34)
as
begin
	select * from bank_accounts where iban = @iban
end
