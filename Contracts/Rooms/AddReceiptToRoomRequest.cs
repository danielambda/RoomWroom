namespace Contracts.Rooms;

public record AddReceiptToRoomRequest(string ReceiptId, List<int> ExcludedItemsIds);