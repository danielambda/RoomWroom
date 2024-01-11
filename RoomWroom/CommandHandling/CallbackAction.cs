namespace RoomWroom.CommandHandling;

internal class CallbackAction(string callbackText, Func<ResponseUnit, Task<Response>> function, CallbackActionType type)
{
    private readonly string _callbackText = callbackText;

    private readonly Func<ResponseUnit, Task<Response>> _function = function;
    
    private readonly CallbackActionType _type = type;
    
    internal bool Matches(string text) => text.StartsWith(_callbackText);

    internal (Task<Response>, CallbackActionType) InvokeTask(ResponseUnit response) 
        => (_function.Invoke(response), _type);
}

public enum CallbackActionType
{
    NewMessage,
    ClearButtonsAndNewMessage,
    EditCurrentMessage
}