using Domain.ShopItemAggregate.ValueObjects;

namespace Domain.RoomAggregate.ValueObjects;

public class OwnedShopItem : ValueObjectBase
{
    public ShopItemId ShopItemId { get; }
    public decimal Quantity { get; }
    
    public OwnedShopItem(ShopItemId shopItemId, decimal quantity)
    {
        ShopItemId = shopItemId;
        Quantity = quantity;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ShopItemId;
        yield return Quantity;
    }
}