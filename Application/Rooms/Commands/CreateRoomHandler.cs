using Application.Common.Interfaces.Perception;
using Domain.Common.Enums;
using Domain.Common.ValueObjects;
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
        var (name, budgetAmount, budgetCurrency, budgetLowerBound, userIds, ownedShopItemCommands) = command;
        
        var room = Room.CreateNew(
            name,
            new Money(budgetAmount, Enum.Parse<Currency>(budgetCurrency)),
            budgetLowerBound,
            userIds.Select(id =>
                UserId.Create(Guid.Parse(id))),
            ownedShopItemCommands.Select(item =>
                new OwnedShopItem(item.ShopItemId!, item.Quantity)
            )
        );

        await _repository.AddAsync(room, cancellationToken);

        return room;
    }
}