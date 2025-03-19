CREATE or ALTER PROCEDURE GetHashedPassword
    @id_user INT
AS
BEGIN 
    SELECT hashed_password
    FROM users
    WHERE id_user = @id_user;
END
GO