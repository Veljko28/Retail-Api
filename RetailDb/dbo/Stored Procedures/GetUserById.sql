CREATE PROCEDURE [dbo].[GetUserById]
	@Id int
AS
begin
	SELECT * FROM [dbo].[User] WHERE Id = @Id;
end