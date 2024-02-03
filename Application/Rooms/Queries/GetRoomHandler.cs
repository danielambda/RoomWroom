using Application.Common.Interfaces.Perception;
using Domain.RoomAggregate;

namespace Application.Rooms.Queries;

public class GetRoomHandler(IRoomRepository repository) : IRequestHandler<GetRoomQuery, ErrorOr<Room>>
{
    private readonly IRoomRepository _repository = repository;

    public async Task<ErrorOr<Room>> Handle(GetRoomQuery request, CancellationToken cancellationToken)
    {
        Room? room = await _repository.GetAsync(request.RoomId!, cancellationToken);

        if (room is null)
            return Error.NotFound(nameof(Room)); //TODO заменить всё вот такое на Domain Errors

        return room;
    }
}