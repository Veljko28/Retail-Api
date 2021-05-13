CREATE PROCEDURE [dbo].[GetProductsById]
@Id int
AS
begin
	SELECT ProductName, [Description], RetailPrice, CreateDate, LastModified
	FROM [dbo].[Product] WHERE Id = @Id;
end
