using Domain.ReceiptAggregate.ValueObjects;

namespace Application.Receipts.Interfaces;

public interface IReceiptItemsFromQrCreator
{
    Task<IEnumerable<ReceiptItem>?> CreateAsync(string qr, CancellationToken cancellationToken = default);
}