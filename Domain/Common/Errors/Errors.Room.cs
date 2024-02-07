using Domain.RoomAggregate.ValueObjects;
using ErrorOr;

namespace Domain.Common.Errors;

public static partial class Errors
{
    public static class Room
    {
        public static Error NotFound(RoomId id) =>
            Error.NotFound("Room.NotFound", $"Room with id '{id}' was not found");
    }
}