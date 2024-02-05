using Application.Common.Interfaces.Perception;

namespace Application.Rooms.Commands;

public class AddUserToRoomHandler(
    IUserRepository userRepository,
    IRoomRepository roomRepository)
    : IRequestHandler<AddUserToRoomCommand, ErrorOr<Success>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoomRepository _roomRepository = roomRepository;
    
    public async Task<ErrorOr<Success>> Handle(AddUserToRoomCommand request, CancellationToken cancellationToken)
    {
        var (userId, roomId) = request;
        
        bool userWasAdded = await _roomRepository.TryAddUserToRoomAsync(userId, roomId, cancellationToken);

        if (userWasAdded is false)
            return Error.Conflict(); //TODO, когда-то придется по всем "Error." пройтись
        
        bool roomWasSet = await _userRepository.TrySetRoomForUser(roomId, userId, cancellationToken);

        if (roomWasSet is true) 
            return new Success();
        
        await _roomRepository.TryRemoveUserFromRoomAsync(userId, roomId, cancellationToken);
            
        return Error.Conflict(); //TODO, когда-то придется по всем "Error." пройтись probably this is not found

    }
}