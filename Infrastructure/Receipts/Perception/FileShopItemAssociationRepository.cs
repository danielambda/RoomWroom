﻿using System.Collections.Concurrent;
using System.Text.Json;
using Application.Common.Interfaces.Perception;
using Domain.ReceiptAggregate.ValueObjects;

namespace Infrastructure.Receipts.Perception;

public class FileShopItemAssociationRepository : IShopItemAssociationsRepository
{
    private const string SHOP_ITEM_ASSOCIATIONS_FILE = "ShopItemAssociations.json";
    
    private static readonly ConcurrentDictionary<string, ShopItemAssociation> Associations = InitAssociations();

    public Task<ShopItemAssociation?> GetAsync(string originalName, CancellationToken cancellationToken) => 
        Task.FromResult(Associations.GetValueOrDefault(originalName));

    public Task AddOrUpdateAsync(ShopItemAssociation association, CancellationToken cancellationToken = default)
    {
        Associations.AddOrUpdate(association.OriginalName,
            _ =>association,
            (_, _) =>association);
        UpdateReceiptsFile();

        return Task.CompletedTask;
    }

    private static ConcurrentDictionary<string, ShopItemAssociation> InitAssociations()
    {
        using FileStream stream = new(SHOP_ITEM_ASSOCIATIONS_FILE, FileMode.OpenOrCreate, FileAccess.Read);
        Span<byte> buffer = stackalloc byte[(int)stream.Length];
        _ = stream.Read(buffer);

        if (buffer.Length == 0)
            return [];

        Utf8JsonReader jsonReader = new(buffer);
        JsonElement jsonElement = JsonElement.ParseValue(ref jsonReader);

        ConcurrentDictionary<string, ShopItemAssociation>? associations = jsonElement.Deserialize();
        
        return associations ?? [];
    }
    
    private static void UpdateReceiptsFile()
    {
        using FileStream stream = new(SHOP_ITEM_ASSOCIATIONS_FILE, FileMode.OpenOrCreate, FileAccess.Write);
        string json = Associations.Serialize();
        Span<byte> buffer = System.Text.Encoding.UTF8.GetBytes(json);

        stream.Write(buffer);
    }
}

file static class SerializationExtensions
{
    public static ConcurrentDictionary<string, ShopItemAssociation>? Deserialize(this JsonElement jsonElement) => 
        jsonElement.Deserialize<ConcurrentDictionary<string, ShopItemAssociation>>();

    public static string Serialize(this ConcurrentDictionary<string, ShopItemAssociation> associations) => 
        JsonSerializer.Serialize(associations);
}