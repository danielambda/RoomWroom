using ErrorOr;

namespace Domain.Common.Errors;

public static partial class Errors
{
    public static class Room
    {
        public static Error NotFound =>
            Error.NotFound("Room.NotFound", $"Room was not found");

        public static Error EmptyName
            => Error.Validation("Room.EmptyName", "Room cannot has an empty name");
    }
}