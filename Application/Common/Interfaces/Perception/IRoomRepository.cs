using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Common.Interfaces.Perception;

public interface IRoomRepository
{
    Task<Room?> GetAsync(RoomId id, CancellationToken cancellationToken = default);

    Task AddAsync(Room room, CancellationToken cancellationToken = default!);
    
    Task AddShopItemToRoomAsync(OwnedShopItem shopItem, RoomId roomId, CancellationToken cancellationToken = default);
    
    Task AddShopItemsToRoomAsync(IEnumerable<OwnedShopItem> shopItems, RoomId roomId,
        CancellationToken cancellationToken = default);

    Task<bool> TryAddUserToRoomAsync(UserId userId, RoomId roomId, CancellationToken cancellationToken = default);
    Task<bool> TryRemoveUserFromRoomAsync(UserId userId, RoomId roomId, CancellationToken cancellationToken = default);
}