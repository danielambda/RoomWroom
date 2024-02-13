using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.RoomAggregate;

public sealed class Room : AggregateRoot<RoomId>
{
    public string Name { get; }
    public Money Budget { get; private set; }
    public decimal BudgetLowerBound { get; }
    public bool MoneyRoundingRequired { get; }
    public IReadOnlyList<UserId> UserIds => _userIds.AsReadOnly();
    public IReadOnlyList<OwnedShopItem> OwnedShopItems => _ownedShopItems.AsReadOnly();

    private readonly List<UserId> _userIds;
    
    private readonly List<OwnedShopItem> _ownedShopItems;

    private Room(RoomId id, string name, Money budget, decimal budgetLowerBound, bool moneyRoundingRequired,
        IEnumerable<UserId> userIds, IEnumerable<OwnedShopItem> ownedShopItems) : base(id)
    {
        Name = name;
        Budget = budget;
        BudgetLowerBound = budgetLowerBound;
        MoneyRoundingRequired = moneyRoundingRequired;
        
        _userIds = userIds.ToList();
        _ownedShopItems = ownedShopItems.ToList();
    }

    public static Room Create(
        RoomId id, string name, Money budget, decimal budgetLowerBound, bool moneyRoundingRequired,
        IEnumerable<UserId> userIds, IEnumerable<OwnedShopItem> ownedShopItems) =>
        new(id, name, budget, budgetLowerBound, moneyRoundingRequired, userIds, ownedShopItems);

    public static Room CreateNew(
        string name, Money budget, decimal budgetLoverBound, bool moneyRoundingRequired,
        IEnumerable<UserId> userIds, IEnumerable<OwnedShopItem> ownedShopItems) =>
        new(RoomId.CreateUnique(), name, budget, budgetLoverBound, moneyRoundingRequired, userIds, ownedShopItems);

    public void AddOwnedShopItem(OwnedShopItem shopItem)
    {
        _ownedShopItems.Add(shopItem);

        Money sum = shopItem.Sum;
        
        if (MoneyRoundingRequired)
            sum = sum.ToRounded();
        
        SpendMoney(sum);
    }

    public void AddOwnedShopItems(IEnumerable<OwnedShopItem> shopItems)
    {
        var ownedShopItems = shopItems as OwnedShopItem[] ?? shopItems.ToArray();
        _ownedShopItems.AddRange(ownedShopItems);

        Money sum = ownedShopItems.Select(item => item.Sum).Aggregate((s1, s2) => s1 + s2);

        if (MoneyRoundingRequired)
            sum = sum.ToRounded();
        
        SpendMoney(sum);
    }

    public void AddUser(UserId userId) => _userIds.Add(userId);

    public void RemoveUser(UserId userId) => _userIds.Remove(userId);

    private void SpendMoney(Money money)
    {
        Budget -= money;

        if (Budget < BudgetLowerBound)
        {
            //TODO may be domain event
        }
    }
}