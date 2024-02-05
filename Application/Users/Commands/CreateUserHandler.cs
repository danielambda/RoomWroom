using Application.Common.Interfaces.Perception;
using Domain.UserAggregate;

namespace Application.Users.Commands;

public class CreateUserHandler (
    IUserRepository repository, 
    IRoomRepository roomRepository)
    : IRequestHandler<CreateUserCommand, ErrorOr<User>>
{
    private readonly IUserRepository _repository = repository;
    private readonly IRoomRepository _roomRepository = roomRepository;
    
    public async Task<ErrorOr<User>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = User.CreateNew(command.Name, command.UserRole, command.RoomId);

        if (user.RoomId is not null)
            await _roomRepository.TryAddUserToRoomAsync(user.Id, user.RoomId, cancellationToken);
        
        await _repository.AddAsync(user, cancellationToken);

        return user;
    }
}