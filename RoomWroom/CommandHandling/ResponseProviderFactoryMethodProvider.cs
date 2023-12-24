namespace RoomWroom.CommandHandling;

internal class ResponseProviderFactoryMethodProvider(CommandHandler baseCommandHandler)
{
    private readonly CommandHandler _baseCommandHandler = baseCommandHandler;

    public Func<long, IResponseProvider> ResponseProviderFactoryMethod => Method;

    private IResponseProvider Method(long id)
    {
        IResponseProvider commandHandler = CommandHandler.CopyCommandsFrom(_baseCommandHandler);
        return commandHandler;
    }
}