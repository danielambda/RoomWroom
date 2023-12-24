namespace RoomWroom.CommandHandling;

internal interface ICommandProvider
{
    IEnumerable<Command> GetCommands();
}