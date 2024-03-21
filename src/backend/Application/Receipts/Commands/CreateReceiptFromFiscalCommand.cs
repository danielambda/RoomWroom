using Domain.ReceiptAggregate;
using Domain.UserAggregate.ValueObjects;

namespace Application.Receipts.Commands;

//This feature does not work due to ФНС
public record CreateReceiptFromFiscalCommand(
    DateTime DateTime,
    decimal Sum,
    string FiscalDriveNumber,
    string FiscalDocumentNumber,
    string FiscalSign,
    UserId CreatorId 
) : IRequest<ErrorOr<Receipt>>;