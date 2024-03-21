using Domain.ShopItemAggregate.ValueObjects;

namespace Domain.ReceiptAggregate.ValueObjects;

public class ShopItemAssociation : ValueObjectBase
{
    public string OriginalName { get; private set; } = default!;
    public ShopItemId ShopItemId { get; private set; } = default!;

    public ShopItemAssociation(string originalName, ShopItemId shopItemId)
    {
        OriginalName = originalName;
        ShopItemId = shopItemId;
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return OriginalName;
        yield return ShopItemId;
    }

    private ShopItemAssociation()
    {
        
    }
}