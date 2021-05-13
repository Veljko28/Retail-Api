CREATE PROCEDURE [dbo].[AddSaleDetail]
	@SaleId int,
	@ProductId int,
	@Quantity int,
	@PurchasePrice money,
	@Tax money
AS
begin
INSERT INTO SaleDetail (SaleId, ProductId, Quantity, PurchasePrice, Tax)
		 VALUES (@SaleId, @ProductId, @Quantity, @PurchasePrice, @Tax);
end
