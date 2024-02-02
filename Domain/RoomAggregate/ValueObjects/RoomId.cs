namespace Domain.RoomAggregate.ValueObjects;

public class RoomId : ValueObjectBase, IId<RoomId, Guid>
{
    public Guid Value { get; }

    private RoomId(Guid value) => Value = value;

    public static RoomId Create(Guid value) => new(value);
    
    public static RoomId CreateUnique() => new(Guid.NewGuid());
    
    public static implicit operator string?(RoomId? id) => id?.Value.ToString();

    public static implicit operator RoomId?(string? str) => str is null ? null : new(Guid.Parse(str));

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}