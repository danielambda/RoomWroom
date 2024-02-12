using Domain.Common.ValueObjects;
using Domain.RoomAggregate;

namespace Application.Rooms.Commands;

public record CreateRoomCommand(
    string Name,
    Money Budget,
    decimal BudgetLowerBound,
    bool MoneyRoundingRequired,
    IEnumerable<string> UserIds,
    IEnumerable<OwnedShopItemCommand> OwnedShopItems
) : IRequest<ErrorOr<Room>>;

public record OwnedShopItemCommand(
    string ShopItemId,
    decimal Quantity,
    Money Price
);