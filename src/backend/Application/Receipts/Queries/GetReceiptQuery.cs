using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

namespace Application.Receipts.Queries;

public record GetReceiptQuery(ReceiptId Id) : IRequest<ErrorOr<Receipt>>;