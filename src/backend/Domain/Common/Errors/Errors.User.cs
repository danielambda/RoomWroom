using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using ErrorOr;

namespace Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error NotFound =>
            Error.NotFound("User.NotFound", $"User was not found");

        public static Error RoomAlreadySet(UserId id, RoomId roomId) =>
            Error.NotFound("User.RoomAlreadySet", $"User with id '{id.Value}' already has room set to '{roomId}'");
    }
}