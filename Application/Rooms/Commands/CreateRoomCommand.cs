using Domain.RoomAggregate;

namespace Application.Rooms.Commands;

public record CreateRoomCommand(
    string Name,
    decimal BudgetAmount,
    string BudgetCurrency,
    decimal BudgetLowerBound,
    IEnumerable<string> UserIds,
    IEnumerable<OwnedShopItemCommand> OwnedShopItems)
    : IRequest<ErrorOr<Room>>;

public record OwnedShopItemCommand(string ShopItemId, decimal Quantity);