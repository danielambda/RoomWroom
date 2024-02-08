using Application.Common.Interfaces.Perception;
using Domain.Common.Errors;
using Domain.RoomAggregate;
using Domain.UserAggregate;

namespace Application.Users.Commands;

public class CreateUserHandler (
    IUserRepository repository, 
    IRoomRepository roomRepository
) : IRequestHandler<CreateUserCommand, ErrorOr<User>>
{
    private readonly IUserRepository _repository = repository;
    private readonly IRoomRepository _roomRepository = roomRepository;
    
    public async Task<ErrorOr<User>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var (name, userRole, roomId) = command;
        
        var user = User.CreateNew(name, userRole, roomId);

        if (roomId is not null)
        {
            Room? room = await _roomRepository.GetAsync(roomId, cancellationToken);
            if (room is null)
                return Errors.Room.NotFound(roomId);
            
            room.AddUser(user.Id);
        }
        
        await _repository.AddAsync(user, cancellationToken);

        return user;
    }
}