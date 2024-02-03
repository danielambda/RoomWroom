using Application.Common.Interfaces.Perception;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;

namespace Application.Receipts.Commands;

public class AssociateShopItemIdsByIndicesHandler(
    IReceiptRepository repository,
    IShopItemAssociationsRepository associationsRepository) 
    : IRequestHandler<AssociateShopItemIdsByIndicesCommand, ErrorOr<Success>>
{
    private readonly IReceiptRepository _repository = repository;
    private readonly IShopItemAssociationsRepository _associationsRepository = associationsRepository;

    public async Task<ErrorOr<Success>> Handle(AssociateShopItemIdsByIndicesCommand command,
        CancellationToken cancellationToken)
    {
        Receipt? receipt = await _repository.GetAsync(command.ReceiptId, cancellationToken);

        if (receipt is null)
            return Error.NotFound(nameof(Receipt)); //TODO и тут тоже 
        
        receipt.AssociateShopItemIdsByIndices(command.AssociatedShopItemIds);

        IEnumerable<ShopItemAssociation> associations = GenerateAssociations(receipt, command.AssociatedShopItemIds);
        
        if (command.SaveAssociations)
            await _associationsRepository.AddOrUpdateAsync(associations, cancellationToken);
        
        return new Success();
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