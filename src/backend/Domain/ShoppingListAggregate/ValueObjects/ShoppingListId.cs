namespace Domain.ShoppingListAggregate.ValueObjects;

public class ShoppingListId : ValueObjectBase, IId<ShoppingListId, Guid>
{
    public Guid Value { get; }

    private ShoppingListId(Guid value) => Value = value;

    public static ShoppingListId Create(Guid value) => new(value);
    
    public static ShoppingListId CreateNew() => new(Guid.NewGuid());
    
    public static implicit operator string?(ShoppingListId? id) => id?.Value.ToString();

    public static implicit operator ShoppingListId?(string? str) => str is null ? null : new(Guid.Parse(str));

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}