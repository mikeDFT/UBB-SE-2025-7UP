CREATE OR ALTER PROCEDURE UpdateTransactionDescription
    @TransactionID INT,
    @NewDescription VARCHAR(255)
AS
BEGIN
    UPDATE transactions
    SET transaction_description = @NewDescription
    WHERE transaction_id = @TransactionID;
END;
