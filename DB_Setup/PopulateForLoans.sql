use loan_shark

SET IDENTITY_INSERT users ON;
INSERT INTO users (id_user, cnp, first_name, last_name, email, phone_number, hashed_password, password_salt)
VALUES (123, '1234567890123', 'John', 'Doe', 'john.doe@example.com', '0712345678', 'hashed_password_example', 'random_salt');
SET IDENTITY_INSERT users OFF;


INSERT INTO currencies (currency_name)
VALUES 
('EUR'),
('USD'),
('RON');


INSERT INTO bank_accounts (iban, currency, amount, id_user, custom_name)
VALUES 
('RO12345678901234567890123456789012', 'EUR', 1000.00, 123, 'Savings Account'),
('RO23456789012345678901234567890123', 'USD', 2000.00, 123, 'Checking Account'),
('RO34567890123456789012345678901234', 'RON', 5000.00, 123, 'Primary Account');


INSERT INTO loans (id_user, amount, currency, date_deadline, tax_percentage, number_months, loan_state)
VALUES
(123, 5000.00, 'EUR', DATEADD(MONTH, 12, GETDATE()), 5.5, 12, 'unpaid'),
(123, 10000.00, 'EUR', DATEADD(MONTH, 24, GETDATE()), 4.0, 24, 'unpaid'),
(123, 3000.00, 'USD', DATEADD(MONTH, 6, GETDATE()), 6.0, 6, 'unpaid'),
(123, 7000.00, 'USD', DATEADD(MONTH, 18, GETDATE()), 4.5, 18, 'paid'),
(123, 15000.00, 'RON', DATEADD(MONTH, 36, GETDATE()), 7.0, 36, 'unpaid'),
(123, 2000.00, 'RON', DATEADD(MONTH, 12, GETDATE()), 5.0, 12, 'paid');


INSERT INTO currency_exchange_rates (from_currency, to_currency, rate)
VALUES 
('EUR', 'USD', 1.10),
('EUR', 'RON', 4.95),
('USD', 'EUR', 0.91),
('USD', 'RON', 4.50),
('RON', 'EUR', 0.20),
('RON', 'USD', 0.22);