using Domain.ReceiptAggregate;
using Domain.UserAggregate.ValueObjects;

namespace Application.Receipts.Commands;

public record CreateReceiptFromQrCommand(string Qr, UserId CreatorId) : IRequest<ErrorOr<Receipt>>;