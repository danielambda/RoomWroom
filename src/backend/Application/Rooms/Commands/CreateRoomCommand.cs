using Domain.Common.ValueObjects;
using Domain.RoomAggregate;
using Domain.ShopItemAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Rooms.Commands;

public record CreateRoomCommand(
    string Name,
    Money Budget,
    decimal BudgetLowerBound,
    bool MoneyRoundingRequired,
    IEnumerable<UserId> UserIds,
    IEnumerable<OwnedShopItemCommand> OwnedShopItems
) : IRequest<ErrorOr<Room>>;

public record OwnedShopItemCommand(
    ShopItemId ShopItemId,
    decimal Quantity,
    Money Price
);