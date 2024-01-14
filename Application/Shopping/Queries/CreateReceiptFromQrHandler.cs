using Application.Shopping.Interfaces;
using Domain.ReceiptAggregate;

namespace Application.Shopping.Queries;

public class CreateReceiptFromQrHandler(
    IReceiptFromQrCreator receiptFromQrCreatorProvider,
    IReceiptRepository receiptRepository) 
    : IRequestHandler<CreateReceiptFromQrCommand, ErrorOr<ReceiptResult>>
{
    private readonly IReceiptFromQrCreator _receiptFromQrCreatorProvider = receiptFromQrCreatorProvider;
    private readonly IReceiptRepository _receiptRepository = receiptRepository;
    
    public async Task<ErrorOr<ReceiptResult>> Handle(CreateReceiptFromQrCommand request, CancellationToken cancellationToken)
    {
        Receipt? receipt = await _receiptFromQrCreatorProvider.CreateAsync(request.Qr, cancellationToken);

        if (receipt is null)
            return Error.Failure($"{nameof(Receipt)} {request.Qr}");

        await _receiptRepository.AddAsync(receipt, cancellationToken);
        
        return new ReceiptResult(receipt);
    }
}