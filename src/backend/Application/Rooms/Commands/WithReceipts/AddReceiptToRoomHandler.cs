using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;

namespace Application.Rooms.Commands.WithReceipts;

public class AddReceiptToRoomHandler(
    IRoomRepository repository,
    IReceiptRepository receiptRepository
) : IRequestHandler<AddReceiptToRoomCommand, ErrorOr<Success>>
{
    private readonly IRoomRepository _repository = repository;
    private readonly IReceiptRepository _receiptRepository = receiptRepository;

    public async Task<ErrorOr<Success>> Handle(AddReceiptToRoomCommand command, CancellationToken cancellationToken)
    {
        var (receiptId, excludedItemsId, roomId) = command;

        Receipt? receipt = await _receiptRepository.GetAsync(receiptId, cancellationToken);
        if (receipt is null)
            return Errors.Receipt.NotFound(receiptId);

        Room? room = await _repository.GetAsync(roomId, cancellationToken);
        if (room is null)
            return Errors.Room.NotFound;
        
        OwnedShopItem[] shopItemsToAdd = GetItemsAfterExclusion(receipt.Items, excludedItemsId).ToArray();
        if (shopItemsToAdd.Length == 0)
            return Result.Success;
        
        if (room.Budget.Currency != shopItemsToAdd.First().Price.Currency)
            return Errors.Money.MismatchedCurrency;
        
        room.AddOwnedShopItems(shopItemsToAdd);

        return Result.Success;
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

            yield return new OwnedShopItem(associatedShopItemId, items[i].Quantity, items[i].Price);
        }
    }
}