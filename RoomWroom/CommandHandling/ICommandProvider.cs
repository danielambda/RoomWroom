namespace RoomWroom.CommandHandling;

public interface ICommandProvider
{
    public IEnumerable<Command> GetCommands();
}