using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.UserAggregate;

namespace Application.Users.Queries;

public class GetUserHandler(
    IUserRepository repository) 
    : IRequestHandler<GetUserQuery, ErrorOr<User>>
{
    private readonly IUserRepository _repository = repository;
    
    public async Task<ErrorOr<User>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var userId = query.UserId;

        var user = await _repository.GetAsync(userId, cancellationToken);

        if (user is null)
            return Errors.User.NotFound(userId);

        return user;
    }
}