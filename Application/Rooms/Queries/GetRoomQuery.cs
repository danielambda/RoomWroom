using Domain.RoomAggregate;

namespace Application.Rooms.Queries;

public record GetRoomQuery(string RoomId) : IRequest<ErrorOr<Room>>;