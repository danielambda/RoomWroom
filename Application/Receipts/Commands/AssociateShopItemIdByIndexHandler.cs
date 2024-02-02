using Application.Common.Interfaces.Perception;
using Domain.ReceiptAggregate;

namespace Application.Receipts.Commands;

public class AssociateShopItemIdByIndexHandler(
    IReceiptRepository repository) 
    : IRequestHandler<AssociateShopItemIdByIndexCommand, ErrorOr<Success>>
{
    private readonly IReceiptRepository _repository = repository;
    
    public async Task<ErrorOr<Success>> Handle(AssociateShopItemIdByIndexCommand request, CancellationToken cancellationToken)
    {
        Receipt? receipt = await _repository.GetAsync(request.ReceiptId, cancellationToken);

        if (receipt is null)
            return Error.NotFound(nameof(Receipt)); //TODO и тут тоже 
        
        receipt.AssociateShopItemIdAtIndex(request.AssociatedShopItemId, request.Index);

        return new Success();
    }
}