CREATE PROCEDURE [dbo].[AddUser]
	@Id nvarchar(128),
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Password nvarchar(50),
	@EmailAddress nvarchar(256),
	@DateCreated datetime2(7)
AS
begin
	INSERT INTO [dbo].[User] ( Id, FirstName, LastName, [Password], EmailAddress, DateCreated) 
	VALUES (@Id, @FirstName, @LastName, @Password, @EmailAddress, @DateCreated);
end