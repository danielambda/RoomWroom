using Domain.ReceiptAggregate;

namespace Application.Receipts.Commands;

public record CreateReceiptFromQrCommand(string Qr) : IRequest<ErrorOr<Receipt>>;