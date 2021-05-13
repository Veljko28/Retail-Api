CREATE PROCEDURE [dbo].[GetSaleById]
	@Id int
AS
begin
	SELECT * FROM Sale WHERE Id = @Id;
end