CREATE OR ALTER PROCEDURE GetUserByPhoneNumber
    @phone_number varchar(20)
AS
BEGIN
    SELECT * FROM users
    WHERE phone_number = @phone_number;
END
GO
