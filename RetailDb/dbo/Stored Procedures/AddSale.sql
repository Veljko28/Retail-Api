CREATE PROCEDURE [dbo].[AddSale]
	@CashierId int,
	@SaleDate datetime2(7),
	@SubTotal money,
	@Tax money,
	@Total money
AS
begin
	INSERT INTO Sale (CashierId, SaleDate, SubTotal, Tax, Total) 
	 VALUES (@CashierId, @SaleDate, @SubTotal, @Tax, @Total);
end
