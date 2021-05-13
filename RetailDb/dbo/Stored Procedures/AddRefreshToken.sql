CREATE PROCEDURE [dbo].[AddRefreshToken]
	@Token nvarchar(256),
	@JwtId nvarchar(MAX),
	@CreatedDate datetime2(7),
	@Expires datetime2(7),
	@Used bit,
	@Invalidated bit,
	@UserId int
AS
begin
	INSERT INTO [dbo].[RefreshTokens] (Token, JwtId, CreatedDate, Expires,Used, Invalidated, UserId)
	 VALUES (@Token, @JwtId, @CreatedDate, @Expires, @Used, @Invalidated, @UserId);
end