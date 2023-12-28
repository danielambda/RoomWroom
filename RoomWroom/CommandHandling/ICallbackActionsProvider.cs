namespace RoomWroom.CommandHandling;

internal interface ICallbackActionsProvider
{
    IEnumerable<CallbackAction> GetCallbackActions();
}