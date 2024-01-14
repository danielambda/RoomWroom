using Domain.ShopItemAggregate.ValueObjects;

namespace Domain.ReceiptAggregate.ValueObjects;

public class ReceiptItem : ValueObject
{
    public string Name { get; }
    public Money Price { get; }
    public float Quantity { get; }
    public Money Sum { get; }
    public ShopItemId? AssociatedShopItemId { get; }

    public ReceiptItem(string name, Money price, float quantity, ShopItemId? associatedShopItemId = null)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        AssociatedShopItemId = associatedShopItemId;

        Sum = Price * Quantity;
    }
    
    public override string ToString() => $"{Name}.{Environment.NewLine} {Price} x {Quantity} = {Sum}";
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Quantity;
        yield return Price;
        yield return Sum;
        yield return AssociatedShopItemId;
    }
}