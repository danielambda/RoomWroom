using Application.Common.Interfaces.Perception;
using Domain.RoomAggregate.ValueObjects;

namespace Application.Rooms.Commands;

public class AddShopItemToRoomHandler(IRoomRepository repository)
    : IRequestHandler<AddShopItemToRoomCommand, ErrorOr<Success>>
{
    private readonly IRoomRepository _repository = repository;

    public async Task<ErrorOr<Success>> Handle(AddShopItemToRoomCommand command, CancellationToken cancellationToken = default)
    {
        var ownedShopItem = OwnedShopItem.Create(command.ShopItemId, command.Quantity);
        
        await _repository.AddShopItemToRoomAsync(ownedShopItem, command.RoomId, cancellationToken);

        return new Success();
    }
}