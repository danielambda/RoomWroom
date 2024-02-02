namespace Contracts.Rooms;


public record RoomResponse(
    string Id,
    string Name,
    decimal BudgetAmount,
    string BudgetCurrency,
    IEnumerable<string> UserIds,
    IEnumerable<OwnedShopItemResponse> OwnedShopItems);

public record OwnedShopItemResponse(string ShopItemId, decimal Quantity);