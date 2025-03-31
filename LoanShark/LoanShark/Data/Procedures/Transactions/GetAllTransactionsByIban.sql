create or alter procedure GetAllTransactionsByIban
	@Iban VARCHAR(100)
as
begin
	select * from transactions
	where sender_iban = @Iban or receiver_iban = @Iban
end
