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
        var (shopItemId, quantity, price, roomId) = command;
        
        var ownedShopItem = new OwnedShopItem(shopItemId, quantity, price ?? Money.Zero);
        
        Room? room = await _repository.GetAsync(roomId, cancellationToken);
        if (room is null)
            return Errors.Room.NotFound(roomId);

        room.AddOwnedShopItem(ownedShopItem);
        await _repository.SaveChangesAsync();
        
        return new Success();
    }
}