namespace Domain.Common.Models;

public class AggregateRoot<TId> : EntityBase<TId> where TId : notnull
{
    protected AggregateRoot(TId id) : base(id) { }
    
    protected AggregateRoot() {}
}