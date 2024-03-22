namespace Contracts.ShopItems;

public record ShopItemResponse
(
    string Id,
    string Name,
    decimal Quantity,
    string Units,
    string IngredientId
);