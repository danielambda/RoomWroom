using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

namespace Application.Common.Interfaces.Perception;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    
    Task<User?> GetAsync(UserId id, CancellationToken cancellationToken = default);
    Task<bool> TrySetRoomForUser(RoomId roomId, UserId userId, CancellationToken cancellationToken = default);
    Task<bool> TryRemoveRoomFromUser(UserId userId, CancellationToken cancellationToken);
}