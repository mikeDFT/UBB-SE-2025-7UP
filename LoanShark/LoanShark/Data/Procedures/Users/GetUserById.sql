CREATE OR ALTER PROCEDURE GetUserById
    @id_user INT
AS
BEGIN
    SELECT * FROM users
    WHERE id_user = @id_user;
END
GO
