using Application.Common.Interfaces.Perception;
using Application.Rooms.Interfaces;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.RoomAggregate.ValueObjects;

namespace Application.Rooms.Commands;

public class AddReceiptToRoomHandler(
    IRoomRepository repository,
    IReceiptRepository receiptRepository) 
    : IRequestHandler<AddReceiptToRoomCommand, ErrorOr<Success>>
{
    private readonly IRoomRepository _repository = repository;
    private readonly IReceiptRepository _receiptRepository = receiptRepository;
    
    public async Task<ErrorOr<Success>> Handle(AddReceiptToRoomCommand request, CancellationToken cancellationToken)
    {
        Receipt? receipt = await _receiptRepository.GetAsync(request.ReceiptId, cancellationToken);

        if (receipt is null)
            return Error.NotFound(nameof(Receipt)); //TODO тут тоже 

        IEnumerable<OwnedShopItem> shopItemsToAdd = GetItemsAfterExclusion(receipt.Items, request.ExcludedItemsId);
        
        await _repository.AddShopItemsToRoomAsync(shopItemsToAdd, request.RoomId, cancellationToken);

        return new Success();
    }

    private static IEnumerable<OwnedShopItem> GetItemsAfterExclusion(
        IReadOnlyList<ReceiptItem> items,
        IReadOnlyList<int> indexesToExclude)
    {
        for (var i = 0; i < items.Count; i++)
        {
            if (indexesToExclude.Contains(i))
                continue;
            
            if (items[i].AssociatedShopItemId is not { } associatedShopItemId)
                continue;

            yield return OwnedShopItem.Create(associatedShopItemId, items[i].Quantity);
        }
    }
}