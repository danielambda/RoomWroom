using Application.Receipts.Commands;
using Contracts.Receipts;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;

namespace Api.Receipts;

public static class Mapper
{
    public static CreateReceiptFromQrCommand ToCommand(this CreateReceiptFromQrRequest request) => new(request.Qr);
    
    public static ReceiptResponse ToResponse(this Receipt receipt) => 
        new(receipt.Id!,
            receipt.Qr,
            receipt.Items.Select(item =>
                item.ToResponse()).ToList());

    private static ReceiptItemResponse ToResponse(this ReceiptItem receiptItem) =>
        new(receiptItem.Name,
            receiptItem.Price.Amount,
            receiptItem.Price.Currency.ToString(),
            receiptItem.Quantity,
            receiptItem.AssociatedShopItemId);

    public static AssociateShopItemIdByIndexCommand ToCommand(
        this (string ReceiptId, AssociateShopItemIdByIndexRequest Request) tuple) =>
        new(tuple.Request.AssociatedShopItemId!, tuple.Request.Index, tuple.ReceiptId!);

    public static AssociateShopItemIdsByIndicesCommand ToCommand(
        this (string ReceiptId, AssociateShopItemIdsByIndicesRequest Request) tuple) =>
        new(tuple.Request.AssociatedShopItemIds.Select(id => 
                id is null ? null : ShopItemId.Create(Guid.Parse(id))),
            tuple.ReceiptId!);
}

