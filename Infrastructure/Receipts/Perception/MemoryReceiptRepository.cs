using System.Collections.Concurrent;
using Application.Common.Interfaces.Perception;
using Application.Receipts.Interfaces;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

namespace Infrastructure.Receipts.Perception;

public class MemoryReceiptRepository : IReceiptRepository
{
    private readonly ConcurrentDictionary<string, Receipt> _receipts = [];
    
    public Task<Receipt?> GetAsync(ReceiptId id, CancellationToken cancellationToken = default) => 
        Task.FromResult(_receipts.GetValueOrDefault(id!));

    public Task<bool> CheckExistenceByQr(string? qr, CancellationToken cancellationToken = default) =>
        Task.FromResult(_receipts.Values.Any(receipt => receipt.Qr == qr));

    public Task AddAsync(Receipt receipt, CancellationToken cancellationToken = default)
    {
        var id = receipt.Id.Value.ToString();
        _receipts.TryAdd(id, receipt);
        
        return Task.CompletedTask;
    }
}