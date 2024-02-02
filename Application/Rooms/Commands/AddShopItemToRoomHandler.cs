using Application.Rooms.Interfaces;
using Domain.RoomAggregate.ValueObjects;

namespace Application.Rooms.Commands;

public class AddShopItemToRoomHandler(IRoomRepository repository) : IRequestHandler<AddShopItemToRoomCommand>
{
    private readonly IRoomRepository _repository = repository;

    public async Task Handle(AddShopItemToRoomCommand command, CancellationToken cancellationToken = default)
    {
        var ownedShopItem = OwnedShopItem.Create(command.ShopItemId, command.Quantity);
        
        await _repository.AddShopItemToRoomAsync(ownedShopItem, command.RoomId, cancellationToken);
    }
}