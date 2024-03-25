namespace Contracts.Rooms;


public record RoomResponse
(
    string Id,
    string Name,
    decimal BudgetAmount,
    string BudgetCurrency,
    decimal BudgetLowerBound,
    bool MoneyRoundingRequired,
    List<string> UserIds,
    List<OwnedShopItemResponse> OwnedShopItems
);

public record OwnedShopItemResponse
(
    string ShopItemId,
    decimal Quantity,
    decimal PriceAmount,
    string PriceCurrency
);