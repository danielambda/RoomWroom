namespace Contracts.Rooms;

public record CreateRoomRequest(
    string Name,
    decimal BudgetAmount,
    string BudgetCurrency,
    IEnumerable<string> UserIds,
    IEnumerable<OwnedShopItemRequest> OwnedShopItems);

public record OwnedShopItemRequest(string ShopItemId, decimal Quantity);