using RoomWroom.CommandHandling;

namespace RoomWroom.Grocery;

internal class GroceryCallbackActionsProvider : ICallbackActionsProvider
{
    internal const string MARK_ALL_SHARED_CALLBACK = "Grocery_MarkAllShared";  
    internal const string SELECT_SHARED_CALLBACK = "Grocery_SelectShared";
    internal const string EDIT_NAMES_CALLBACK = "Grocery_EditNames";

    public IEnumerable<CallbackAction> GetCallbackActions() =>
    [
        new(MARK_ALL_SHARED_CALLBACK, MarkAllShared, CallbackActionType.ClearButtonsAndNewMessage),
        new(SELECT_SHARED_CALLBACK, SelectShared, CallbackActionType.EditCurrentMessage),
        new(EDIT_NAMES_CALLBACK, EditNames, CallbackActionType.EditCurrentMessage)
    ];

    private static async Task<Response> MarkAllShared(ResponseUnit response)
    {
        await Task.Delay(123);
        return "This thing is not implemented yet";
    }

    private static Task<Response> SelectShared(ResponseUnit responseUnit)
    {
        IEnumerable<List<ResponseCallbackButton>> newResponseCallbackButtons =
        [
            [],
            [new("Cancel", "c"), new("Confirm", "c")]
        ];

        

        Response response = new(responseUnit with { ResponseCallbackButtons = newResponseCallbackButtons });
        return Task.FromResult(response);
    }

    private static async Task<Response> EditNames(ResponseUnit responseUnit)
    {
        return new Response(responseUnit with
        {
            ResponseCallbackButtons =
            [
                [
                    new("1", "1"), new("2", "2"), new("3", "3"), new("4", "4"), new("5", "5"), new("6", "6"),
                    new(">", "6")
                ],
                [new("Cancel", "c"), new("Confirm", "c")]
            ]
        });
    }
}