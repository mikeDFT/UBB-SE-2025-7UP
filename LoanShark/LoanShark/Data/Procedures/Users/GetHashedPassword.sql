CREATE or ALTER PROCEDURE GetHashedPassword
    @id_user INT,
    @hashed_password VARCHAR(255) OUTPUT,
    @password_salt VARCHAR(32) OUTPUT
AS
BEGIN 
    SELECT @hashed_password = hashed_password, 
    @password_salt = password_salt
    FROM users
    WHERE id_user = @id_user;
END
GO