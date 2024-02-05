using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Rooms.Commands;

public record RemoveUserFromRoomCommand(UserId userId, RoomId roomId) : IRequest<ErrorOr<Success>>;