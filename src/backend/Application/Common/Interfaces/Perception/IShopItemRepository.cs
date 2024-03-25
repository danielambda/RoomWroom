using Domain.ShopItemAggregate;
using Domain.ShopItemAggregate.ValueObjects;

namespace Application.Common.Interfaces.Perception;

public interface IShopItemRepository
{
    Task<ShopItem?> GetAsync(ShopItemId id, CancellationToken cancellationToken = default);
    
    Task<IQueryable<ShopItem>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task AddAsync(ShopItem shopItem, CancellationToken cancellationToken = default);
    
    Task AddAsync(IEnumerable<ShopItem> shopItems, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(ShopItemId id, CancellationToken cancellationToken = default);
    
    Task<bool> CheckExistenceAsync(ShopItemId id, CancellationToken cancellationToken);
}