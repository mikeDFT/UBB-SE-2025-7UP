CREATE OR ALTER PROCEDURE GetBankAccountBalanceByIban
    @iban NVARCHAR(50)
AS
BEGIN
    SELECT amount, currency FROM BankAccount WHERE iban=@iban;
END
