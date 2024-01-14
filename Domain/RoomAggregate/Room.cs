using Domain.RoomAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Domain.RoomAggregate;

public class Room : AggregateRoot<RoomId>
{
    public string Name { get; }
    public IEnumerable<UserId> UserIds { get; }
    
    private Room(RoomId id, string name, IEnumerable<UserId> userIds) : base(id)
    {
        Name = name;
        UserIds = userIds;
    }
}