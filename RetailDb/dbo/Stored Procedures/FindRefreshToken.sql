CREATE PROCEDURE [dbo].[FindRefreshToken]
	@Token nvarchar(256)
AS
begin
	SELECT * FROM [dbo].[RefreshTokens] WHERE Token = @Token;
end
