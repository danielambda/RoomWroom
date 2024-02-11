using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;

namespace Application.Common.Interfaces.Perception;

public interface IRoomRepository
{
    Task<Room?> GetAsync(RoomId id, CancellationToken cancellationToken = default);

    Task AddAsync(Room room, CancellationToken cancellationToken = default!);
    
    Task SaveChangesAsync();
}