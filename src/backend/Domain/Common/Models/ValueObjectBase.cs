namespace Domain.Common.Models;

public abstract class ValueObjectBase : IEquatable<ValueObjectBase>
{
    public static bool operator ==(ValueObjectBase? left, ValueObjectBase? right) => Equals(left, right);

    public static bool operator !=(ValueObjectBase? left, ValueObjectBase? right) => !(left == right);

    protected abstract IEnumerable<object?> GetEqualityComponents();

    public bool Equals(ValueObjectBase? other) => Equals((object?)other);

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObjectBase)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x is null ? 0 : x.GetHashCode())
            .Aggregate((x, y) => x ^ y);
    }
}