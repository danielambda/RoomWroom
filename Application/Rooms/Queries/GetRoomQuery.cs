using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;

namespace Application.Rooms.Queries;

public record GetRoomQuery(RoomId RoomId) : IRequest<ErrorOr<Room>>;