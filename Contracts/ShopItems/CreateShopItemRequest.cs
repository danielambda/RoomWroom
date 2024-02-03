namespace Contracts.ShopItems;

public record CreateShopItemRequest(string Name, decimal Quantity, string Units);