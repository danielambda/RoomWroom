namespace RoomWroom.CommandHandling;

public class ResponseProviderFactoryMethodProvider(CommandHandler baseCommandHandler)
{
    private readonly CommandHandler _baseCommandHandler = baseCommandHandler;

    public Func<ResponseProvider> ResponseProviderFactoryMethod => Method; 
    
    private ResponseProvider Method()
    {
        CommandHandler commandHandler = CommandHandler.CopyCommandsFrom(_baseCommandHandler);
        return commandHandler.ResponseProvider;
    }
}