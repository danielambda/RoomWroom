namespace Domain.ShopItemAggregate.ValueObjects;

public class ShopItemId : ValueObject
{
    public Guid Value { get; }

    private ShopItemId(Guid value) => Value = value;

    public static ShopItemId Create(Guid value) => new(value);
    
    public static ShopItemId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}