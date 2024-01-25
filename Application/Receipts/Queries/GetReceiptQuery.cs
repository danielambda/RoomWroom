using Domain.ReceiptAggregate;

namespace Application.Receipts.Queries;

public record GetReceiptQuery(string Id) : IRequest<ErrorOr<Receipt>>;