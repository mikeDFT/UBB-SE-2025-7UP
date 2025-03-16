use loan_shark;
GO
CREATE OR ALTER TRIGGER trg_update_transactions_on_bank_account_delete
ON bank_accounts
AFTER DELETE
AS
BEGIN
    -- Update transactions where sender_iban matches the deleted iban, set sender_iban to NULL
    UPDATE transactions
    SET sender_iban = NULL
    FROM transactions t
    INNER JOIN deleted d ON t.sender_iban = d.iban;

    -- Update transactions where receiver_iban matches the deleted iban, set receiver_iban to NULL
    UPDATE transactions
    SET receiver_iban = NULL
    FROM transactions t
    INNER JOIN deleted d ON t.receiver_iban = d.iban;

    -- Delete transactions where both sender_iban and receiver_iban are NULL
    DELETE FROM transactions
    WHERE sender_iban IS NULL AND receiver_iban IS NULL;
END;

GO
CREATE TRIGGER trg_delete_currency_exchange_rates_on_currency_delete
ON currencies
AFTER DELETE
AS
BEGIN
    -- Delete from currency_exchange_rates where from_currency matches the deleted currency
    DELETE FROM currency_exchange_rates
    WHERE from_currency IN (SELECT currency_name FROM deleted);

    -- Delete from currency_exchange_rates where to_currency matches the deleted currency
    DELETE FROM currency_exchange_rates
    WHERE to_currency IN (SELECT currency_name FROM deleted);
END;