using Application.Receipts.Commands;
using Application.Receipts.Queries;
using Contracts.Receipts;
using Domain.ReceiptAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Api.Receipts;

[Route("users/{userId}/receipts")]
public class ReceiptsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateReceiptRequest request, string userId)
    {
        CreateReceiptCommand command = request.ToCommand(userId);

        ErrorOr<Receipt> result = await Mediator.Send(command);
        
        return result.Match(
            receipt => OkCreated(receipt.ToResponse()),
            errors => Problem(errors));
    }
    
    [HttpPost("qr")]
    public async Task<IActionResult> CreateFromQr(CreateReceiptFromQrRequest request, string userId)
    {
        CreateReceiptFromQrCommand command = request.ToCommand(userId);
        
        ErrorOr<Receipt> result = await Mediator.Send(command);

        return result.Match(
            receipt => OkCreated(receipt.ToResponse()),
            errors => Problem(errors));
    }

    //This feature does not work due to ФНС
    /*[HttpPost("fiscal")]
    public async Task<IActionResult> CreateFromFiscal(CreateReceiptFromFiscalRequest request, string userId)
    {
        CreateReceiptFromFiscalCommand command = request.ToCommand(userId);

        ErrorOr<Receipt> result = await Mediator.Send(command);

        return result.Match(
            receipt => OkCreated(receipt.ToResponse()),
            errors => Problem(errors));
    }*/

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, string userId)
    {
        GetReceiptQuery query = new(id!);
        
        ErrorOr<Receipt> result = await Mediator.Send(query);
        
        return result.Match(
            receipt => Ok(receipt.ToResponse()),
            errors => Problem(errors));
    }

    [HttpPost("{id}/associate-shop-item")]
    public async Task<IActionResult> AssociateShopItemIdByIndex(
        string id, AssociateShopItemIdByIndexRequest request, string userId)
    {
        AssociateShopItemIdByIndexCommand command = request.ToCommand(id);

        ErrorOr<Success> result = await Mediator.Send(command);

        return result.Match(
            _ => Ok(),
            errors => Problem(errors));
    }

    [HttpPost("{id}/associate-shop-items")]
    public async Task<IActionResult> AssociateShopItemIdsByIndices(
        string id, AssociateShopItemIdsByIndicesRequest request, string userId)
    {
        AssociateShopItemIdsByIndicesCommand command = request.ToCommand(id);

        ErrorOr<Success> result = await Mediator.Send(command);

        return result.Match(
            _ => Ok(),
            errors => Problem(errors));
    }
}