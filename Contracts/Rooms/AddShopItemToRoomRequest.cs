namespace Contracts.Rooms;

public record AddShopItemToRoomRequest(
    string ShopItemId,
    decimal Quantity,
    decimal? PriceAmount,
    string? PriceCurrency);