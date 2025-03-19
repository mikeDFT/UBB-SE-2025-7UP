CREATE OR ALTER PROCEDURE UpdateLoan
    @id_loan INT,
    @amount FLOAT,
    @currency VARCHAR(5),
    @date_deadline DATETIME,
    @date_paid DATETIME = NULL,
    @tax_percentage FLOAT,
    @loan_state VARCHAR(50)
AS
BEGIN
    UPDATE loans
    SET
        amount = @amount,
        currency = @currency,
        date_deadline = @date_deadline,
        date_paid = @date_paid,
        tax_percentage = @tax_percentage,
        loan_state = @loan_state
    WHERE id_loan = @id_loan;
END
GO