create or alter procedure GetExchangeRate
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
