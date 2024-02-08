using Domain.ReceiptAggregate.ValueObjects;
using Domain.RoomAggregate.ValueObjects;

namespace Application.Rooms.Commands;

public record AddReceiptToRoomCommand(
    ReceiptId ReceiptId,
    List<int> ExcludedItemsId, 
    RoomId RoomId
) : IRequest<ErrorOr<Success>>; 