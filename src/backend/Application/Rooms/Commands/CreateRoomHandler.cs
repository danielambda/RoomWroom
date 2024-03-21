using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;

namespace Application.Rooms.Commands;

public class CreateRoomHandler(
    IRoomRepository repository
) : IRequestHandler<CreateRoomCommand, ErrorOr<Room>>
{
    private readonly IRoomRepository _repository = repository;

    public async Task<ErrorOr<Room>> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
    {
        var (name, budget, budgetLowerBound, moneyRoundingRequired, userIds, ownedShopItems) = command;

        if (string.IsNullOrEmpty(name))
            return Errors.Room.EmptyName;
        
        var room = Room.CreateNew(
            name,
            budget,
            budgetLowerBound,
            moneyRoundingRequired,
            userIds, 
            ownedShopItems.Select(item =>
                new OwnedShopItem(item.ShopItemId, item.Quantity, item.Price)
            )
        );

        await _repository.AddAsync(room, cancellationToken);

        return room;
    }
}