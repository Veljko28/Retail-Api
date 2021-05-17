CREATE PROCEDURE [dbo].[GetUserById]
	@Id nvarchar(128)
AS
begin
	SELECT * FROM [dbo].[User] WHERE Id = @Id;
end