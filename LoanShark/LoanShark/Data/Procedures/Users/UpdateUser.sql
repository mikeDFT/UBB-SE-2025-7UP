CREATE OR ALTER PROCEDURE UpdateUser
    @id_user INT,
    @cnp VARCHAR(13),
    @first_name VARCHAR(50),
    @last_name VARCHAR(50),
    @email VARCHAR(100),
    @phone_number VARCHAR(20),
    @hashed_password VARCHAR(255),
    @password_salt VARCHAR(32)
AS
BEGIN
    UPDATE Users
    SET
        cnp = @cnp,
        first_name = @first_name,
        last_name = @last_name,
        email = @email,
        phone_number = @phone_number,
        hashed_password = @hashed_password,
        password_salt = @password_salt
    WHERE
        id_user = @id_user
END
GO