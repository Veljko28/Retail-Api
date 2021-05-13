CREATE PROCEDURE [dbo].[AddUser]
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Password nvarchar(50),
	@EmailAddress nvarchar(256),
	@DateCreated datetime2(7)
AS
begin
	INSERT INTO [dbo].[User] ( FirstName, LastName, [Password], EmailAddress, DateCreated) 
	VALUES (@FirstName, @LastName, @Password, @EmailAddress, @DateCreated);
end