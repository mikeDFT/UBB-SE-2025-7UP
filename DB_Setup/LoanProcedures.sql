USE loan_shark

GO
CREATE OR ALTER PROCEDURE CreateLoan
    @id_user INT,
    @amount FLOAT,
    @currency VARCHAR(5),
    @date_deadline DATETIME,
    @tax_percentage FLOAT,
    @number_months INT,
    @loan_state VARCHAR(50),
    @id_loan INT OUTPUT
AS
BEGIN
    INSERT INTO loans (
        id_user,
        amount,
        currency,
        date_deadline,
        tax_percentage,
        number_months,
        loan_state
    )
    VALUES (
        @id_user,
        @amount,
        @currency,
        @date_deadline,
        @tax_percentage,
        @number_months,
        @loan_state
    );
    
    SET @id_loan = IDENT_CURRENT('loans');
END
GO


CREATE OR ALTER PROCEDURE DeleteLoan
    @id_loan INT
AS
BEGIN
    DELETE FROM loans
    WHERE id_loan = @id_loan;
END
GO


CREATE OR ALTER PROCEDURE GetAllCurrencyExchanges
AS
BEGIN
    SELECT * FROM currency_exchange_rates;
END

GO
CREATE OR ALTER PROCEDURE GetAllLoans
AS
BEGIN
    SELECT * FROM loans;
END
GO



CREATE OR ALTER PROCEDURE GetBankAccountsByUserId
    @id_user INT
AS
BEGIN
    SELECT * FROM bank_accounts WHERE id_user = @id_user;
END

GO
CREATE OR ALTER PROCEDURE GetLoanById
    @id_loan INT
AS
BEGIN
    SELECT * FROM loans
    WHERE id_loan = @id_loan;
END
GO

CREATE OR ALTER PROCEDURE GetLoansByUserId
    @id_user INT
AS
BEGIN
    SELECT * FROM loans
    WHERE id_user = @id_user;
END
GO

CREATE OR ALTER PROCEDURE UpdateLoan
    @id_loan INT,
    @amount FLOAT,
    @currency VARCHAR(5),
    @date_deadline DATETIME,
    @date_paid DATETIME = NULL,
    @tax_percentage FLOAT,
    @loan_state VARCHAR(50)
AS
BEGIN
    UPDATE loans
    SET
        amount = @amount,
        currency = @currency,
        date_deadline = @date_deadline,
        date_paid = @date_paid,
        tax_percentage = @tax_percentage,
        loan_state = @loan_state
    WHERE id_loan = @id_loan;
END
GO