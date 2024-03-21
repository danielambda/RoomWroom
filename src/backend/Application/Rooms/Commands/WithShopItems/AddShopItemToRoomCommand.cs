using Domain.Common.ValueObjects;
using Domain.RoomAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;

namespace Application.Rooms.Commands.WithShopItems;

public record AddShopItemToRoomCommand(
    ShopItemId ShopItemId,
    decimal Quantity,
    Money? Price,
    RoomId RoomId
) : IRequest<ErrorOr<Success>>;