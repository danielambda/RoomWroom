using Domain.ShopItemAggregate.ValueObjects;

namespace Domain.RoomAggregate.ValueObjects;

public class OwnedShopItem : ValueObjectBase
{
    public ShopItemId ShopItemId { get; private set; } = default!;
    public decimal Quantity { get; private set; }
    public Money Price { get; private set; } = default!;
    public Money Sum => Quantity * Price;
    
    public OwnedShopItem(ShopItemId shopItemId, decimal quantity, Money price)
    {
        ShopItemId = shopItemId;
        Quantity = quantity;
        Price = price;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ShopItemId;
        yield return Quantity;
        yield return Price;
    }

    private OwnedShopItem()
    {
    }
}