using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.RoomAggregate;

namespace Application.Rooms.Queries;

public class GetRoomHandler(IRoomRepository repository) : IRequestHandler<GetRoomQuery, ErrorOr<Room>>
{
    private readonly IRoomRepository _repository = repository;

    public async Task<ErrorOr<Room>> Handle(GetRoomQuery query, CancellationToken cancellationToken)
    {
        var roomId = query.RoomId;
        
        Room? room = await _repository.GetAsync(roomId!, cancellationToken);

        if (room is null)
            return Errors.Room.NotFound;

        return room;
    }
}