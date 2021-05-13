CREATE TABLE [dbo].[RefreshTokens]
(
	[Token] NVARCHAR(256) NOT NULL PRIMARY KEY, 
    [JwtId] NVARCHAR(MAX) NOT NULL, 
    [CreatedDate] DATETIME2 NOT NULL, 
    [Expires] DATETIME2 NOT NULL, 
    [Used] BIT NOT NULL, 
    [Invalidated] BIT NOT NULL, 
    [UserId] NVARCHAR(MAX) NOT NULL
)
