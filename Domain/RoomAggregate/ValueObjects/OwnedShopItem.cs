using Domain.ShopItemAggregate.ValueObjects;

namespace Domain.RoomAggregate.ValueObjects;

public class OwnedShopItem : ValueObjectBase
{
    public ShopItemId ShopItemId { get; }
    public decimal Quantity { get; }
    public Money Price { get; }
    
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
    }
}