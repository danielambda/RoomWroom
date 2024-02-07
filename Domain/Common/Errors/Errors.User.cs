using Domain.UserAggregate.ValueObjects;
using ErrorOr;

namespace Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error NotFound(UserId id) =>
            Error.NotFound("User.NotFound", $"User with id '{id}' was not found");
    }
}