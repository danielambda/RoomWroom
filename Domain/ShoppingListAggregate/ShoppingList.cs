using Domain.ShoppingListAggregate.ValueObjects;

namespace Domain.ShoppingListAggregate;

public sealed  class ShoppingList : AggregateRoot<ShoppingListId>
{
    private readonly List<ShoppingListItem> _list;

    public IReadOnlyList<ShoppingListItem> List => _list.AsReadOnly();
    
    private ShoppingList(ShoppingListId id, List<ShoppingListItem> list) : base(id)
    {
        _list = list;
    }

    public void Add(ShoppingListItem item) => _list.Add(item);
    
    public static ShoppingList CreateNew(List<ShoppingListItem> list) => new(ShoppingListId.CreateUnique(), list);
}