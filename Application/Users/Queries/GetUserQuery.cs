using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

namespace Application.Users.Queries;

public record GetUserQuery(UserId UserId) : IRequest<ErrorOr<User>>;