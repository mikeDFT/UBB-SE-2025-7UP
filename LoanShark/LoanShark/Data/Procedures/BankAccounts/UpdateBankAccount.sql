CREATE OR ALTER PROCEDURE UpdateBankAccount
	@iban VARCHAR(100),
	@custom_name VARCHAR(100),
	@daily_limit FLOAT,
	@max_per_transaction FLOAT,
	@max_nr_transactions_daily INT,
	@blocked BIT
AS
BEGIN
	UPDATE bank_accounts
	SET custom_name = @custom_name, 
	daily_limit = @daily_limit,
	max_per_transaction = @max_per_transaction,
	max_nr_transactions_daily = @max_nr_transactions_daily,
	blocked = @blocked
	WHERE iban = @iban
END
GO