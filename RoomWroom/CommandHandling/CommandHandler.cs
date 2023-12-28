namespace RoomWroom.CommandHandling;

internal class CommandHandler : IResponseProvider
{
    private readonly List<Command> _commands = [];
    private readonly List<Type> _remainingRequestedTypes = [];

    private readonly Queue<Image> _receivedImages = [];
    private readonly Queue<string> _receivedTexts = [];

    private Command? _currentCommand;

    public CommandHandler(IEnumerable<ICommandProvider> commandProviders)
    {
        foreach (ICommandProvider commandProvider in commandProviders) 
            _commands.AddRange(commandProvider.GetCommands());
    }
    
    public static CommandHandler CopyCommandsFrom(CommandHandler commandHandler) => new(commandHandler._commands);

    private CommandHandler(List<Command> commands) => _commands = [..commands];

    public Task<Response>? GetResponseTask(string? text, Image? image)
    {
        if (text is not null)
        {
            if (text.IsCommand())
                return GetResponseForCommand(text);
            
            HandleReceivedText(text);
        }

        if (image is not null)
        {
            HandleReceivedImage(image);
        }

        return _remainingRequestedTypes.Count == 0 ? InvokeCurrentCommandTask() : null;
    }

    private Task<Response>? GetResponseForCommand(string commandText)
    {
        _currentCommand = _commands.SingleOrDefault(command => command.Matches(commandText));

        if (_currentCommand is null) 
            return null;
            
        IEnumerable<Type> requiredTypes = _currentCommand.GetRequiredTypes();
        UpdateRequestedTypes(requiredTypes);
            
        return Task.FromResult(_currentCommand.ImmediateResponse);
    }

    private void HandleReceivedImage(Image image)
    {
        if (_remainingRequestedTypes.Remove(typeof(Image)))
            _receivedImages.Enqueue(image);
    }

    private void HandleReceivedText(string text)
    {
        if (_remainingRequestedTypes.Remove(typeof(string)))
            _receivedTexts.Enqueue(text);
    }

    private void UpdateRequestedTypes(IEnumerable<Type> types)
    {
        IEnumerable<Type> array = types as Type[] ?? types.ToArray();
        
        _remainingRequestedTypes.Clear();
        _remainingRequestedTypes.AddRange(array);
    }

    private Task<Response>? InvokeCurrentCommandTask()
    {
        return _currentCommand switch
        {
            Command<Image> command
                => command.InvokeTask(_receivedImages.Dequeue()),
            Command<string> command
                => command.InvokeTask(_receivedTexts.Dequeue()),
            Command<Image, string> command
                => command.InvokeTask(_receivedImages.Dequeue(), _receivedTexts.Dequeue()),
            Command<string, Image> command
                => command.InvokeTask(_receivedTexts.Dequeue(), _receivedImages.Dequeue()),
            Command<Image, Image> command
                => command.InvokeTask(_receivedImages.Dequeue(), _receivedImages.Dequeue()),
            Command<string, string> command
                => command.InvokeTask(_receivedTexts.Dequeue(), _receivedTexts.Dequeue()),
            Command<Image, Image, Image> command
                => command.InvokeTask(_receivedImages.Dequeue(), _receivedImages.Dequeue(), _receivedImages.Dequeue()),
            Command<Image, Image, string> command
                => command.InvokeTask(_receivedImages.Dequeue(), _receivedImages.Dequeue(), _receivedTexts.Dequeue()),
            Command<Image, string, Image> command
                => command.InvokeTask(_receivedImages.Dequeue(), _receivedTexts.Dequeue(), _receivedImages.Dequeue()),
            Command<Image, string, string> command
                => command.InvokeTask(_receivedImages.Dequeue(), _receivedTexts.Dequeue(), _receivedTexts.Dequeue()),
            Command<string, string, string> command
                => command.InvokeTask(_receivedTexts.Dequeue(), _receivedTexts.Dequeue(), _receivedTexts.Dequeue()),
            _ => null
        };
    }
}

file static class StringExtensions
{  
    public static bool IsCommand(this string str) => str.StartsWith('/');
} 