using Application.Common.Interfaces.Perception;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

namespace Application.Receipts.Commands;

public class AssociateShopItemIdByIndexHandler(
    IReceiptRepository repository,
    IShopItemAssociationsRepository associationsRepository) 
    : IRequestHandler<AssociateShopItemIdByIndexCommand, ErrorOr<Success>>
{
    private readonly IReceiptRepository _repository = repository;
    private readonly IShopItemAssociationsRepository _associationsRepository = associationsRepository;

    public async Task<ErrorOr<Success>> Handle(AssociateShopItemIdByIndexCommand command,
        CancellationToken cancellationToken)
    {
        Receipt? receipt = await _repository.GetAsync(command.ReceiptId, cancellationToken);

        if (receipt is null)
            return Error.NotFound(nameof(Receipt)); //TODO и тут тоже 

        receipt.AssociateShopItemIdAtIndex(command.AssociatedShopItemId, command.Index);

        ShopItemAssociation association = new(receipt.Items[command.Index].Name, command.AssociatedShopItemId);

        if (command.SaveAssociation) 
            await _associationsRepository.AddOrUpdateAsync(association, cancellationToken);

        return new Success();
    }
}