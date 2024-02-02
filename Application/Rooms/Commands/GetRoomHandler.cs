using Application.Rooms.Interfaces;
using Domain.RoomAggregate;

namespace Application.Rooms.Commands;

public class GetRoomHandler(IRoomRepository repository) : IRequestHandler<GetRoomCommand, ErrorOr<Room>>
{
    private readonly IRoomRepository _repository = repository;

    public async Task<ErrorOr<Room>> Handle(GetRoomCommand request, CancellationToken cancellationToken)
    {
        Room? room = await _repository.GetAsync(request.RoomId!, cancellationToken);

        if (room is null)
            return Error.NotFound(nameof(Room)); //TODO заменить всё вот такое на Domain Errors

        return room;
    }
}