using Domain.ReceiptAggregate.ValueObjects;

namespace Domain.ReceiptAggregate;

public class Receipt : AggregateRoot<ReceiptId>
{
    private readonly List<ReceiptItem> _items;

    public IReadOnlyList<ReceiptItem> Items => _items.AsReadOnly();
    
    public string? Qr { get; }

    private Receipt(ReceiptId id, IEnumerable<ReceiptItem> items, string? qr) : base(id)
    {
        _items = items
            .OrderBy(item => item.Name)
            .CombineSame()
            .Where(item => item is { Quantity: > 0, Price.Amount: > 0 })
            .ToList();
        
        Qr = qr;
    }   

    public static Receipt CreateNew(IEnumerable<ReceiptItem> items, string? qr = null) => 
        new(ReceiptId.CreateUnique(), items, qr);
}

file static class GroceryItemExtensions
{
    public static IEnumerable<ReceiptItem> CombineSame(this IOrderedEnumerable<ReceiptItem> items)
    {
        ReceiptItem? previousItem = null;

        foreach (ReceiptItem currentItem in items)
        {
            if (currentItem.Price == previousItem?.Price && currentItem.Name == previousItem.Name)
            {
                float quantitySum = previousItem.Quantity + currentItem.Quantity;
                previousItem = new ReceiptItem(currentItem.Name, currentItem.Price, quantitySum);
                
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