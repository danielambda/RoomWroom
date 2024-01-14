namespace Domain.RoomAggregate.ValueObjects;

public class RoomId : ValueObject
{
    public Guid Value { get; }

    private RoomId(Guid value) => Value = value;

    public static RoomId Create(Guid value) => new(value);
    
    public static RoomId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}