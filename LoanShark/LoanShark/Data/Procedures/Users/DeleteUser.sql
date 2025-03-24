CREATE OR ALTER PROCEDURE DeleteUser
	@id_user INT
AS
BEGIN
	-- Check if user exists
	IF NOT EXISTS (SELECT 1 FROM users WHERE id_user = @id_user)
	BEGIN
		RAISERROR('User does not exist', 16, 1);
		RETURN;
	END

	BEGIN TRANSACTION;

	DECLARE @iban VARCHAR(100)
	DECLARE @error BIT = 0

	-- Create a cursor to iterate through all bank accounts of this user
	DECLARE bank_accounts_cursor CURSOR FOR
	SELECT iban from bank_accounts WHERE id_user = @id_user

	OPEN bank_accounts_cursor
	FETCH NEXT FROM bank_accounts_cursor INTO @iban

	WHILE @@FETCH_STATUS = 0
	BEGIN
		BEGIN TRY
			-- Call the RemoveBankAccount procedure to delete the bank account
			EXEC RemoveBankAccount @iban
		END TRY
		BEGIN CATCH
			SET @error = 1
			BREAK
		END CATCH

		FETCH NEXT FROM bank_accounts_cursor INTO @iban
	END

	CLOSE bank_accounts_cursor
	DEALLOCATE bank_accounts_cursor

	-- if all accounts were deleted successfully, delete the user
	IF @error = 0
	BEGIN
		DELETE FROM users WHERE id_user = @id_user

		IF @@ERROR <> 0
		BEGIN
			ROLLBACK TRANSACTION
			RAISERROR('Error deleting user', 16, 1)
			RETURN
		END
	END
	ELSE
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR('One or more errors occured while deleting bank accounts. User deletion aborted', 16, 1)
		RETURN
	END

	COMMIT TRANSACTION
END
GO
