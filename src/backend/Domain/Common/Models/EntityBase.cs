namespace Domain.Common.Models;

public abstract class EntityBase<TId> : IEquatable<EntityBase<TId>>
    where TId : notnull
{
    public TId Id { get; private set; } = default!;

    protected EntityBase(TId id) => Id = id;

    protected EntityBase() { }

    public bool Equals(EntityBase<TId>? other) => Equals((object?)other);

    public override bool Equals(object? obj) => obj is EntityBase<TId> entity && Id.Equals(entity.Id);

    public static bool operator ==(EntityBase<TId> left, EntityBase<TId> right) => Equals(left, right);
    
    public static bool operator !=(EntityBase<TId> left, EntityBase<TId> right) => !(left == right);

    public override int GetHashCode() => Id.GetHashCode();
}