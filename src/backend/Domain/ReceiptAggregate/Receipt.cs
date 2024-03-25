using Domain.ReceiptAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.ReceiptAggregate;

public sealed class Receipt : AggregateRoot<ReceiptId>
{
    private readonly List<ReceiptItem> _items = default!;

    public IReadOnlyList<ReceiptItem> Items => _items.AsReadOnly();

    public string? Qr { get; private set; } = default!;
    public UserId CreatorId { get; private set; } = default!;

    public Money Sum => _items
        .Select(item => item.Sum)
        .Aggregate((s1, s2) => s1 + s2);

    public Currency Currency => _items.First().Price.Currency;

    private Receipt(ReceiptId id, List<ReceiptItem> items, string? qr, UserId creatorId) : base(id)
    {
        _items = items;
        Qr = qr;
        CreatorId = creatorId;
    }

    public static Receipt CreateNew(IEnumerable<ReceiptItem> items, string? qr, UserId creatorId)
    {
        var itemsList = items
            .OrderBy(item => item.Name)
            .CombineSame()
            .Where(item => item is { Quantity: > 0, Price.Amount: > 0 })
            .ToList();
        
        return new(ReceiptId.CreateNew(), itemsList, qr, creatorId);
    }

    public static Receipt Create(ReceiptId id, IEnumerable<ReceiptItem> items, string? qr, UserId creatorId) =>
        new(id, items.ToList(), qr, creatorId);

    public void AssociateShopItemIdAtIndex(ShopItemId shopItemId, int index) =>
        _items[index].AssociateWith(shopItemId);

    public void AssociateShopItemIdsByIndices(IEnumerable<ShopItemId?> associatedShopItemIds)
    {
        var currentIndex = 0;
        foreach (ShopItemId? associatedId in associatedShopItemIds)
        {
            if (associatedId != null) 
                _items[currentIndex].AssociateWith(associatedId);
            
            currentIndex++;
        }
    }

    public async Task AssociateShopItemIdsAsync
    (
        Func<string, CancellationToken,
        Task<ShopItemAssociation?>> associationFunc,
        CancellationToken cancellationToken = default
    )
    {
        foreach (ReceiptItem receiptItem in _items)
        {
            ShopItemAssociation? association = await associationFunc(receiptItem.Name, cancellationToken);
            
            if (association is not null)
                receiptItem.AssociateWith(association.ShopItemId);
        }
    }

    private Receipt()
    {
    }
}

file static class ReceiptItemExtensions
{
    public static IEnumerable<ReceiptItem> CombineSame(this IOrderedEnumerable<ReceiptItem> items)
    {
        ReceiptItem? previousItem = null;

        foreach (ReceiptItem currentItem in items)
        {
            if (currentItem.Price == previousItem?.Price && currentItem.Name == previousItem.Name)
            {
                decimal quantitySum = previousItem.Quantity + currentItem.Quantity;
                previousItem = new(currentItem.Name, currentItem.Price, quantitySum);
                
                continue;
            }
            
            if (previousItem is not null)
                yield return previousItem;

            previousItem = currentItem;
        }

        if (previousItem is not null)
            yield return previousItem;
    }
}