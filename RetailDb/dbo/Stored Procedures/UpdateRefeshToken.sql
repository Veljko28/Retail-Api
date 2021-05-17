CREATE PROCEDURE [dbo].[UpdateRefeshToken]
	@Token nvarchar(256),
	@JwtId nvarchar(MAX),
	@CreatedDate datetime2(7),
	@Expires datetime2(7),
	@Used bit,
	@Invalidated bit,
	@UserId nvarchar(128)
AS
begin
	UPDATE [dbo].[RefreshTokens] SET JwtId = @JwtId, CreatedDate = @CreatedDate, Expires = @Expires, Used = @Used,
	Invalidated = @Invalidated, UserId = @UserId WHERE Token = @Token;
end
