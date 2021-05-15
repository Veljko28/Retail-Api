CREATE PROCEDURE [dbo].[GetSaleDetailById]
	@SaleId int
AS
begin
	SELECT * FROM SaleDetail WHERE SaleId = @SaleId;
end