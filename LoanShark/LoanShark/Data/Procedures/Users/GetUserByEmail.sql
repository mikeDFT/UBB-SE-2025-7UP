CREATE OR ALTER PROCEDURE GetUserByEmail
    @email varchar(100)
AS
BEGIN
    SELECT * FROM users
    WHERE email = @email;
END
GO
