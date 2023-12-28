namespace RoomWroom.CommandHandling;

internal class CallbackHandler : ICallbackResponseProvider
{
    private readonly List<CallbackAction> _callbackActions = [];

    public CallbackHandler(IEnumerable<ICallbackActionsProvider> callbackActionsProviders)
    {
        foreach (ICallbackActionsProvider commandProvider in callbackActionsProviders) 
            _callbackActions.AddRange(commandProvider.GetCallbackActions());
    }
    
    public static CallbackHandler CopyCallbackActionsFrom(CallbackHandler callbackHandler) =>
        new(callbackHandler._callbackActions);

    private CallbackHandler(List<CallbackAction> callbackActions) => _callbackActions = [..callbackActions];

    public (Task<Response>, CallbackActionType)? GetCallbackResponseTask(string callback, ResponseUnit responseUnit) 
        => _callbackActions.SingleOrDefault(callbackAction 
                                         => callbackAction.Matches(callback))?.InvokeTask(responseUnit);
}