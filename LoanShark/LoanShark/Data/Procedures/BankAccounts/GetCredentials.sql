CREATE OR ALTER PROCEDURE GetCredentials
@email VARCHAR(100)
AS
BEGIN
	SELECT hashed_password, password_salt FROM users WHERE email=@email;
END