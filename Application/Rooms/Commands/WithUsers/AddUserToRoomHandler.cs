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
        
        Room? room = await _roomRepository.GetAsync(roomId, cancellationToken);
        if (room is null)
            return Errors.Room.NotFound(roomId);

        User? user = await _userRepository.GetAsync(userId, cancellationToken);
        if (user is null)
            return Errors.User.NotFound(userId);
        
        room.AddUser(userId);
        await _roomRepository.SaveChangesAsync();
        
        user.SetRoom(roomId);
        await _userRepository.SaveChangesAsync();

        return new Success();
    }
}