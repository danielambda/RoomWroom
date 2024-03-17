using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.RoomAggregate;
using Domain.UserAggregate;

namespace Application.Rooms.Commands.WithUsers;

public class AddUserToRoomHandler(
    IUserRepository userRepository,
    IRoomRepository roomRepository
) : IRequestHandler<AddUserToRoomCommand, ErrorOr<Success>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoomRepository _roomRepository = roomRepository;
    
    public async Task<ErrorOr<Success>> Handle(AddUserToRoomCommand command, CancellationToken cancellationToken)
    {
        var (userId, roomId) = command;
        
        User? user = await _userRepository.GetAsync(userId, cancellationToken);
        if (user is null)
            return Errors.User.NotFound(userId);
        if (user.RoomId is { } userRoomId)
            return Errors.User.RoomAlreadySet(userId, userRoomId);
        
        Room? room = await _roomRepository.GetAsync(roomId, cancellationToken);
        if (room is null)
            return Errors.Room.NotFound(roomId);
        
        room.AddUser(userId);
        
        user.SetRoom(roomId);

        return Result.Success;
    }
}