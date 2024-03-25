using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.Common.ValueObjects;
using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;

namespace Application.Rooms.Commands.WithShopItems;

public class AddShopItemToRoomHandler(
    IRoomRepository roomRepository,
    IShopItemRepository shopItemRepository
) : IRequestHandler<AddShopItemToRoomCommand, ErrorOr<Success>>
{
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly IShopItemRepository _shopItemRepository = shopItemRepository;

    public async Task<ErrorOr<Success>> Handle(AddShopItemToRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var (shopItemId, quantity, price, roomId) = command;
        
        var ownedShopItem = new OwnedShopItem(shopItemId, quantity, price ?? Money.Zero);
        
        Room? room = await _roomRepository.GetAsync(roomId, cancellationToken);
        if (room is null)
            return Errors.Room.NotFound;

        bool shopItemExists = await _shopItemRepository.CheckExistenceAsync(shopItemId, cancellationToken);
        if (shopItemExists is false)
            return Errors.ShopItem.NotFound;
        
        if (room.Budget.Currency != ownedShopItem.Price.Currency)
            return Errors.Money.MismatchedCurrency;
        
        room.AddOwnedShopItem(ownedShopItem);
        await _roomRepository.SaveChangesAsync(cancellationToken);
        
        return Result.Success;
    }
}