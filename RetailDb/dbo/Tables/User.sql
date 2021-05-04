CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] NVARCHAR(50) NOT NULL,
    [LastName] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(100) NOT NULL, 
    [EmailAddress] NVARCHAR(256) NOT NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT getutcdate()

)
