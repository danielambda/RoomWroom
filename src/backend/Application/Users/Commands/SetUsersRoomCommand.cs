using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Application.Users.Commands;

public record SetUsersRoomCommand(UserId UserId, RoomId RoomId) : IRequest<ErrorOr<Success>>;