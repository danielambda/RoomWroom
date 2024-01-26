using Domain.ShopItemAggregate.ValueObjects;

namespace Domain.ShopItemAggregate;

public class ShopItem : AggregateRoot<ShopItemId>
{
    public string Name { get; private set; } = null!;

    private ShopItem(ShopItemId id, string name) : base(id)
    {
        Name = name;
    }

    private ShopItem()
    {
    }

    public static ShopItem CreateNew(string name) => new(ShopItemId.CreateUnique(), name);
    public static ShopItem Create(ShopItemId id, string name) => new(id, name);
}