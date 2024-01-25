using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.Enums;
using Domain.UserAggregate.ValueObjects;

namespace Domain.UserAggregate;

public class User : AggregateRoot<UserId>
{
    public string Name { get; }
    public RoomId RoomId { get; }
    public UserRole Role { get; }
    
    private User(UserId id, string name, RoomId roomId, UserRole role) : base(id)
    {
        Name = name;
        RoomId = roomId;
        Role = role;
    }
}