using Application.Common.Interfaces.Perception;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Infrastructure.Common.Persistence;

namespace Infrastructure.Receipts.Perception;

public class ReceiptRepository(
    RoomWroomDbContext dbContext
) : IReceiptRepository
{
    private readonly RoomWroomDbContext _dbContext = dbContext;
    
    public async Task<Receipt?> GetAsync(ReceiptId id, CancellationToken cancellationToken) 
        => await _dbContext.Receipts.FindAsync([id], cancellationToken);

    public async Task<bool> CheckExistenceByQr(string? qr, CancellationToken cancellationToken) 
        => await _dbContext.Receipts.AnyAsync(receipt => receipt.Qr == qr, cancellationToken);

    public async Task AddAsync(Receipt receipt, CancellationToken cancellationToken) 
        => await _dbContext.Receipts.AddAsync(receipt, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await _dbContext.SaveChangesAsync(cancellationToken);
}