CREATE PROCEDURE [dbo].[AddProduct]
	@ProductName nvarchar(100),
	@Description nvarchar(MAX),
	@RetailPrice money,
	@CreateDate datetime2(7),
	@LastModified datetime2(7)
AS
begin
	INSERT INTO [dbo].[Product] (ProductName, [Description], RetailPrice, CreateDate, LastModified)
	 VALUES (@ProductName, @Description, @RetailPrice, @CreateDate, @LastModified);
end
