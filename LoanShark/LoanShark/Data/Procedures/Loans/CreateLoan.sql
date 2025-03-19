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