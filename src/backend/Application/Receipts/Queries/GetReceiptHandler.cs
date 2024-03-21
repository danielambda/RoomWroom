using Application.Common.Interfaces.Perception;
using Domain.ReceiptAggregate;

namespace Application.Receipts.Queries;

public class GetReceiptHandler(
    IReceiptRepository receiptRepository
) : IRequestHandler<GetReceiptQuery, ErrorOr<Receipt>>
{
    private readonly IReceiptRepository _receiptRepository = receiptRepository;
    
    public async Task<ErrorOr<Receipt>> Handle(GetReceiptQuery query, CancellationToken cancellationToken)
    {
        var receiptId = query.Id;
        
        Receipt? receipt = await _receiptRepository.GetAsync(receiptId, cancellationToken);
        if (receipt is null)
            return Error.NotFound($"{nameof(Receipt)} {receiptId}");

        return receipt;
    }
}