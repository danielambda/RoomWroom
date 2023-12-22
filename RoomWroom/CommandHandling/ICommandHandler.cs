namespace RoomWroom.CommandHandling;

public interface ICommandHandler
{
    public ResponseProvider ResponseProvider { get; }
}