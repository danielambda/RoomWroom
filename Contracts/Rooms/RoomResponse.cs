namespace Contracts.Rooms;


public record RoomResponse(
    string Id,
    string Name,
    IEnumerable<string> UserIds,
    IEnumerable<OwnedShopItemResponse> OwnedShopItems);

public record OwnedShopItemResponse(string ShopItemId, decimal Quantity);