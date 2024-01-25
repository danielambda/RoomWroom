using Application.Receipts.Commands;
using Application.Receipts.Queries;
using Contracts.Receipts;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;

namespace Api.Common.Mapping;

public interface IMapper
{
    public ReceiptResponse MapToResponse(Receipt receipt);
    public ReceiptItemResponse MapToResponse(ReceiptItem receiptItem);
    public CreateReceiptFromQrCommand MapToCommand(CreateReceiptFromQrRequest request);
    public GetReceiptQuery MapToQuery(GetReceiptRequest request);
}