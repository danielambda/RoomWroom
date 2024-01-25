using Domain.ReceiptAggregate;

namespace Application.Receipts.Interfaces;

public interface IReceiptRepository
{
    Task<Receipt?> GetAsync(string id, CancellationToken cancellationToken = default);

    Task<Receipt?> GetFromQrAsync(string qr, CancellationToken cancellationToken = default);

    Task AddAsync(Receipt receipt, CancellationToken cancellationToken = default);
}