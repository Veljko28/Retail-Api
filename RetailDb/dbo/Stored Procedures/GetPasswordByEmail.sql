CREATE PROCEDURE [dbo].[GetPasswordByEmail]
	@EmailAddress nvarchar(256)
AS
begin
	SELECT [Password], Id FROM [dbo].[User] WHERE EmailAddress = @EmailAddress;
end
	