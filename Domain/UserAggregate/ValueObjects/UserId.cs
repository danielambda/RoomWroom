namespace Domain.UserAggregate.ValueObjects;

public class UserId : ValueObjectBase, IId<UserId, Guid>
{
    public Guid Value { get; }

    private UserId(Guid value) => Value = value;

    public static UserId Create(Guid value) => new(value);
    
    public static UserId CreateUnique() => new(Guid.NewGuid());

    public static implicit operator string?(UserId id) => id.Value.ToString();

    public static implicit operator UserId(string str) => new(Guid.Parse(str));

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}