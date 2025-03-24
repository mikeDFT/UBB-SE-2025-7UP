CREATE OR ALTER PROCEDURE GetBankAccountsByUserId
    @id_user INT
AS
BEGIN
    SELECT * FROM bank_accounts WHERE id_user = @id_user;
END

