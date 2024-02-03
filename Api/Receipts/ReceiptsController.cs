using Application.Receipts.Commands;
using Application.Receipts.Queries;
using Contracts.Receipts;
using Domain.ReceiptAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Api.Receipts;

[Route("receipts")]
public class ReceiptsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost("qr")]
    public async Task<IActionResult> CreateFromQr(CreateReceiptFromQrRequest request)
    {
        CreateReceiptFromQrCommand command = request.ToCommand();
        
        ErrorOr<Receipt> result = await _mediator.Send(command);

        return result.Match(
            receipt => OkCreated(receipt.ToResponse()),
            errors => Problem(errors));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        GetReceiptQuery query = new(id);
        
        ErrorOr<Receipt> result = await _mediator.Send(query);
        
        return result.Match(
            receipt => Ok(receipt.ToResponse()),
            errors => Problem(errors));
    }

    [HttpPost("{id}/associate-shop-item")]
    public async Task<IActionResult> AssociateShopItemIdByIndex(
        string id, AssociateShopItemIdByIndexRequest request)
    {
        AssociateShopItemIdByIndexCommand command = (id, request).ToCommand();

        ErrorOr<Success> result = await _mediator.Send(command);

        return result.Match(
            _ => Ok(),
            errors => Problem(errors));
    }

    [HttpPost("{id}/associate-shop-items")]
    public async Task<IActionResult> AssociateShopItemIdsByIndices(
        string id, AssociateShopItemIdsByIndicesRequest request)
    {
        AssociateShopItemIdsByIndicesCommand command = (id, request).ToCommand();

        ErrorOr<Success> result = await _mediator.Send(command);

        return result.Match(
            _ => Ok(),
            errors => Problem(errors));
    }
}