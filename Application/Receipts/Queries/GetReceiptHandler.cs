using Application.Common.Interfaces.Perception;
using Application.Receipts.Interfaces;
using Domain.ReceiptAggregate;

namespace Application.Receipts.Queries;

public class GetReceiptHandler(
    IReceiptRepository receiptRepository) 
    : IRequestHandler<GetReceiptQuery, ErrorOr<Receipt>>
{
    private readonly IReceiptRepository _receiptRepository = receiptRepository;
    
    public async Task<ErrorOr<Receipt>> Handle(GetReceiptQuery request, CancellationToken cancellationToken)
    {
        Receipt? receipt = await _receiptRepository.GetAsync(request.Id!, cancellationToken);

        if (receipt is null)
            return Error.NotFound($"{nameof(Receipt)} {request.Id}");

        return receipt;
    }
}