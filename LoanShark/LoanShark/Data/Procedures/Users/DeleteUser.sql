CREATE OR ALTER PROCEDURE DeleteUser
    @id_user INT
AS
BEGIN
    DELETE FROM users
    WHERE id_user = @id_user;
END
GO