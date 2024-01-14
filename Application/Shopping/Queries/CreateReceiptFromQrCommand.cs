namespace Application.Shopping.Queries;

public record CreateReceiptFromQrCommand(string Qr) : IRequest<ErrorOr<ReceiptResult>>;