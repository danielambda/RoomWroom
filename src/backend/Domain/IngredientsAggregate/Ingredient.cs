using Domain.IngredientsAggregate.ValueObjects;

namespace Domain.IngredientsAggregate;

public class Ingredient : AggregateRoot<IngredientId>
{
    public string Name { get; }

    private Ingredient(IngredientId id, string name) : base(id)
    {
        Name = name;
    }

    public static Ingredient Create(IngredientId id, string name) => new(id, name);
    public static Ingredient CreateNew(string name) => new(IngredientId.CreateNew(), name);
}