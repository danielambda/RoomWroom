using Application.Rooms.Commands;
using Contracts.Rooms;
using Domain.RoomAggregate;

namespace Api.Rooms;

public static class Mapper
{
    public static CreateRoomCommand ToCommand(this CreateRoomRequest request) =>
        new(request.Name, request.BudgetAmount, request.BudgetCurrency,
            request.UserIds, request.OwnedShopItems.Select(item =>
            new OwnedShopItemCommand(item.ShopItemId, item.Quantity)));

    public static RoomResponse ToResponse(this Room room) =>
        new(room.Id!,
            room.Name,
            room.Budget.Amount,
            room.Budget.Currency.ToString(),
            room.UserIds.Select(id =>
                id.Value.ToString()),
            room.OwnedShopItems.Select(item =>
                new OwnedShopItemResponse(item.ShopItemId!, item.Quantity)));
    
    public static AddShopItemToRoomCommand ToCommand(this (string RoomId, AddShopItemToRoomRequest Request) tuple) => 
        new(tuple.Request.ShopItemId!, tuple.Request.Quantity, tuple.RoomId!);

    public static AddReceiptToRoomCommand ToCommand(this (string RoomId, AddReceiptToRoomRequest Request) tuple) =>
        new(tuple.Request.ReceiptId!, tuple.Request.ExcludedItemsIds, tuple.RoomId!);
}