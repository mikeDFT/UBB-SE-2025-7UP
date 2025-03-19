create or alter procedure GetBankAccountTransactions
	@iban int
as
begin
	select * from transactions where sender_iban = @iban or receiver_iban = @iban
end

