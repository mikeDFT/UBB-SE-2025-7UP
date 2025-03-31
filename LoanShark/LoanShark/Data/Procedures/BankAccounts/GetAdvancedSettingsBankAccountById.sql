CREATE OR ALTER PROCEDURE GetBankAccountByIBAN 
	@iban VARCHAR(100),
	@daily_limit FLOAT,
	@custom_name VARCHAR(100),
	@max_per_transaction FLOAT,
	@max_nr_transactions INT,
	@blocked BIT
AS
BEGIN
	SELECT @daily_limit=daily_limit,
	@custom_name=custom_name,
	@max_per_transaction=max_per_transaction,
	@max_nr_transactions=max_nr_transactions_daily,
	@blocked=blocked
	FROM bank_accounts WHERE iban=@iban;
END