CREATE OR ALTER PROCEDURE GetUserBankAccounts
    @id_user int
AS
BEGIN
    SELECT * FROM bank_accounts WHERE id_user = @id_user ORDER BY date_created ASC;
END;
