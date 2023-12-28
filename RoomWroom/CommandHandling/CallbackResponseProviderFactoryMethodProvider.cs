namespace RoomWroom.CommandHandling;

internal class CallbackResponseProviderFactoryMethodProvider(CallbackHandler baseCallbackHandler)
{
    private readonly CallbackHandler _baseCallbackHandler = baseCallbackHandler;

    public Func<long, ICallbackResponseProvider> CallbackResponseProviderFactoryMethod => Method;

    private ICallbackResponseProvider Method(long id)
    {
        ICallbackResponseProvider callbackHandler = CallbackHandler.CopyCallbackActionsFrom(_baseCallbackHandler);
        return callbackHandler;
    }
}