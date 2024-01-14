using Domain.ReceiptAggregate;

namespace Application.Shopping.Interfaces;

public interface IReceiptRepository
{
    Task<Receipt?> GetAsync(string id, CancellationToken cancellationToken = default);

    Task AddAsync(Receipt receipt, CancellationToken cancellationToken = default);
}