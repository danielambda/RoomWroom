using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.RoomAggregate;
using Domain.UserAggregate;

namespace Application.Rooms.Commands.WithUsers;

public class RemoveUserFromRoomHandler(
    IRoomRepository roomRepository, 
    IUserRepository userRepository
) : IRequestHandler<RemoveUserFromRoomCommand, ErrorOr<Deleted>>
{
    private readonly IRoomRepository _roomRepository = roomRepository;
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<ErrorOr<Deleted>> Handle(RemoveUserFromRoomCommand command, CancellationToken cancellationToken)
    {
        var (userId, roomId) = command;

        Room? room = await _roomRepository.GetAsync(roomId, cancellationToken);
        if (room is null)
            return Errors.Room.NotFound;

        User? user = await _userRepository.GetAsync(userId, cancellationToken);
        if (user is null)
            return Errors.User.NotFound;

        room.RemoveUser(userId);
        user.RemoveRoom();
        
        return Result.Deleted;
    }
}