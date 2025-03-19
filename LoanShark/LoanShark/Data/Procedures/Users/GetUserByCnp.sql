CREATE OR ALTER PROCEDURE GetUserByCNP
    @cnp varchar(13)
AS
BEGIN
    SELECT * FROM users
    WHERE cnp = @cnp;
END
GO
