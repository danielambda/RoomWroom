namespace Domain.ReceiptAggregate.ValueObjects;

public class ReceiptId : ValueObjectBase, IId<ReceiptId, Guid>
{
    public Guid Value { get; }
    
    private ReceiptId(Guid value) => Value = value;

    public static ReceiptId Create(Guid value) => new(value);
    
    public static ReceiptId CreateNew() => new(Guid.NewGuid());

    public static implicit operator string?(ReceiptId? id) => id?.Value.ToString();

    public static implicit operator ReceiptId?(string? str) => str is null ? null : new(Guid.Parse(str));

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}