CREATE OR ALTER PROCEDURE GetLoansByUserId
    @id_user INT
AS
BEGIN
    SELECT * FROM loans
    WHERE id_user = @id_user;
END
GO