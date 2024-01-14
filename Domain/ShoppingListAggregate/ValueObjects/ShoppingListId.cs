namespace Domain.ShoppingListAggregate.ValueObjects;

public class ShoppingListId : ValueObject
{
    public Guid Value { get; }

    private ShoppingListId(Guid value) => Value = value;

    public static ShoppingListId Create(Guid value) => new(value);
    
    public static ShoppingListId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}