using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;

namespace Application.Receipts.Commands;

public class AssociateShopItemIdsByIndicesHandler(
    IReceiptRepository repository,
    IShopItemAssociationsRepository associationsRepository
) : IRequestHandler<AssociateShopItemIdsByIndicesCommand, ErrorOr<Success>>
{
    private readonly IReceiptRepository _repository = repository;
    private readonly IShopItemAssociationsRepository _associationsRepository = associationsRepository;

    public async Task<ErrorOr<Success>> Handle(AssociateShopItemIdsByIndicesCommand command,
        CancellationToken cancellationToken)
    {
        var (associatedShopItemIds, saveAssociations, receiptId) = command;
        associatedShopItemIds = associatedShopItemIds as ShopItemId[] ?? associatedShopItemIds.ToArray();
        
        Receipt? receipt = await _repository.GetAsync(receiptId, cancellationToken);
        if (receipt is null)
            return Errors.Receipt.NotFound(receiptId);

        receipt.AssociateShopItemIdsByIndices(associatedShopItemIds);
        await _repository.SaveChangesAsync();

        IEnumerable<ShopItemAssociation> associations = GenerateAssociations(receipt, associatedShopItemIds);
        
        if (saveAssociations)
            await _associationsRepository.AddOrUpdateAsync(associations, cancellationToken);
        
        return Result.Success;
    }

    private static IEnumerable<ShopItemAssociation> GenerateAssociations(
        Receipt receipt, IEnumerable<ShopItemId?> associatedIds)
    {
        var currentIndex = 0;
        
        foreach (ShopItemId? associatedId in associatedIds)
        {
            if (associatedId != null)
            {
                string originalName = receipt.Items[currentIndex].Name;

                yield return new ShopItemAssociation(originalName, associatedId);
            }

            currentIndex++;
        }
    }
}