﻿using Application.Receipts.Commands;
using Contracts.Receipts;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

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
}

