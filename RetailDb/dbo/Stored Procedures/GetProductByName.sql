CREATE PROCEDURE [dbo].[GetProductByName]
	@ProductName nvarchar(100)
AS
begin
	SELECT * FROM [dbo].[Product] WHERE ProductName LIKE @ProductName
end
