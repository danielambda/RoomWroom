using Domain.ReceiptAggregate;

namespace Application.Receipts.Interfaces;

public interface IReceiptFromQrCreator
{
    Task<Receipt?> CreateAsync(string qr, CancellationToken cancellationToken = default);
}