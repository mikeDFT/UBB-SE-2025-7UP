

use loan_shark;

ALTER TABLE users DROP CONSTRAINT CK__users__phone_num__59063A47

ALTER TABLE users
ADD CONSTRAINT PhoneNumberConstraint CHECK (
    phone_number LIKE '07%' AND
    LEN(phone_number) = 10 AND
    phone_number NOT LIKE '%[^0-9]%'
);

select * from transactions
select * from bank_accounts
select * from currency_exchange_rates
select * from users

INSERT INTO bank_accounts (iban, currency, amount, id_user, custom_name, daily_limit, max_per_transaction, max_nr_transactions_daily, blocked)
VALUES 
    ('RO45SEUP0000000000000000', 'USD', 50000, 6, 'BANK', 50000, 50000, 100, 0);

ALTER TABLE loan_shark
DROP CONSTRAINT CK__bank_accou__iban__5BE2A6F2;

UPDATE bank_accounts
SET iban = LEFT(iban, 24)
WHERE LEN(iban) <> 24;

ALTER TABLE bank_accounts
ADD CONSTRAINT iban CHECK (LEN(iban) = 24);

select * from users

-- Re-insert data into the currencies table
INSERT INTO currencies (currency_name)
VALUES ('USD'), ('EUR'), ('RON');

-- Re-insert data into the currency_exchange_rates table
INSERT INTO currency_exchange_rates (from_currency, to_currency, rate)
VALUES 
    ('USD', 'EUR', 0.92), ('EUR', 'USD', 1.09),
    ('USD', 'RON', 4.50), ('RON', 'USD', 0.22),
    ('EUR', 'RON', 4.90), ('RON', 'EUR', 0.20);

-- Re-insert data into the users table
INSERT INTO users (cnp, first_name, last_name, email, phone_number, hashed_password, password_salt)
VALUES 
    ('1234567890123', 'Andrei', 'Popescu', 'andrei.popescu@email.com', '0723456789', 'hashed_pwd_1', 'salt_1'),
    ('2345678901234', 'Maria', 'Ionescu', 'maria.ionescu@email.com', '0734567890', 'hashed_pwd_2', 'salt_2'),
    ('3456789012345', 'Vlad', 'Georgescu', 'vlad.georgescu@email.com', '0745678901', 'hashed_pwd_3', 'salt_3'),
    ('4567890123456', 'Ioana', 'Dumitrescu', 'ioana.dumitrescu@email.com', '0756789012', 'hashed_pwd_4', 'salt_4'),
    ('5678901234567', 'Alex', 'Constantin', 'alex.constantin@email.com', '0767890123', 'hashed_pwd_5', 'salt_5');

-- Re-insert data into the bank_accounts table
INSERT INTO bank_accounts (iban, currency, amount, id_user, custom_name, daily_limit, max_per_transaction, max_nr_transactions_daily, blocked)
VALUES 
    ('RO12BANK0000000000000001', 'USD', 5000, 7, 'Main Account', 50000, 50000, 100, 0),
    ('RO34BANK0000000000000002', 'EUR', 3000, 8, 'Savings', 50000, 50000, 100, 0),
    ('RO56BANK0000000000000003', 'RON', 10000, 9, 'Personal', 50000, 50000, 100, 0),
    ('RO78BANK0000000000000004', 'USD', 25000, 10, 'Business Account', 50000, 50000, 100, 0),
    ('RO90BANK0000000000000005', 'EUR', 500, 11, 'Blocked Account', 50000, 50000, 100, 1); -- Blocked account

-- Re-insert data into the transactions table
INSERT INTO transactions (sender_iban, receiver_iban, transaction_datetime, sender_currency, receiver_currency, sender_amount, receiver_amount, transaction_type, transaction_description)
VALUES 
    ('RO12BANK0000000000000001', 'RO56BANK0000000000000003', GETDATE(), 'USD', 'EUR', 100, 92, 'user to user', 'Payment for services'),
    ('RO34BANK0000000000000002', 'RO56BANK0000000000000003', GETDATE(), 'EUR', 'RON', 200, 980, 'user to user', 'Loan repayment'),
    ('RO56BANK0000000000000003', 'RO78BANK0000000000000004', GETDATE(), 'RON', 'USD', 450, 100, 'user to user', 'Business transfer'),
    ('RO78BANK0000000000000004', 'RO90BANK0000000000000005', GETDATE(), 'USD', 'EUR', 50, 46, 'user to user', 'Test transaction'),
    ('RO12BANK0000000000000001', 'RO78BANK0000000000000004', GETDATE(), 'USD', 'USD', 1000, 1000, 'loan', 'Loan received from bank');

	select * from transactions


