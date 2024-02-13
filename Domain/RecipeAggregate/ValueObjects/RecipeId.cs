namespace Domain.RecipeAggregate.ValueObjects;

public class RecipeId : ValueObjectBase, IId<RecipeId, Guid>
{
    public Guid Value { get; }

    private RecipeId(Guid value) => Value = value;

    public static RecipeId Create(Guid value) => new(value);

    public static RecipeId CreateUnique() => new(Guid.NewGuid());

    public static implicit operator string?(RecipeId? id) => id?.ToString();

    public static implicit operator RecipeId?(string? str) => str is null ? null : new(Guid.Parse(str));
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}