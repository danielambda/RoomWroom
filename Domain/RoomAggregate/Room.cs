using Domain.Common.Exceptions;
using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.RoomAggregate;

public sealed class Room : AggregateRoot<RoomId>
{
    public string Name { get; private set; }
    public Money Budget { get; private set; }
    public decimal BudgetLowerBound { get; private set; }
    public bool MoneyRoundingRequired { get; private set; }
    public IReadOnlyList<UserId> UserIds => _userIds.AsReadOnly();
    public IReadOnlyList<OwnedShopItem> OwnedShopItems => _ownedShopItems.AsReadOnly();

    private readonly List<UserId> _userIds;
    
    private readonly List<OwnedShopItem> _ownedShopItems;

    private Room(RoomId id, string name, Money budget, decimal budgetLowerBound, bool moneyRoundingRequired,
        IEnumerable<UserId> userIds, IEnumerable<OwnedShopItem> ownedShopItems)
        : base(id)
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
        IEnumerable<UserId> userIds, IEnumerable<OwnedShopItem> ownedShopItems)
        => new(id, name, budget, budgetLowerBound, moneyRoundingRequired, userIds, ownedShopItems);

    public static Room CreateNew(
        string name, Money budget, decimal budgetLoverBound, bool moneyRoundingRequired,
        IEnumerable<UserId> userIds, IEnumerable<OwnedShopItem> ownedShopItems) =>
        new(RoomId.CreateUnique(), name, budget, budgetLoverBound, moneyRoundingRequired, userIds, ownedShopItems);

    public void AddOwnedShopItem(OwnedShopItem shopItem)
    {
        if (shopItem.Price.Currency != Budget.Currency)
            throw new MismatchedСurrenciesExceptions();
        
        _ownedShopItems.Add(shopItem);

        Money sum = shopItem.Sum;
        
        if (MoneyRoundingRequired)
            sum = sum.ToRounded();
        
        SpendMoney(sum);
    }

    public void AddOwnedShopItems(IEnumerable<OwnedShopItem> shopItems)
    {
        var ownedShopItems = shopItems as OwnedShopItem[] ?? shopItems.ToArray();
        
        if (ownedShopItems.Length == 0)
            return;
        
        _ownedShopItems.AddRange(ownedShopItems);

        Money sum = ownedShopItems
            .Select(item => item.Sum)
            .Aggregate((s1, s2) => s1 + s2);

        if (sum.Currency != Budget.Currency)
            throw new MismatchedСurrenciesExceptions();
        
        if (MoneyRoundingRequired)
            sum = sum.ToRounded();
        
        SpendMoney(sum);
    }

    public void AddUser(UserId userId)
    {
        if (_userIds.Contains(userId))
            return;
        
        _userIds.Add(userId);
    }

    public void RemoveUser(UserId userId) => _userIds.Remove(userId);

    private void SpendMoney(Money money)
    {
        if (money.Currency != Budget.Currency)
            throw new MismatchedСurrenciesExceptions();
        
        Budget -= money;

        if (Budget < BudgetLowerBound)
        {
            //TODO may be domain event
        }
    }

#pragma warning disable CS8618
    private Room()
#pragma warning restore CS8618
    {
    }
}