using Domain.ShopItemAggregate;
using Domain.ShopItemAggregate.ValueObjects;

namespace Application.Common.Interfaces;

public interface IShopItemRepository
{
    Task<ShopItem?> GetAsync(ShopItemId id, CancellationToken cancellationToken = default);
    Task AddAsync(ShopItem shopItem, CancellationToken cancellationToken = default);
    Task AddAsync(IEnumerable<ShopItem> shopItems, CancellationToken cancellationToken = default);
}