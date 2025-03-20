CREATE OR ALTER PROCEDURE GetBankAccountsByUser @id_user INT
AS
BEGIN
	SELECT * FROM bank_accounts WHERE id_user=@id_user;
END