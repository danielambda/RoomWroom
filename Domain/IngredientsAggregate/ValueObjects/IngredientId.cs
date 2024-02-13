namespace Domain.IngredientsAggregate.ValueObjects;

public class IngredientId : ValueObjectBase, IId<IngredientId, Guid>
{
    public Guid Value { get; }

    private IngredientId(Guid value) => Value = value;

    public static IngredientId Create(Guid value) => new(value);

    public static IngredientId CreateUnique() => new(Guid.NewGuid());

    public static implicit operator string?(IngredientId? id) => id?.ToString();

    public static implicit operator IngredientId?(string? str) => str is null ? null : new(Guid.Parse(str));
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}