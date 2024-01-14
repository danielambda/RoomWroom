using System.Collections.Concurrent;
using Application.Shopping.Interfaces;
using Domain.ReceiptAggregate;

namespace Infrastructure.Shopping.Repositories;

public class MemoryReceiptRepository : IReceiptRepository
{
    private readonly ConcurrentDictionary<string, Receipt> _receipts = [];
    
    public Task<Receipt?> GetAsync(string id, CancellationToken cancellationToken = default) => 
        Task.FromResult(_receipts.GetValueOrDefault(id));

    public Task AddAsync(Receipt receipt, CancellationToken cancellationToken = default)
    {
        string id = receipt.Id.Value.ToString();
        _receipts.TryAdd(id, receipt);
        
        return Task.CompletedTask;
    }
}