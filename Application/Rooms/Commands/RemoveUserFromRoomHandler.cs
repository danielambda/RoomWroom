using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;

namespace Application.Rooms.Commands;

public class RemoveUserFromRoomHandler(
    IRoomRepository roomRepository, 
    IUserRepository userRepository)
    : IRequestHandler<RemoveUserFromRoomCommand, ErrorOr<Success>>
{
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<ErrorOr<Success>> Handle(RemoveUserFromRoomCommand request, CancellationToken cancellationToken)
    {
        var (userId, roomId) = request;

        bool userWasRemoved = await _roomRepository.TryRemoveUserFromRoomAsync(userId, roomId, cancellationToken);
        if (userWasRemoved is false)
            return Errors.Room.NotFound(roomId);

        bool roomWasRemoved = await _userRepository.TryRemoveRoomFromUser(userId, cancellationToken);
        if (roomWasRemoved is false)
            return Errors.User.NotFound(userId);

        return new Success();
    }
}