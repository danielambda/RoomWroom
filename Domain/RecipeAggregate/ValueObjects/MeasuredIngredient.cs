using Domain.IngredientsAggregate.ValueObjects;

namespace Domain.RecipeAggregate.ValueObjects;

public class MeasuredIngredient : ValueObjectBase
{
    public IngredientId IngredientId { get; }
    public decimal Amount { get; }
    public Units Units { get; }

    public MeasuredIngredient(IngredientId ingredientId, decimal amount, Units units)
    {
        IngredientId = ingredientId;
        Amount = amount;
        Units = units;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return IngredientId;
        yield return Amount;
        yield return Units;
    }
}