using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Rooms.Commands;

public record AddUserToRoomCommand(UserId userId, RoomId roomId) : IRequest<ErrorOr<Success>>;