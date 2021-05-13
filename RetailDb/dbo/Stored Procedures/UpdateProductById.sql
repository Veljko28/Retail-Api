CREATE PROCEDURE [dbo].[UpdateProductById]
	@Id int,
	@ProductName nvarchar(100),
	@Description nvarchar(MAX),
	@RetailPrice money,
	@CreateDate datetime2(7),
	@LastModified datetime2(7)
AS
begin
	UPDATE [dbo].[Product] SET ProductName = @ProductName, [Description] = @Description,
	RetailPrice = @RetailPrice,  CreateDate = @CreateDate, LastModified = @LastModified WHERE Id = @Id
end
