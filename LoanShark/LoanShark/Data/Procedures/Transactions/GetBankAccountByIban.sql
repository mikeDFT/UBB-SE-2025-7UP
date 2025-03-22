create or alter procedure GetBankAccountByIban
	@iban nvarchar(100)
as
begin
	if @iban is null or len(@iban) <> 24
    begin
        raiserror ('Invalid iban. It must be exactly 24 characters longgggggg.', 16, 1);
        return;
    end

	if not exists (select 1 from bank_accounts where iban = @iban)
    begin
        raiserror ('No bank account found for the provided iban.', 16, 1);
        return;
    end

	select * from bank_accounts where iban = @iban
end
