using Domain.ShopItemAggregate.ValueObjects;

namespace Domain.ShopItemAggregate;

public sealed class ShopItem : AggregateRoot<ShopItemId>
{
    public string Name { get; private set; } = default!;
    public decimal Quantity { get; private set; } = default!;
    public Units Units { get; private set; } = default!;

    private ShopItem(ShopItemId id, string name, decimal quantity, Units units) : base(id)
    {
        Name = name;
        Quantity = quantity;
        Units = units;
    }

    private ShopItem()
    {
    }

    public static ShopItem CreateNew(string name, decimal quantity, Units units) =>
        new(ShopItemId.CreateUnique(), name, quantity, units);
    public static ShopItem Create(ShopItemId id, string name, decimal quantity, Units units) 
        => new(id, name, quantity, units);
}