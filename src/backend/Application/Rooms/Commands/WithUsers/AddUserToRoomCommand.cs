using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Rooms.Commands.WithUsers;

public record AddUserToRoomCommand(UserId userId, RoomId roomId) : IRequest<ErrorOr<Success>>;