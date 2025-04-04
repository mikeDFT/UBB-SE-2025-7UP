USE [master]
GO
/****** Object:  Database [loan_shark]    Script Date: 3/23/2025 11:29:27 PM ******/
CREATE DATABASE [loan_shark]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'loan_shark', FILENAME = N'D:\ALEX\SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\loan_shark.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'loan_shark_log', FILENAME = N'D:\ALEX\SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\loan_shark_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [loan_shark] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [loan_shark].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [loan_shark] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [loan_shark] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [loan_shark] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [loan_shark] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [loan_shark] SET ARITHABORT OFF 
GO
ALTER DATABASE [loan_shark] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [loan_shark] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [loan_shark] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [loan_shark] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [loan_shark] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [loan_shark] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [loan_shark] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [loan_shark] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [loan_shark] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [loan_shark] SET  ENABLE_BROKER 
GO
ALTER DATABASE [loan_shark] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [loan_shark] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [loan_shark] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [loan_shark] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [loan_shark] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [loan_shark] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [loan_shark] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [loan_shark] SET RECOVERY FULL 
GO
ALTER DATABASE [loan_shark] SET  MULTI_USER 
GO
ALTER DATABASE [loan_shark] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [loan_shark] SET DB_CHAINING OFF 
GO
ALTER DATABASE [loan_shark] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [loan_shark] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [loan_shark] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [loan_shark] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'loan_shark', N'ON'
GO
ALTER DATABASE [loan_shark] SET QUERY_STORE = ON
GO
ALTER DATABASE [loan_shark] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [loan_shark]
GO
/****** Object:  Table [dbo].[bank_accounts]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bank_accounts](
	[iban] [varchar](100) NOT NULL,
	[currency] [varchar](5) NOT NULL,
	[amount] [float] NOT NULL,
	[id_user] [int] NOT NULL,
	[custom_name] [varchar](100) NULL,
	[daily_limit] [float] NOT NULL,
	[max_per_transaction] [float] NOT NULL,
	[max_nr_transactions_daily] [int] NOT NULL,
	[blocked] [bit] NOT NULL,
	[date_created] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[iban] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[currencies]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[currencies](
	[currency_name] [varchar](5) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[currency_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[currency_exchange_rates]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[currency_exchange_rates](
	[from_currency] [varchar](5) NOT NULL,
	[to_currency] [varchar](5) NOT NULL,
	[rate] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[from_currency] ASC,
	[to_currency] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[loans]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[loans](
	[id_loan] [int] IDENTITY(1,1) NOT NULL,
	[id_user] [int] NOT NULL,
	[amount] [float] NOT NULL,
	[currency] [varchar](5) NOT NULL,
	[date_taken] [datetime] NULL,
	[date_deadline] [datetime] NOT NULL,
	[date_paid] [datetime] NULL,
	[tax_percentage] [float] NOT NULL,
	[number_months] [int] NOT NULL,
	[loan_state] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_loan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[transactions]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[transactions](
	[transaction_id] [int] IDENTITY(1,1) NOT NULL,
	[sender_iban] [varchar](100) NULL,
	[receiver_iban] [varchar](100) NULL,
	[transaction_datetime] [datetime] NULL,
	[sender_currency] [varchar](5) NOT NULL,
	[receiver_currency] [varchar](5) NOT NULL,
	[sender_amount] [float] NOT NULL,
	[receiver_amount] [float] NOT NULL,
	[transaction_type] [varchar](100) NOT NULL,
	[transaction_description] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[transaction_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id_user] [int] IDENTITY(1,1) NOT NULL,
	[cnp] [varchar](13) NOT NULL,
	[first_name] [varchar](50) NOT NULL,
	[last_name] [varchar](50) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[phone_number] [varchar](20) NOT NULL,
	[hashed_password] [varchar](255) NOT NULL,
	[password_salt] [varchar](32) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_user] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[phone_number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[cnp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[bank_accounts] ADD  DEFAULT ((0)) FOR [amount]
GO
ALTER TABLE [dbo].[bank_accounts] ADD  DEFAULT ((50000)) FOR [daily_limit]
GO
ALTER TABLE [dbo].[bank_accounts] ADD  DEFAULT ((50000)) FOR [max_per_transaction]
GO
ALTER TABLE [dbo].[bank_accounts] ADD  DEFAULT ((100)) FOR [max_nr_transactions_daily]
GO
ALTER TABLE [dbo].[bank_accounts] ADD  DEFAULT ((0)) FOR [blocked]
GO
ALTER TABLE [dbo].[bank_accounts] ADD  DEFAULT (getdate()) FOR [date_created]
GO
ALTER TABLE [dbo].[loans] ADD  DEFAULT (getdate()) FOR [date_taken]
GO
ALTER TABLE [dbo].[loans] ADD  DEFAULT (NULL) FOR [date_paid]
GO
ALTER TABLE [dbo].[transactions] ADD  DEFAULT (getdate()) FOR [transaction_datetime]
GO
ALTER TABLE [dbo].[transactions] ADD  DEFAULT ('') FOR [transaction_description]
GO
ALTER TABLE [dbo].[bank_accounts]  WITH CHECK ADD FOREIGN KEY([currency])
REFERENCES [dbo].[currencies] ([currency_name])
GO
ALTER TABLE [dbo].[bank_accounts]  WITH CHECK ADD FOREIGN KEY([id_user])
REFERENCES [dbo].[users] ([id_user])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[currency_exchange_rates]  WITH CHECK ADD FOREIGN KEY([from_currency])
REFERENCES [dbo].[currencies] ([currency_name])
GO
ALTER TABLE [dbo].[currency_exchange_rates]  WITH CHECK ADD FOREIGN KEY([to_currency])
REFERENCES [dbo].[currencies] ([currency_name])
GO
ALTER TABLE [dbo].[loans]  WITH CHECK ADD FOREIGN KEY([currency])
REFERENCES [dbo].[currencies] ([currency_name])
GO
ALTER TABLE [dbo].[loans]  WITH CHECK ADD FOREIGN KEY([id_user])
REFERENCES [dbo].[users] ([id_user])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[transactions]  WITH CHECK ADD FOREIGN KEY([receiver_iban])
REFERENCES [dbo].[bank_accounts] ([iban])
GO
ALTER TABLE [dbo].[transactions]  WITH CHECK ADD FOREIGN KEY([receiver_currency])
REFERENCES [dbo].[currencies] ([currency_name])
GO
ALTER TABLE [dbo].[transactions]  WITH CHECK ADD FOREIGN KEY([sender_iban])
REFERENCES [dbo].[bank_accounts] ([iban])
GO
ALTER TABLE [dbo].[transactions]  WITH CHECK ADD FOREIGN KEY([sender_currency])
REFERENCES [dbo].[currencies] ([currency_name])
GO
ALTER TABLE [dbo].[bank_accounts]  WITH CHECK ADD  CONSTRAINT [CK__bank_accou__iban__7C4F7684] CHECK  ((len([iban])=(24)))
GO
ALTER TABLE [dbo].[bank_accounts] CHECK CONSTRAINT [CK__bank_accou__iban__7C4F7684]
GO
ALTER TABLE [dbo].[currency_exchange_rates]  WITH CHECK ADD CHECK  (([rate]>(0)))
GO
ALTER TABLE [dbo].[loans]  WITH CHECK ADD CHECK  (([amount]>(0)))
GO
ALTER TABLE [dbo].[loans]  WITH CHECK ADD CHECK  (([loan_state]='unpaid' OR [loan_state]='paid'))
GO
ALTER TABLE [dbo].[loans]  WITH CHECK ADD CHECK  (([number_months]>(0)))
GO
ALTER TABLE [dbo].[loans]  WITH CHECK ADD CHECK  (([tax_percentage]>(0)))
GO
ALTER TABLE [dbo].[transactions]  WITH CHECK ADD  CONSTRAINT [Check_Different_Accounts] CHECK  (([sender_iban]<>[receiver_iban] OR [sender_iban] IS NULL OR [receiver_iban] IS NULL))
GO
ALTER TABLE [dbo].[transactions] CHECK CONSTRAINT [Check_Different_Accounts]
GO
ALTER TABLE [dbo].[transactions]  WITH CHECK ADD  CONSTRAINT [CK__transacti__recei__06CD04F7] CHECK  ((len([receiver_iban])=(24)))
GO
ALTER TABLE [dbo].[transactions] CHECK CONSTRAINT [CK__transacti__recei__06CD04F7]
GO
ALTER TABLE [dbo].[transactions]  WITH CHECK ADD CHECK  (([receiver_amount]>(0)))
GO
ALTER TABLE [dbo].[transactions]  WITH CHECK ADD  CONSTRAINT [CK__transacti__sende__05D8E0BE] CHECK  ((len([sender_iban])=(24)))
GO
ALTER TABLE [dbo].[transactions] CHECK CONSTRAINT [CK__transacti__sende__05D8E0BE]
GO
ALTER TABLE [dbo].[transactions]  WITH CHECK ADD CHECK  (([sender_amount]>(0)))
GO
ALTER TABLE [dbo].[transactions]  WITH CHECK ADD CHECK  (([transaction_type]='stocks' OR [transaction_type]='loan' OR [transaction_type]='user to user'))
GO
ALTER TABLE [dbo].[users]  WITH CHECK ADD  CONSTRAINT [PhoneNumberConstraint] CHECK  (([phone_number] like '07%' AND len([phone_number])=(10) AND NOT [phone_number] like '%[^0-9]%'))
GO
ALTER TABLE [dbo].[users] CHECK CONSTRAINT [PhoneNumberConstraint]
GO
/****** Object:  StoredProcedure [dbo].[AddBankAccount]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[AddBankAccount]
	@iban VARCHAR(100),
	@currency VARCHAR(5),
	@amount FLOAT,
	@id_user INT,
	@custom_name VARCHAR(100),
	@daily_limit FLOAT,
	@max_per_transaction FLOAT,
	@max_nr_transactions_daily INT,
	@blocked BIT
AS
BEGIN
	INSERT INTO bank_accounts (
		iban,
		currency,
		amount,
		id_user,
		custom_name,
		daily_limit,
		max_per_transaction,
		max_nr_transactions_daily,
		blocked
	) VALUES (
		@iban,
		@currency,
		@amount,
		@id_user,
		@custom_name,
		@daily_limit,
		@max_per_transaction,
		@max_nr_transactions_daily,
		@blocked
	);
END
GO
/****** Object:  StoredProcedure [dbo].[AddTransaction]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[AddTransaction]
    @senderIban nvarchar(100),
    @receiverIban nvarchar(100),
    @senderCurrency nvarchar(5),
    @receiverCurrency nvarchar(5),
    @senderAmount float,
    @receiverAmount float,
    @transactionType nvarchar(100),
    @description nvarchar(255)
as
begin
	if @senderIban is null or len(@senderIban) <> 24
    begin
        raiserror ('Invalid sender iban. It must be exactly 24 characters long.', 16, 1);
        return;
    end

	if @receiverIban is null or len(@receiverIban) <> 24
    begin
        raiserror ('Invalid receiver iban. It must be exactly 24 characters long.', 16, 1);
        return;
    end

	if @senderIban = @receiverIban
    begin
        raiserror ('Transaction error: Sender and receiver iban cannot be the same.', 16, 1);
        return;
    end

	if not exists (select 1 from currencies where currency_name = @senderCurrency)
    begin
        raiserror ('Invalid sender currency.', 16, 1);
        return;
    end

	if not exists (select 1 from currencies where currency_name = @receiverCurrency)
    begin
        raiserror ('Invalid receiver currency.', 16, 1);
        return;
    end

	if not exists (select 1 from bank_accounts where iban = @senderIban)
    begin
        raiserror ('Sender bank account does not exist.', 16, 1);
        return;
    end

	if not exists (select 1 from bank_accounts where iban = @receiverIban)
    begin
        raiserror ('Receiver bank account does not exist.', 16, 1);
        return;
    end

    insert into transactions (
        sender_iban, receiver_iban, transaction_datetime, 
        sender_currency, receiver_currency, sender_amount, receiver_amount, 
        transaction_type, transaction_description
    )
    values (
        @SenderIBAN, @ReceiverIBAN, GETDATE(), 
        @SenderCurrency, @ReceiverCurrency, @SenderAmount, @ReceiverAmount, 
        @TransactionType, @description
    );
end
GO
/****** Object:  StoredProcedure [dbo].[CreateLoan]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[CreateLoan]
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
/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[CreateUser]
    @cnp VARCHAR(13),
    @first_name VARCHAR(50),
    @last_name VARCHAR(50),
    @email VARCHAR(100),
    @phone_number VARCHAR(20),
    @hashed_password VARCHAR(255),
    @password_salt VARCHAR(32),
    @id_user INT OUTPUT
AS
BEGIN
    INSERT INTO users (
        cnp,
        first_name,
        last_name,
        email,
        phone_number,
        hashed_password,
        password_salt
    )
    VALUES (
        @cnp,
        @first_name,
        @last_name,
        @email,
        @phone_number,
        @hashed_password,
        @password_salt
    );
    
    SET @id_user = IDENT_CURRENT('users');
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteLoan]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[DeleteLoan]
    @id_loan INT
AS
BEGIN
    DELETE FROM loans
    WHERE id_loan = @id_loan;
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteUser]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[DeleteUser]
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
/****** Object:  StoredProcedure [dbo].[GetAllBankAccounts]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[GetAllBankAccounts]
as
begin
	select * from bank_accounts
end
GO
/****** Object:  StoredProcedure [dbo].[GetAllCurrencyExchangeRates]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetAllCurrencyExchangeRates]
AS
BEGIN
    SELECT * FROM currency_exchange_rates;
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllCurrencyExchanges]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetAllCurrencyExchanges]
AS
BEGIN
    SELECT * FROM currency_exchange_rates;
END

GO
/****** Object:  StoredProcedure [dbo].[GetAllCurrencyExhangeRates]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[GetAllCurrencyExhangeRates]
as
begin
	select * from currency_exchange_rates
end
GO
/****** Object:  StoredProcedure [dbo].[GetAllLoans]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetAllLoans]
AS
BEGIN
    SELECT * FROM loans;
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllTransactionsByIban]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[GetAllTransactionsByIban]
	@Iban VARCHAR(100)
as
begin
	select * from transactions
	where sender_iban = @Iban or receiver_iban = @Iban
end
GO
/****** Object:  StoredProcedure [dbo].[GetBankAccountBalanceByIban]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetBankAccountBalanceByIban]
    @iban NVARCHAR(50)
AS
BEGIN
    SELECT amount, currency FROM bank_accounts WHERE iban=@iban;
END
GO
/****** Object:  StoredProcedure [dbo].[GetBankAccountByIBAN]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[GetBankAccountByIBAN]
	@iban nvarchar(100)
as
begin
	if @iban is null or len(@iban) <> 24
    begin
        raiserror ('Invalid iban. It must be exactly 24 characters longgggggg.', 16, 1);
        return;
    end

	if not exists (select 1 from bank_accounts where iban = @iban)
    begin
        raiserror ('No bank account found for the provided iban.', 16, 1);
        return;
    end

	select * from bank_accounts where iban = @iban
end
GO
/****** Object:  StoredProcedure [dbo].[GetBankAccountsByUser]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetBankAccountsByUser] @id_user INT
AS
BEGIN
	SELECT * FROM bank_accounts WHERE id_user=@id_user;
END
GO
/****** Object:  StoredProcedure [dbo].[GetBankAccountsByUserId]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetBankAccountsByUserId]
    @id_user INT
AS
BEGIN
    SELECT * FROM bank_accounts WHERE id_user = @id_user;
END

GO
/****** Object:  StoredProcedure [dbo].[GetBankAccountTransactions]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[GetBankAccountTransactions]
	@iban varchar(100)
as
begin
	if @iban is null or len(@iban) <> 24
    begin
        raiserror ('Invalid iban. It must be exactly 24 characters long.', 16, 1);
        return;
    end

	if not exists (select 1 from bank_accounts where iban = @iban)
    begin
        raiserror ('Bank account not found for the provided iban.', 16, 1);
        return;
    end

	select * 
	from transactions 
	where sender_iban = @iban or receiver_iban = @iban
end
GO
/****** Object:  StoredProcedure [dbo].[GetCredentials]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetCredentials]
@email VARCHAR(100)
AS
BEGIN
	SELECT hashed_password, password_salt FROM users WHERE email=@email;
END
GO
/****** Object:  StoredProcedure [dbo].[GetCurrencies]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetCurrencies]
AS
BEGIN
	SELECT * FROM currencies;
END
GO
/****** Object:  StoredProcedure [dbo].[GetExchangeRate]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[GetExchangeRate]
	@fromCurrency nvarchar(5),
	@toCurrency nvarchar(5)
as
begin
	if @fromCurrency is null or len(@fromCurrency) = 0
    begin
        raiserror ('invalid fromCurrency. it cannot be null or empty.', 16, 1);
        return;
    end

    if @toCurrency is null or len(@toCurrency) = 0
    begin
        raiserror ('invalid toCurrency. it cannot be null or empty.', 16, 1);
        return;
    end

	if not exists (select 1 from currency_exchange_rates where from_currency = @fromCurrency and to_currency = @toCurrency)
    begin
        raiserror ('exchange rate not found for the provided currencies.', 16, 1);
        return;
    end


	select rate 
	from currency_exchange_rates 
	where from_currency = @fromCurrency and to_currency = @toCurrency
end
GO
/****** Object:  StoredProcedure [dbo].[GetHashedPassword]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetHashedPassword]
    @id_user INT,
    @hashed_password VARCHAR(255) OUTPUT,
    @password_salt VARCHAR(32) OUTPUT
AS
BEGIN 
    SELECT @hashed_password = hashed_password, 
    @password_salt = password_salt
    FROM users
    WHERE id_user = @id_user;
END
GO
/****** Object:  StoredProcedure [dbo].[GetLoanById]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetLoanById]
    @id_loan INT
AS
BEGIN
    SELECT * FROM loans
    WHERE id_loan = @id_loan;
END
GO
/****** Object:  StoredProcedure [dbo].[GetLoansByUserId]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetLoansByUserId]
    @id_user INT
AS
BEGIN
    SELECT * FROM loans
    WHERE id_user = @id_user;
END
GO
/****** Object:  StoredProcedure [dbo].[GetTransactionsByType]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetTransactionsByType]
    @TransactionType NVARCHAR(50)
AS
BEGIN
    SELECT * 
    FROM transactions
    WHERE transaction_type = @TransactionType;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetUserBankAccounts]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetUserBankAccounts]
    @id_user int
AS
BEGIN
    SELECT * FROM bank_accounts WHERE id_user = @id_user ORDER BY date_created ASC;
END;
GO
/****** Object:  StoredProcedure [dbo].[GetUserByCnp]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetUserByCnp]
    @cnp varchar(13)
AS
BEGIN
    SELECT * FROM users
    WHERE cnp = @cnp;
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserByEmail]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetUserByEmail]
    @email varchar(100)
AS
BEGIN
    SELECT * FROM users
    WHERE email = @email;
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserById]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetUserById]
    @id_user INT
AS
BEGIN
    SELECT * FROM users
    WHERE id_user = @id_user;
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserByPhoneNumber]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetUserByPhoneNumber]
    @phone_number varchar(20)
AS
BEGIN
    SELECT * FROM users
    WHERE phone_number = @phone_number;
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserCredentials]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetUserCredentials]
	@email VARCHAR(100)
AS
BEGIN
	SELECT hashed_password, password_salt FROM users WHERE email=@email
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserInfoAfterLogin]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetUserInfoAfterLogin]
    @email NVARCHAR(100)
AS
BEGIN
    SELECT id_user, cnp, first_name, last_name, email, phone_number FROM Users WHERE email = @email
END
GO
/****** Object:  StoredProcedure [dbo].[RemoveBankAccount]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[RemoveBankAccount]
	@iban VARCHAR(100)
AS
BEGIN
	BEGIN TRANSACTION;
	-- First set referencing values to NULL form the transactions table
	UPDATE transactions
	SET sender_iban = NULL
	WHERE sender_iban = @iban;

	UPDATE transactions
	SET receiver_iban = NULL
	WHERE receiver_iban = @iban;

	DELETE FROM bank_accounts WHERE iban=@iban;
	COMMIT TRANSACTION;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateBankAccount]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[UpdateBankAccount]
	@iban VARCHAR(100),
	@custom_name VARCHAR(100),
	@daily_limit FLOAT,
	@max_per_transaction FLOAT,
	@max_nr_transactions_daily INT,
	@blocked BIT
AS
BEGIN
	UPDATE bank_accounts
	SET custom_name = @custom_name, 
	daily_limit = @daily_limit,
	max_per_transaction = @max_per_transaction,
	max_nr_transactions_daily = @max_nr_transactions_daily,
	blocked = @blocked
	WHERE iban = @iban
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateBankAccountBalance]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   procedure [dbo].[UpdateBankAccountBalance]
	@iban varchar(100),
	@amount float
as
begin
	if @iban is NULL OR len(@iban) <> 24
    begin
        RAISERROR ('Invalid IBAN. It must be exactly 24 characters long.', 16, 1);
        return;
    end

	if @amount < 0
    begin
        raiserror ('Invalid balance. Amount cannot be negative.', 16, 1);
        return;
    end

	if NOT EXISTS (select 1 from bank_accounts where iban = @iban)
    begin
        raiserror ('Bank account with the provided IBAN does not exist.', 16, 1);
        return;
    end

	update bank_accounts
	set amount = @amount
	where iban = @iban
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateLoan]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[UpdateLoan]
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
/****** Object:  StoredProcedure [dbo].[UpdateTransactionDescription]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[UpdateTransactionDescription]
    @TransactionID INT,
    @NewDescription VARCHAR(255)
AS
BEGIN
    UPDATE transactions
    SET transaction_description = @NewDescription
    WHERE transaction_id = @TransactionID;
END;
GO
/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 3/23/2025 11:29:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[UpdateUser]
    @id_user INT,
    @cnp VARCHAR(13),
    @first_name VARCHAR(50),
    @last_name VARCHAR(50),
    @email VARCHAR(100),
    @phone_number VARCHAR(20),
    @hashed_password VARCHAR(255),
    @password_salt VARCHAR(32)
AS
BEGIN
    UPDATE Users
    SET
        cnp = @cnp,
        first_name = @first_name,
        last_name = @last_name,
        email = @email,
        phone_number = @phone_number,
        hashed_password = @hashed_password,
        password_salt = @password_salt
    WHERE
        id_user = @id_user
END
GO
USE [master]
GO
ALTER DATABASE [loan_shark] SET  READ_WRITE 
GO
