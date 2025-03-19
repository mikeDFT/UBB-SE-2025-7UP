CREATE OR ALTER PROCEDURE DeleteLoan
    @id_loan INT
AS
BEGIN
    DELETE FROM loans
    WHERE id_loan = @id_loan;
END
GO