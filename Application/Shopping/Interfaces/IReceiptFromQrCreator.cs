using Domain.ReceiptAggregate;

namespace Application.Shopping.Interfaces;

public interface IReceiptFromQrCreator
{
    Task<Receipt?> CreateAsync(string qr, CancellationToken cancellationToken = default);
}