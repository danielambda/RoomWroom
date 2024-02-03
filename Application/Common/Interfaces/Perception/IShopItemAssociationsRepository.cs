using Domain.ReceiptAggregate.ValueObjects;

namespace Application.Common.Interfaces.Perception;

public interface IShopItemAssociationsRepository
{
    Task<ShopItemAssociation?> GetAsync(string originalName, CancellationToken cancellationToken = default);
    
    Task AddOrUpdateAsync(ShopItemAssociation association, CancellationToken cancellationToken = default);
    
    Task AddOrUpdateAsync(IEnumerable<ShopItemAssociation> associations, CancellationToken cancellationToken = default);
}