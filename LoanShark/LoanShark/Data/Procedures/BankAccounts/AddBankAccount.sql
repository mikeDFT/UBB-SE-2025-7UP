CREATE OR ALTER PROCEDURE AddBankAccount
	@iban VARCHAR(100),
	@currency VARCHAR(5),
	@amount FLOAT,
	@id_user INT,
	@custom_name VARCHAR(100),
	@daily_limit FLOAT,
	@max_per_transaction FLOAT,
	@max_nr_transactions_daily INT,
	@blocked BIT
AS
BEGIN
	INSERT INTO bank_accounts (
		iban,
		currency,
		amount,
		id_user,
		custom_name,
		daily_limit,
		max_per_transaction,
		max_nr_transactions_daily,
		blocked
	) VALUES (
		@iban,
		@currency,
		@amount,
		@id_user,
		@custom_name,
		@daily_limit,
		@max_per_transaction,
		@max_nr_transactions_daily,
		@blocked
	);
END
