using Application.Common.Interfaces.Perception;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;
using Infrastructure.Common.Persistence;

namespace Infrastructure.Users.Perception;

public class UserRepository(RoomWroomDbContext dbContext) : IUserRepository
{
    private readonly RoomWroomDbContext _dbContext = dbContext;
    
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetAsync(UserId id, CancellationToken cancellationToken = default)
        => await _dbContext.Users.FindAsync([id], cancellationToken);

    public async Task<bool> CheckExistenceAsync(UserId id, CancellationToken cancellationToken = default) 
        => await _dbContext.Users.AnyAsync(user => user.Id == id, cancellationToken);
}