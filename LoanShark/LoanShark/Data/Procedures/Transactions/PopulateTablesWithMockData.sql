INSERT INTO currencies (currency_name)
VALUES ('USD'), ('EUR'), ('RON');


INSERT INTO currency_exchange_rates (from_currency, to_currency, rate)
VALUES 
    ('USD', 'EUR', 0.92), ('EUR', 'USD', 1.09),
    ('USD', 'RON', 4.50), ('RON', 'USD', 0.22),
    ('EUR', 'RON', 4.90), ('RON', 'EUR', 0.20);

INSERT INTO users (cnp, first_name, last_name, email, phone_number, hashed_password, password_salt)
VALUES 
    ('1234567890123', 'Andrei', 'Popescu', 'andrei.popescu@email.com', '0723456789', 'hashed_pwd_1', 'salt_1'),
    ('2345678901234', 'Maria', 'Ionescu', 'maria.ionescu@email.com', '0734567890', 'hashed_pwd_2', 'salt_2'),
    ('3456789012345', 'Vlad', 'Georgescu', 'vlad.georgescu@email.com', '0745678901', 'hashed_pwd_3', 'salt_3'),
    ('4567890123456', 'Ioana', 'Dumitrescu', 'ioana.dumitrescu@email.com', '0756789012', 'hashed_pwd_4', 'salt_4'),
    ('5678901234567', 'Alex', 'Constantin', 'alex.constantin@email.com', '0767890123', 'hashed_pwd_5', 'salt_5');



INSERT INTO bank_accounts (iban, currency, amount, id_user, custom_name, daily_limit, max_per_transaction, max_nr_transactions_daily, blocked)
VALUES 
    ('RO12BANK00000000000000000000000001', 'USD', 5000, 6, 'Main Account', 50000, 50000, 100, 0),
    ('RO34BANK00000000000000000000000002', 'EUR', 3000, 2, 'Savings', 50000, 50000, 100, 0),
    ('RO56BANK00000000000000000000000003', 'RON', 10000, 3, 'Personal', 50000, 50000, 100, 0),
    ('RO78BANK00000000000000000000000004', 'USD', 25000, 4, 'Business Account', 50000, 50000, 100, 0),
    ('RO90BANK00000000000000000000000005', 'EUR', 500, 5, 'Blocked Account', 50000, 50000, 100, 1); -- Blocked account


INSERT INTO transactions (sender_iban, receiver_iban, transaction_datetime, sender_currency, receiver_currency, sender_amount, receiver_amount, transaction_type, transaction_description)
VALUES 
    ('RO12BANK00000000000000000000000001', 'RO34BANK00000000000000000000000002', GETDATE(), 'USD', 'EUR', 100, 92, 'user to user', 'Payment for services'),
    ('RO34BANK00000000000000000000000002', 'RO56BANK00000000000000000000000003', GETDATE(), 'EUR', 'RON', 200, 980, 'user to user', 'Loan repayment'),
    ('RO56BANK00000000000000000000000003', 'RO78BANK00000000000000000000000004', GETDATE(), 'RON', 'USD', 450, 100, 'user to user', 'Business transfer'),
    ('RO78BANK00000000000000000000000004', 'RO90BANK00000000000000000000000005', GETDATE(), 'USD', 'EUR', 50, 46, 'user to user', 'Test transaction'),
    ('RO12BANK00000000000000000000000001', 'RO12BANK00000000000000000000000001', GETDATE(), 'USD', 'USD', 1000, 1000, 'loan', 'Loan received from bank');

select * from users
use loan_shark;



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