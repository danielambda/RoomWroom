namespace Domain.Common.Models;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public static bool operator ==(ValueObject? left, ValueObject? right) => Equals(left, right);

    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);

    protected abstract IEnumerable<object?> GetEqualityComponents();

    public bool Equals(ValueObject? other) => Equals((object?)other);

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        ValueObject other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x is null ? 0 : x.GetHashCode())
            .Aggregate((x, y) => x ^ y);
    }
}