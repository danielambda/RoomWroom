using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Receipts.Queries;

public record GetReceiptQuery(UserId UserId, ReceiptId ReceiptIdId) : IRequest<ErrorOr<Receipt>>;