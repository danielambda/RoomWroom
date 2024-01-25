namespace Domain.ShopItemAggregate.ValueObjects;

public class ShopItemId : ValueObjectBase, IId<ShopItemId, Guid>
{
    public Guid Value { get; }

    private ShopItemId(Guid value) => Value = value;

    public static ShopItemId Create(Guid value) => new(value);
    
    public static ShopItemId CreateUnique() => new(Guid.NewGuid());
    
    public static implicit operator string?(ShopItemId? id) => id?.Value.ToString();

    public static implicit operator ShopItemId?(string? str) => str is null ? null : new(Guid.Parse(str));

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}