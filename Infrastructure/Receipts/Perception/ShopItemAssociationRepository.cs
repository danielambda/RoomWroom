using Application.Common.Interfaces.Perception;
using Domain.ReceiptAggregate.ValueObjects;
using Infrastructure.Common.Persistence;

namespace Infrastructure.Receipts.Perception;

public class ShopItemAssociationRepository(
    RoomWroomDbContext dbContext    
) : IShopItemAssociationsRepository
{
    private readonly RoomWroomDbContext _dbContext = dbContext;
    
    private DbSet<ShopItemAssociation> Associations => _dbContext.ShopItemAssociations;
    
    public async Task<ShopItemAssociation?> GetAsync(string originalName, CancellationToken cancellationToken) 
        => await Associations.FindAsync([originalName], cancellationToken);

    public async Task AddOrUpdateAsync(ShopItemAssociation association, CancellationToken cancellationToken)
    {
        bool associationAlreadyExists =
            await Associations.AnyAsync(item => item.OriginalName == association.OriginalName, cancellationToken);

        if (associationAlreadyExists)
            Associations.Update(association);
        else
            await Associations.AddAsync(association, cancellationToken);
        
        await SaveChangesAsync(cancellationToken);
    }

    public async Task AddOrUpdateAsync(IEnumerable<ShopItemAssociation> associations, CancellationToken cancellationToken)
    {
        await _dbContext.ShopItemAssociations.AddRangeAsync(associations, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await _dbContext.SaveChangesAsync(cancellationToken);
}
