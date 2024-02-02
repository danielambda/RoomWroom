using Domain.RoomAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;

namespace Application.Rooms.Commands;

public record AddShopItemToRoomCommand(ShopItemId ShopItemId, decimal Quantity, RoomId RoomId)
    : IRequest<ErrorOr<Success>>;