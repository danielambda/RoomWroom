using Application.Shopping.Interfaces;
using Domain.ReceiptAggregate;

namespace Application.Shopping.Queries;

public class GetReceiptHandler(
    IReceiptRepository receiptRepository) 
    : IRequestHandler<GetReceiptQuery, ErrorOr<ReceiptResult>>
{
    private readonly IReceiptRepository _receiptRepository = receiptRepository;
    
    public async Task<ErrorOr<ReceiptResult>> Handle(GetReceiptQuery request, CancellationToken cancellationToken)
    {
        Receipt? receipt = await _receiptRepository.GetAsync(request.Id, cancellationToken);

        if (receipt is null)
            return Error.NotFound($"{nameof(Receipt)} {request.Id}");

        return new ReceiptResult(receipt);
    }
}