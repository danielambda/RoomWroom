using Application.Receipts.Interfaces;
using Domain.ReceiptAggregate;

namespace Application.Receipts.Commands;

public class CreateReceiptFromQrHandler(
    IReceiptFromQrCreator receiptFromQrCreatorProvider,
    IReceiptRepository receiptRepository) 
    : IRequestHandler<CreateReceiptFromQrCommand, ErrorOr<Receipt>>
{
    private readonly IReceiptFromQrCreator _receiptFromQrCreatorProvider = receiptFromQrCreatorProvider;
    private readonly IReceiptRepository _receiptRepository = receiptRepository;
    
    public async Task<ErrorOr<Receipt>> Handle(
        CreateReceiptFromQrCommand request, CancellationToken cancellationToken)
    {
        string qr = request.Qr;
        
        bool receiptAlreadyExists = await _receiptRepository.CheckExistenceByQr(qr, cancellationToken: cancellationToken);
        if (receiptAlreadyExists)
            return Error.Conflict(description: $"{nameof(Receipt)} with qr {qr} already exists");
        
        Receipt? receipt = await _receiptFromQrCreatorProvider.CreateAsync(qr, cancellationToken);
        if (receipt is null)
            return Error.Failure(description: $"{nameof(Receipt)} {qr}");

        await _receiptRepository.AddAsync(receipt, cancellationToken);
        
        return receipt;
    }
}