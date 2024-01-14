namespace Application.Shopping.Queries;

public record GetReceiptQuery(string Id) : IRequest<ErrorOr<ReceiptResult>>;