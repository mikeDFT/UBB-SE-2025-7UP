create database loan_shark;

use loan_shark;

CREATE TABLE users (
    id_user INT PRIMARY KEY IDENTITY(1,1),
    cnp VARCHAR(13) NOT NULL UNIQUE,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    phone_number VARCHAR(20) NOT NULL UNIQUE CHECK (phone_number LIKE '^07[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    hashed_password VARCHAR(255) NOT NULL,
    password_salt VARCHAR(32) NOT NULL
);

CREATE TABLE bank_accounts (
    iban VARCHAR(100) PRIMARY KEY CHECK (LEN(iban) = 34),
    currency VARCHAR(5) NOT NULL,
    amount FLOAT NOT NULL DEFAULT 0,
    id_user INT NOT NULL,
    custom_name VARCHAR(100),
    daily_limit FLOAT NOT NULL DEFAULT 50000,
    max_per_transaction FLOAT NOT NULL DEFAULT 50000,
    max_nr_transactions_daily INT NOT NULL DEFAULT 100,
    blocked BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (id_user) REFERENCES users(id_user) ON DELETE CASCADE
);

CREATE TABLE transactions (
    transaction_id INT PRIMARY KEY IDENTITY(1,1),
    sender_iban VARCHAR(100) CHECK (LEN(sender_iban) = 34),
    receiver_iban VARCHAR(100) CHECK (LEN(receiver_iban) = 34),
    transaction_datetime DATETIME DEFAULT CURRENT_TIMESTAMP,
    sender_currency VARCHAR(5) NOT NULL,
    receiver_currency VARCHAR(5) NOT NULL,
    sender_amount FLOAT NOT NULL CHECK (sender_amount > 0),
    receiver_amount FLOAT NOT NULL CHECK (receiver_amount > 0),
    transaction_type VARCHAR(100) NOT NULL CHECK (transaction_type IN ('user to user', 'loan', 'stocks')),
    transaction_description VARCHAR(50) DEFAULT '',
    FOREIGN KEY (sender_iban) REFERENCES bank_accounts(iban),
    FOREIGN KEY (receiver_iban) REFERENCES bank_accounts(iban)
);

CREATE TABLE loans (
    id_loan INT PRIMARY KEY IDENTITY(1,1),
    id_user INT NOT NULL,
    amount FLOAT NOT NULL CHECK (amount > 0),
    currency VARCHAR(5) NOT NULL,
    date_taken DATETIME DEFAULT CURRENT_TIMESTAMP,
    date_deadline DATETIME NOT NULL,
    date_paid DATETIME DEFAULT NULL,
    tax_percentage FLOAT NOT NULL CHECK (tax_percentage > 0),
    number_months INT NOT NULL CHECK (number_months > 0),
    loan_state VARCHAR(50) NOT NULL CHECK (loan_state IN ('paid', 'unpaid')),
    FOREIGN KEY (id_user) REFERENCES users(id_user) ON DELETE CASCADE
);

CREATE TABLE currencies (
	currency_name varchar(5) primary key
);

CREATE TABLE currency_exchange_rates (
    from_currency VARCHAR(5) NOT NULL,
    to_currency VARCHAR(5) NOT NULL,
    rate FLOAT NOT NULL CHECK (rate > 0),
	FOREIGN KEY from_currency REFERENCES currencies(currency_name) ON DELETE CASCADE,
	FOREIGN KEY to_currency REFERENCES currencies(currency_name) ON DELETE CASCADE,
    PRIMARY KEY (from_currency, to_currency)
);