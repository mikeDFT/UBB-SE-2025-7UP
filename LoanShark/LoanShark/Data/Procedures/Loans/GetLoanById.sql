CREATE OR ALTER PROCEDURE GetLoanById
    @id_loan INT
AS
BEGIN
    SELECT * FROM loans
    WHERE id_loan = @id_loan;
END
GO