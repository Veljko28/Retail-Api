CREATE TABLE [dbo].[RefreshTokens]
(
    [JwtId] NVARCHAR(40) NOT NULL PRIMARY KEY, 
	[Token] NVARCHAR(256) NOT NULL , 
    [CreatedDate] DATETIME2 NOT NULL, 
    [Expires] DATETIME2 NOT NULL, 
    [Used] BIT NOT NULL, 
    [Invalidated] BIT NOT NULL, 
    [UserId] NVARCHAR(MAX) NOT NULL
)
