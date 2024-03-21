using Application.Users.Commands;
using Contracts.Users;
using Domain.UserAggregate;
using Domain.UserAggregate.Enums;

namespace Api.Users;

public static class Mapper
{
    public static CreateUserCommand ToCommand(this CreateUserRequest request) =>
        new(request.Name, Enum.Parse<UserRole>(request.UserRole), request.RoomId);

    public static UserResponse ToResponse(this User user) =>
        new(user.Id!, user.Name, user.Role.ToString(), user.RoomId);
}