using Domain.ShopItemAggregate.ValueObjects;

namespace Domain.ReceiptAggregate.ValueObjects;

public class ShopItemAssociation : ValueObjectBase
{
    public string OriginalName { get; private set; }
    public ShopItemId ShopItemId { get; private set; }

    public ShopItemAssociation(string originalName, ShopItemId shopItemId)
    {
        OriginalName = originalName;
        ShopItemId = shopItemId;
    }

    private ShopItemAssociation()
    {
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return OriginalName;
        yield return ShopItemId;
    }
}