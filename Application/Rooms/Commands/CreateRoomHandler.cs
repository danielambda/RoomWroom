using Application.Common.Interfaces.Perception;
using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Rooms.Commands;

public class CreateRoomHandler(
    IRoomRepository repository
) : IRequestHandler<CreateRoomCommand, ErrorOr<Room>>
{
    private readonly IRoomRepository _repository = repository;

    public async Task<ErrorOr<Room>> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
    {
        var (name, budget, budgetLowerBound, moneyRoundingRequired, userIds, ownedShopItems) = command;
        
        var room = Room.CreateNew(
            name,
            budget,
            budgetLowerBound,
            moneyRoundingRequired,
            userIds.Select(id =>
                UserId.Create(Guid.Parse(id))
            ),
            ownedShopItems.Select(item =>
                new OwnedShopItem(item.ShopItemId!, item.Quantity, item.Price)
            )
        );

        await _repository.AddAsync(room, cancellationToken);

        return room;
    }
}