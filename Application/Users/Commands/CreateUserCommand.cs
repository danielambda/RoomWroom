using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.Enums;

namespace Application.Users.Commands;

public record CreateUserCommand(
    string Name,
    UserRole UserRole,
    RoomId? RoomId
) : IRequest<ErrorOr<User>>;