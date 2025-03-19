CREATE OR ALTER PROCEDURE GetAllCurrencyExchanges
AS
BEGIN
    SELECT * FROM currency_exchange_rates;
END

