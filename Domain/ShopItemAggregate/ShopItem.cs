using Domain.IngredientsAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;

namespace Domain.ShopItemAggregate;

public sealed class ShopItem : AggregateRoot<ShopItemId>
{
    public string Name { get; }
    public decimal Quantity { get; }
    public Units Units { get; }
    public IngredientId? IngredientId { get; }

    private ShopItem(ShopItemId id, string name, decimal quantity, Units units, IngredientId? ingredientId) : base(id)
    {
        Name = name;
        Quantity = quantity;
        Units = units;
        IngredientId = ingredientId;
    }

    public static ShopItem CreateNew(string name, decimal quantity, Units units, IngredientId? ingredientId = null) =>
        new(ShopItemId.CreateUnique(), name, quantity, units, ingredientId);

    public static ShopItem Create(ShopItemId id, string name, decimal quantity, Units units,
        IngredientId? ingredientId = null) =>
        new(id, name, quantity, units, ingredientId);
}