CREATE OR ALTER PROCEDURE RemoveBankAccount
	@iban VARCHAR(100)
AS
BEGIN
	BEGIN TRANSACTION;
	-- First set referencing values to NULL form the transactions table
	UPDATE transactions
	SET sender_iban = NULL
	WHERE sender_iban = @iban;

	UPDATE transactions
	SET receiver_iban = NULL
	WHERE receiver_iban = @iban;

	DELETE FROM bank_accounts WHERE iban=@iban;
	COMMIT TRANSACTION;
END
GO
