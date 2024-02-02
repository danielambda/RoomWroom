using Domain.RoomAggregate;

namespace Application.Rooms.Commands;

public record GetRoomCommand(string RoomId) : IRequest<ErrorOr<Room>>;