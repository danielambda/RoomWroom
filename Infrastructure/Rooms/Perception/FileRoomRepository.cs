using System.Collections.Concurrent;
using System.Text.Json;
using Application.Rooms.Interfaces;
using Domain.Common.Enums;
using Domain.Common.ValueObjects;
using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Infrastructure.Rooms.Perception;

public class FileRoomRepository : IRoomRepository
{
    private const string SHOP_ITEMS_FILE = "Rooms.json";
    
    private static readonly ConcurrentDictionary<string, Room> Rooms = InitRooms();
    
    public Task<Room?> GetAsync(RoomId id, CancellationToken cancellationToken = default)=> 
        Task.FromResult(Rooms.GetValueOrDefault(id!));

    public Task AddShopItemToRoomAsync(OwnedShopItem shopItem, RoomId roomId,
        CancellationToken cancellationToken = default)
    {
        Rooms[roomId!].AddOwnedShopItem(shopItem);
        UpdateShopItemsFile();

        return Task.CompletedTask;
    }

    public Task AddAsync(Room room, CancellationToken cancellationToken = default)
    {
        Rooms.TryAdd(room.Id!, room);
        UpdateShopItemsFile();
        
        return Task.CompletedTask;
    }

    private static ConcurrentDictionary<string, Room> InitRooms()
    {
        using FileStream stream = new(SHOP_ITEMS_FILE, FileMode.OpenOrCreate, FileAccess.Read);
        Span<byte> buffer = stackalloc byte[(int)stream.Length];
        _ = stream.Read(buffer);

        if (buffer.Length == 0)
            return [];

        Utf8JsonReader jsonReader = new(buffer);
        JsonElement jsonElement = JsonElement.ParseValue(ref jsonReader);

        ConcurrentDictionary<string, Room>? shopItems = jsonElement.Deserialize();
        
        return shopItems ?? [];
    }
    
    private static void UpdateShopItemsFile()
    {
        using FileStream stream = new(SHOP_ITEMS_FILE, FileMode.OpenOrCreate, FileAccess.Write);
        string json = Rooms.Serialize();
        Span<byte> buffer = System.Text.Encoding.UTF8.GetBytes(json);

        stream.Write(buffer);
    }
}

file static class SerializationExtensions
{
    public static ConcurrentDictionary<string, Room>? Deserialize(this JsonElement jsonElement)
    {
        if (jsonElement.Deserialize<Dictionary<string, RoomDto>>() is not { } roomDtos)
            return null;

        return new(roomDtos.Select(pair => new KeyValuePair<string, Room>(
            pair.Key,
            Room.Create(
                pair.Value.Id!,
                pair.Value.Name,
                new Money(pair.Value.BudgetAmount, Enum.Parse<Currency>(pair.Value.BudgetCurrency)),
                pair.Value.UserIds.Select(id => UserId.Create(Guid.Parse(id))),
                pair.Value.OwnedShopItemDtos.Select(item =>
                    OwnedShopItem.Create(item.ShopItemId!, item.Quantity))))));
    }

    public static string Serialize(this ConcurrentDictionary<string, Room> rooms) =>
        JsonSerializer.Serialize(rooms.Select(pair =>
            new KeyValuePair<string, RoomDto>(
                pair.Key,
                new RoomDto(
                    pair.Value.Id!,
                    pair.Value.Name,
                    pair.Value.Budget.Amount,
                    pair.Value.Budget.Currency.ToString(),
                    pair.Value.UserIds.Select(id => id.Value.ToString()),
                    pair.Value.OwnedShopItems.Select(item =>
                        new OwnedShopItemDto(item.ShopItemId!, item.Quantity)))))
            .ToDictionary());

    private record RoomDto(
        string Id,
        string Name,
        decimal BudgetAmount,
        string BudgetCurrency,
        IEnumerable<string> UserIds,
        IEnumerable<OwnedShopItemDto> OwnedShopItemDtos);
    
    private record OwnedShopItemDto(string ShopItemId, decimal Quantity);
}
