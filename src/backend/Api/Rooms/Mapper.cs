using Application.Rooms.Commands;
using Application.Rooms.Commands.WithReceipts;
using Application.Rooms.Commands.WithShopItems;
using Application.Rooms.Commands.WithUsers;
using Contracts.Rooms;
using Domain.Common.Enums;
using Domain.Common.ValueObjects;
using Domain.RoomAggregate;
using Domain.UserAggregate.ValueObjects;

namespace Api.Rooms;

public static class Mapper
{
    public static CreateRoomCommand ToCommand(this CreateRoomRequest request) =>
        new
        (
            request.Name,
            new Money
            (
                request.BudgetAmount,
                Enum.TryParse(request.BudgetCurrency, out Currency currency)
                    ? currency
                    : Currency.None
            ),
            request.BudgetLowerBound,
            request.MoneyRoundingRequired,
            request.UserIds.Select(id => UserId.Create(Guid.Parse(id))),
            request.OwnedShopItems.Select(item =>
                new OwnedShopItemCommand
                (
                    item.ShopItemId!,
                    item.Quantity,
                    new Money
                    (
                        item.PriceAmount,
                        Enum.Parse<Currency>(item.PriceCurrency)
                    )
                )
            )
        );

    public static RoomResponse ToResponse(this Room room) =>
        new
        (
            room.Id!,
            room.Name,
            room.Budget.Amount,
            room.Budget.Currency.ToString(),
            room.BudgetLowerBound,
            room.MoneyRoundingRequired,
            room.UserIds.Select(id => id.Value.ToString()),
            room.OwnedShopItems.Select(item =>
                new OwnedShopItemResponse(item.ShopItemId!, item.Quantity))
        );

    public static AddShopItemToRoomCommand ToCommand(this AddShopItemToRoomRequest request, string roomId) =>
        new
        (
            request.ShopItemId!,
            request.Quantity,
            request is { PriceAmount: not null, PriceCurrency: not null }
                ? new Money
                    (
                        request.PriceAmount.Value,
                        Enum.Parse<Currency>(request.PriceCurrency)
                    )
                : null,
            roomId!
        );

    public static AddReceiptToRoomCommand ToCommand(this AddReceiptToRoomRequest request, string roomId) =>
        new(request.ReceiptId!, request.ExcludedItemsIndices, roomId!);

    public static AddUserToRoomCommand ToCommand(this AddUserToRoomRequest request, string roomId) =>
        new(request.UserId!, roomId!);
}