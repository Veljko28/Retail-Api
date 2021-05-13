CREATE PROCEDURE [dbo].[FindRefreshToken]
	@Token nvarchar(256)
AS
begin
	SET NOCOUNT ON;

	SELECT * FROM RefreshTokens WHERE @Token = Token;
end
