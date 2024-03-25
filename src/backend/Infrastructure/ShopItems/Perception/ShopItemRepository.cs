using Application.Common.Interfaces.Perception;
using Domain.ShopItemAggregate;
using Domain.ShopItemAggregate.ValueObjects;
using Infrastructure.Common.Persistence;

namespace Infrastructure.ShopItems.Perception;

public class ShopItemRepository(
    RoomWroomDbContext dbContext    
) : IShopItemRepository
{
    private readonly RoomWroomDbContext _dbContext = dbContext;
    
    public async Task<ShopItem?> GetAsync(ShopItemId id, CancellationToken cancellationToken = default) 
        => await _dbContext.ShopItems.FindAsync([id], cancellationToken: cancellationToken);

    public Task<IQueryable<ShopItem>> GetAllAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(_dbContext.ShopItems.AsQueryable());

    public async Task AddAsync(ShopItem shopItem, CancellationToken cancellationToken = default)
    {
        _dbContext.ShopItems.Add(shopItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAsync(IEnumerable<ShopItem> shopItems, CancellationToken cancellationToken = default)
    {
        _dbContext.ShopItems.AddRange(shopItems);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(ShopItemId id, CancellationToken cancellationToken = default)
        => await _dbContext.ShopItems.Where(item => item.Id == id)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken) > 0;

    public async Task<bool> CheckExistenceAsync(ShopItemId id, CancellationToken cancellationToken) 
        => await _dbContext.ShopItems.AnyAsync(item => item.Id == id, cancellationToken);
}