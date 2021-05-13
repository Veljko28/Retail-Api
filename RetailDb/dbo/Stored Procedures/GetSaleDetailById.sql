CREATE PROCEDURE [dbo].[GetSaleDetailById]
	@Id int
AS
begin
	SELECT * FROM SaleDetail WHERE Id = @Id;
end