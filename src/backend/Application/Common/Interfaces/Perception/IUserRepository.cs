using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

namespace Application.Common.Interfaces.Perception;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    
    Task<User?> GetAsync(UserId id, CancellationToken cancellationToken = default);

    Task<bool> CheckExistenceAsync(UserId id, CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken);
}