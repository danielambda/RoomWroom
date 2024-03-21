using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

namespace Application.Receipts.Commands;

public class AssociateShopItemIdByIndexHandler(
    IReceiptRepository repository,
    IShopItemAssociationsRepository associationsRepository
) : IRequestHandler<AssociateShopItemIdByIndexCommand, ErrorOr<Success>>
{
    private readonly IReceiptRepository _repository = repository;
    private readonly IShopItemAssociationsRepository _associationsRepository = associationsRepository;

    public async Task<ErrorOr<Success>> Handle(AssociateShopItemIdByIndexCommand command,
        CancellationToken cancellationToken)
    {
        var (associatedShopItemId, index, saveAssociation, receiptId) = command;
        
        Receipt? receipt = await _repository.GetAsync(receiptId, cancellationToken);
        if (receipt is null)
            return Errors.Receipt.NotFound(receiptId); 

        receipt.AssociateShopItemIdAtIndex(associatedShopItemId, index);
        await _repository.SaveChangesAsync(cancellationToken);

        ShopItemAssociation association = new(receipt.Items[index].Name, associatedShopItemId);
        
        if (saveAssociation) 
            await _associationsRepository.AddOrUpdateAsync(association, cancellationToken);

        return Result.Success;
    }
}