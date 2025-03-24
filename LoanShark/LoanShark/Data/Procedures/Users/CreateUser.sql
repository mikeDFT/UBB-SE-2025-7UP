CREATE OR ALTER PROCEDURE CreateUser
    @cnp VARCHAR(13),
    @first_name VARCHAR(50),
    @last_name VARCHAR(50),
    @email VARCHAR(100),
    @phone_number VARCHAR(20),
    @hashed_password VARCHAR(255),
    @password_salt VARCHAR(32),
    @id_user INT OUTPUT
AS
BEGIN
    INSERT INTO users (
        cnp,
        first_name,
        last_name,
        email,
        phone_number,
        hashed_password,
        password_salt
    )
    VALUES (
        @cnp,
        @first_name,
        @last_name,
        @email,
        @phone_number,
        @hashed_password,
        @password_salt
    );
    
    SET @id_user = IDENT_CURRENT('users');
END
GO