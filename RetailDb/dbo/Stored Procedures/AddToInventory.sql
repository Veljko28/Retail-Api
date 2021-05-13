CREATE PROCEDURE [dbo].[AddToInventory]
	@ProductId int,
	@Quantity int,
	@PurchasePrice money,
	@PurchaseDate datetime2(7)
	AS
begin
	INSERT INTO Inventory (ProductId,Quantity,PurchasePrice,PurchaseDate)
	VALUES (@ProductId,@Quantity,@PurchasePrice,@PurchaseDate);
end