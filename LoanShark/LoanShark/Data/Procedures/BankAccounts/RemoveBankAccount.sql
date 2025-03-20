CREATE OR ALTER PROCEDURE RemoveBankAccount
	@iban VARCHAR(100)
AS
BEGIN
	DELETE FROM bank_accounts WHERE iban=@iban;
END