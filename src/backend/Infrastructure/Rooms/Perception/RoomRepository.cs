using Application.Common.Interfaces.Perception;
using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;
using Infrastructure.Common.Persistence;

namespace Infrastructure.Rooms.Perception;

public class RoomRepository(
    RoomWroomDbContext dbContext    
) : IRoomRepository
{
    private readonly RoomWroomDbContext _dbContext = dbContext;

    public async Task<Room?> GetAsync(RoomId id, CancellationToken cancellationToken = default) 
        => await _dbContext.Rooms.FindAsync([id], cancellationToken);

    public async Task AddAsync(Room room, CancellationToken cancellationToken = default)
    {
        await _dbContext.Rooms.AddAsync(room, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken) 
        => _dbContext.SaveChangesAsync(cancellationToken);
}