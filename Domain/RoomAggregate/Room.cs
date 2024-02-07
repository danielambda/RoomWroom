using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.RoomAggregate;

public sealed class Room : AggregateRoot<RoomId>
{
    public string Name { get; }
    public Money Budget { get; }
    public IReadOnlyList<UserId> UserIds => _userIds.AsReadOnly();
    public IReadOnlyList<OwnedShopItem> OwnedShopItems => _ownedShopItems.AsReadOnly();

    private readonly List<UserId> _userIds;
    
    private readonly List<OwnedShopItem> _ownedShopItems;

    private Room(RoomId id, string name, Money budget,
        IEnumerable<UserId> userIds, IEnumerable<OwnedShopItem> ownedShopItems)
        : base(id)
    {
        Name = name;
        Budget = budget;
        
        _userIds = userIds.ToList();
        _ownedShopItems = ownedShopItems.ToList();
    }

    public static Room Create(
        RoomId id, string name, Money budget,
        IEnumerable<UserId> userIds, IEnumerable<OwnedShopItem> ownedShopItems)
        => new(id, name, budget, userIds, ownedShopItems);

    public static Room CreateNew(
        string name, Money budget, IEnumerable<UserId> userIds, IEnumerable<OwnedShopItem> ownedShopItems) =>
        new(RoomId.CreateUnique(), name, budget, userIds, ownedShopItems);

    public void AddOwnedShopItem(OwnedShopItem shopItem) => _ownedShopItems.Add(shopItem);
    public void AddOwnedShopItems(IEnumerable<OwnedShopItem> shopItems) => _ownedShopItems.AddRange(shopItems);
    
    public void AddUser(UserId userId) => _userIds.Add(userId);

    public bool RemoveUser(UserId userId) => _userIds.Remove(userId);
}