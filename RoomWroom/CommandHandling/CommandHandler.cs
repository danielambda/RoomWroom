namespace RoomWroom.CommandHandling;

public class CommandHandler
{
    private readonly List<Command> _commands = [];

    private Command? _currentCommand;
    private Queue<Type> _requestedTypes = [];

    public ResponseProvider ResponseProvider { get; }

    public CommandHandler(IEnumerable<ICommandProvider> commandProviders)
    {
        foreach (ICommandProvider commandProvider in commandProviders) 
            _commands?.AddRange(commandProvider.GetCommands());

        ResponseProvider = GetResponse;
    }
    
    public static CommandHandler CopyCommandsFrom(CommandHandler commandHandler) => new(commandHandler._commands);

    private CommandHandler(List<Command> commands)
    {
        _commands = [..commands];
        
        ResponseProvider = GetResponse;
    }
    
    private string? GetResponse(string? text, Image? image)
    {
        if (text is not null)
            return GetResponseForText(text);

        if (image is not null)
            return GetResponseForImage(image);            

        return null;
    }

    private string? GetResponseForImage(Image image)
    {
        throw new NotImplementedException();
    }

    private string? GetResponseForText(string text)
    {
        if (IsCommand(text))
        {
            _currentCommand = _commands.SingleOrDefault(command => command.Matches(text));

            if (_currentCommand is null) 
                return null;
            
            ICollection<Type> requiredTypes = _currentCommand.GetRequiredTypes();
            RequestDataOfTypes(requiredTypes);
            
            return _currentCommand.ImmediateResponse;
        }

        return null;
    }

    private void RequestDataOfTypes(IEnumerable<Type> types)
    {
        
    }

    private static bool IsCommand(string text) => text.StartsWith('/');
}