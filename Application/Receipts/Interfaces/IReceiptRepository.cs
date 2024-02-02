using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

namespace Application.Receipts.Interfaces;

public interface IReceiptRepository
{
    Task<Receipt?> GetAsync(ReceiptId id, CancellationToken cancellationToken = default);

    Task<bool> CheckExistenceByQr(string qr, CancellationToken cancellationToken = default);

    Task AddAsync(Receipt receipt, CancellationToken cancellationToken = default);
}