namespace Domain.ReceiptAggregate.ValueObjects;

public class ReceiptId : ValueObject
{
    public Guid Value { get; }

    private ReceiptId(Guid value) => Value = value;

    public static ReceiptId Create(Guid value) => new(value);
    
    public static ReceiptId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}