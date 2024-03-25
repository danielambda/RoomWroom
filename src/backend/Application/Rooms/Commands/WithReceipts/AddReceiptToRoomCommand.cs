using Domain.ReceiptAggregate.ValueObjects;
using Domain.RoomAggregate.ValueObjects;

namespace Application.Rooms.Commands.WithReceipts;

public record AddReceiptToRoomCommand
(
    ReceiptId ReceiptId,
    List<int> ExcludedItemsIndices, 
    RoomId RoomId
) : IRequest<ErrorOr<Success>>; 