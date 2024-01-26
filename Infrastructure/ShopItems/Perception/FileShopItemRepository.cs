using System.Collections.Concurrent;
using System.Text.Json;
using Application.Common.Interfaces;
using Domain.ShopItemAggregate;
using Domain.ShopItemAggregate.ValueObjects;

namespace Infrastructure.ShopItems.Perception;

public class FileShopItemRepository : IShopItemRepository
{   
    private const string SHOP_ITEMS_FILE = "ShopItems.json";
    
    private static readonly ConcurrentDictionary<string, ShopItem> ShopItems = InitShopItems();
    
    public Task<ShopItem?> GetAsync(ShopItemId id, CancellationToken cancellationToken = default)=> 
        Task.FromResult(ShopItems.GetValueOrDefault(id!));

    public Task AddAsync(ShopItem shopItem, CancellationToken cancellationToken = default)
    {
        ShopItems.TryAdd(shopItem.Id!, shopItem);
        UpdateShopItemsFile();
        
        return Task.CompletedTask;
    }

    public Task AddAsync(IEnumerable<ShopItem> shopItems, CancellationToken cancellationToken = default)
    {
        foreach (ShopItem shopItem in shopItems) 
            ShopItems.TryAdd(shopItem.Id!, shopItem);
        UpdateShopItemsFile();
        
        return Task.CompletedTask;
    }

    private static ConcurrentDictionary<string, ShopItem> InitShopItems()
    {
        using FileStream stream = new(SHOP_ITEMS_FILE, FileMode.OpenOrCreate, FileAccess.Read);
        Span<byte> buffer = stackalloc byte[(int)stream.Length];
        _ = stream.Read(buffer);

        if (buffer.Length == 0)
            return [];

        Utf8JsonReader jsonReader = new(buffer);
        JsonElement jsonElement = JsonElement.ParseValue(ref jsonReader);

        ConcurrentDictionary<string, ShopItem>? shopItems = jsonElement.Deserialize();
        
        return shopItems ?? [];
    }
    
    private static void UpdateShopItemsFile()
    {
        using FileStream stream = new(SHOP_ITEMS_FILE, FileMode.OpenOrCreate, FileAccess.Write);
        string json = ShopItems.Serialize();
        Span<byte> buffer = System.Text.Encoding.UTF8.GetBytes(json);

        stream.Write(buffer);
    }
}

file static class SerializationExtensions
{
    public static ConcurrentDictionary<string, ShopItem>? Deserialize(this JsonElement jsonElement)
    {
        if(jsonElement.Deserialize<Dictionary<string, ShopItemDto>>() is not { } shopDtos)
            return null;
        
        return new(shopDtos.Select(pair =>
            new KeyValuePair<string, ShopItem>(
                pair.Key,
                ShopItem.Create(
                    pair.Value.Id!,
                    pair.Value.Name))));
    }

    public static string Serialize(this ConcurrentDictionary<string, ShopItem> shopItems) =>
        JsonSerializer.Serialize(shopItems.Select(pair =>
            new KeyValuePair<string, ShopItemDto>(
                pair.Key,
                new ShopItemDto(
                    pair.Value.Id!,
                    pair.Value.Name)))
            .ToDictionary());

    private record ShopItemDto(string Id, string Name);
}