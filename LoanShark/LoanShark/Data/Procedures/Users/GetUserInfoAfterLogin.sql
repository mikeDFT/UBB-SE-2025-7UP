CREATE OR ALTER PROCEDURE GetUserInfoAfterLogin
    @email NVARCHAR(100)
AS
BEGIN
    SELECT id_user, cnp, first_name, last_name, email, phone_number FROM Users WHERE email = @email
END
