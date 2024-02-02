using Domain.ShopItemAggregate.ValueObjects;

namespace Domain.RoomAggregate.ValueObjects;

public class OwnedShopItem : ValueObjectBase
{
    public ShopItemId ShopItemId { get; }
    public decimal Quantity { get; }
    
    private OwnedShopItem(ShopItemId shopItemId, decimal quantity)
    {
        ShopItemId = shopItemId;
        Quantity = quantity;
    }

    public static OwnedShopItem Create(ShopItemId shopItemId, decimal quantity)
        => new(shopItemId, quantity);
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ShopItemId;
        yield return Quantity;
    }
}