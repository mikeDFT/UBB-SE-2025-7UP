CREATE OR ALTER PROCEDURE GetBankAccountBalanceByUserIban
    @iban NVARCHAR(50)
AS
BEGIN
    SELECT amount, currency FROM BankAccount WHERE iban=@iban;
END
