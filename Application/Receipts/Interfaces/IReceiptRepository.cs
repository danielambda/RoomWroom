using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

namespace Application.Receipts.Interfaces;

public interface IReceiptRepository
{
    Task<Receipt?> GetAsync(ReceiptId id, CancellationToken cancellationToken = default);

    Task<bool> CheckExistence(string qr, CancellationToken cancellationToken = default);

    Task AddAsync(Receipt receipt, CancellationToken cancellationToken = default);
}