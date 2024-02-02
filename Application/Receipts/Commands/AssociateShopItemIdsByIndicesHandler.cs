using Application.Common.Interfaces.Perception;
using Domain.ReceiptAggregate;

namespace Application.Receipts.Commands;

public class AssociateShopItemIdsByIndicesHandler(
    IReceiptRepository repository) 
    : IRequestHandler<AssociateShopItemIdsByIndicesCommand, ErrorOr<Success>>
{
    private readonly IReceiptRepository _repository = repository;

    public async Task<ErrorOr<Success>> Handle(AssociateShopItemIdsByIndicesCommand request,
        CancellationToken cancellationToken)
    {
        Receipt? receipt = await _repository.GetAsync(request.ReceiptId, cancellationToken);

        if (receipt is null)
            return Error.NotFound(nameof(Receipt)); //TODO и тут тоже 
        
        receipt.AssociateShopItemIdsByIndices(request.AssociatedShopItemIds);

        return new Success();
    }
}