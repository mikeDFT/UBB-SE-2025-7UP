create procedure GetExchangeRate
	@fromCurrency nvarchar(5),
	@toCurrency nvarchar(5)
as
begin
	select rate from currency_exchange_rates where from_currency = @fromCurrency and to_currency = @toCurrency
end
