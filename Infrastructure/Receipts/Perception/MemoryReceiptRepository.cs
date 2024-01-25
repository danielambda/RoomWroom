using System.Collections.Concurrent;
using Application.Receipts.Interfaces;
using Domain.ReceiptAggregate;

namespace Infrastructure.Receipts.Repositories;

public class MemoryReceiptRepository : IReceiptRepository
{
    private readonly ConcurrentDictionary<string, Receipt> _receipts = [];
    
    public Task<Receipt?> GetAsync(string id, CancellationToken cancellationToken = default) => 
        Task.FromResult(_receipts.GetValueOrDefault(id));

    public Task<Receipt?> GetFromQrAsync(string qr, CancellationToken cancellationToken = default) => 
        Task.FromResult(_receipts.Values.SingleOrDefault(receipt => receipt.Qr == qr));

    public Task AddAsync(Receipt receipt, CancellationToken cancellationToken = default)
    {
        string id = receipt.Id.Value.ToString();
        _receipts.TryAdd(id, receipt);
        
        return Task.CompletedTask;
    }
}