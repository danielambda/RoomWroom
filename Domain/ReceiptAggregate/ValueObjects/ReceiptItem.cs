using Domain.ShopItemAggregate.ValueObjects;

namespace Domain.ReceiptAggregate.ValueObjects;

public sealed class ReceiptItem : ValueObjectBase
{
    public string Name { get; }
    public Money Price { get; }
    public decimal Quantity { get; }
    public Money Sum { get; }
    public ShopItemId? AssociatedShopItemId { get; private set; } = default;

    public ReceiptItem(string name, Money price, decimal quantity, ShopItemId? associatedShopItemId = null)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        AssociatedShopItemId = associatedShopItemId;

        Sum = Price * Quantity;
    }

    internal void AssociateWith(ShopItemId shopItemId) => AssociatedShopItemId = shopItemId;
    
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