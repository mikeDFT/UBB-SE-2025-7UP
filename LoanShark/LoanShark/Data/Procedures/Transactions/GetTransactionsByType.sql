CREATE OR ALTER PROCEDURE GetTransactionsByType
    @TransactionType NVARCHAR(50)
AS
BEGIN
    SELECT * 
    FROM transactions
    WHERE transaction_type = @TransactionType;
END;
