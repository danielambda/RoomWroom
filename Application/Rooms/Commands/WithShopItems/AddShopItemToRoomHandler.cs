using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.Common.ValueObjects;
using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;

namespace Application.Rooms.Commands.WithShopItems;

public class AddShopItemToRoomHandler(
    IRoomRepository repository
) : IRequestHandler<AddShopItemToRoomCommand, ErrorOr<Success>>
{
    private readonly IRoomRepository _repository = repository;

    public async Task<ErrorOr<Success>> Handle(AddShopItemToRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var (shopItemId, quantity, money, roomId) = command;
        
        var ownedShopItem = new OwnedShopItem(shopItemId, quantity, money ?? Money.Zero);
        
        Room? room = await _repository.GetAsync(roomId, cancellationToken);
        if (room is null)
            return Errors.Room.NotFound(roomId);

        room.AddOwnedShopItem(ownedShopItem);

        if (money is not null)
            room.SpendMoney(money);
        
        return new Success();
    }
}