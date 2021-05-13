CREATE PROCEDURE [dbo].[DeleteProduct]
	@Id int
AS
begin
	DELETE FROM [dbo].[Product] WHERE Id = @Id;
end
