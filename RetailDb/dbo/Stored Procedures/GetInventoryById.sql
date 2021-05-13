CREATE PROCEDURE [dbo].[GetInventoryById]
	@Id int
AS
begin
	SELECT * FROM Inventory WHERE Id = @Id;
end