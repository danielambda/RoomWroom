using Application.Receipts.Commands;
using Contracts.Receipts;
using Domain.Common.Enums;
using Domain.Common.ValueObjects;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;

namespace Api.Receipts;

public static class Mapper
{
    public static CreateReceiptCommand ToCommand(this CreateReceiptRequest request, string userId) =>
        new(request.Items.ConvertAll(item =>
            new ReceiptItemCommand(item.Name,
                new Money(item.MoneyAmount, Enum.Parse<Currency>(item.MoneyCurrency)),
                item.Quantity,
                item.AssociatedShopItemId!)),
            request.Qr,
            userId!); 

    public static CreateReceiptFromQrCommand ToCommand(this CreateReceiptFromQrRequest request, string userId) =>
        new(request.Qr, userId!);

    public static CreateReceiptFromFiscalCommand ToCommand(
        this CreateReceiptFromFiscalRequest request, string userId) =>
        new(request.DateTime, request.Sum, request.FiscalDrive, request.FiscalDocument, request.FiscalSign, userId!);
    
    public static ReceiptResponse ToResponse(this Receipt receipt) =>
        new(receipt.Id!,
            receipt.Items.Select(item =>
                item.ToResponse()).ToList(),
            receipt.Qr,
            receipt.CreatorId!);

    private static ReceiptItemResponse ToResponse(this ReceiptItem receiptItem) =>
        new(receiptItem.Name,
            receiptItem.Price.Amount,
            receiptItem.Price.Currency.ToString(),
            receiptItem.Quantity,
            receiptItem.AssociatedShopItemId);

    public static AssociateShopItemIdByIndexCommand ToCommand(
        this AssociateShopItemIdByIndexRequest request, string receiptId) =>
        new(request.AssociatedShopItemId!, request.Index, request.SaveAssociation, receiptId!);

    public static AssociateShopItemIdsByIndicesCommand ToCommand(
        this AssociateShopItemIdsByIndicesRequest request, string receiptId) =>
        new(request.AssociatedShopItemIds.Select(id =>
                id is null ? null : ShopItemId.Create(Guid.Parse(id))),
            request.SaveAssociations,
            receiptId!);
}

