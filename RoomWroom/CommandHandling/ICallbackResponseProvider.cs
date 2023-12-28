namespace RoomWroom.CommandHandling;

public interface ICallbackResponseProvider
{
    public (Task<Response>, CallbackActionType)? GetCallbackResponseTask(string callback, ResponseUnit responseUnit);
}