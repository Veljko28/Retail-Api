CREATE PROCEDURE [dbo].[GetUser]
	@EmailAddress nvarchar(256),
	@Password nvarchar(50)
AS
begin
	SELECT * FROM [dbo].[User] WHERE EmailAddress = @EmailAddress AND [Password] = @Password;
end
