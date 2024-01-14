using Domain.ShopItemAggregate.ValueObjects;

namespace Domain.ShopItemAggregate;

public class ShopItem : AggregateRoot<ShopItemId>
{
    public string Name { get; }
    
    protected ShopItem(ShopItemId id, string name) : base(id)
    {
        Name = name;
    }
}