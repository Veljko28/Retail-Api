CREATE PROCEDURE [dbo].[GetSaleByDate]
	@SaleDate nvarchar(50)
AS
begin
	SELECT * FROM Sale WHERE SUBSTRING(CONVERT(VARCHAR(25), SaleDate, 126),0,11) = @SaleDate;
end
