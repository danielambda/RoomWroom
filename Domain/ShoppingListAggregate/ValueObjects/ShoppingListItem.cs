using Domain.ShopItemAggregate.ValueObjects;

namespace Domain.ShoppingListAggregate.ValueObjects;

public class ShoppingListItem : ValueObject
{
    public ShopItemId ShopItemId { get; }
    public float Quantity { get; }
    public bool IsOptional { get; }
    
    public ShoppingListItem(ShopItemId shopItemId, float quantity, bool isOptional)// = false)
    {
        ShopItemId = shopItemId;
        Quantity = quantity;
        IsOptional = isOptional;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ShopItemId;
        yield return Quantity;
        yield return IsOptional;
    }
}