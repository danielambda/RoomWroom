﻿using Application.Receipts.Commands;
using Application.Receipts.Queries;
using Contracts.Receipts;
using Domain.ReceiptAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Api.Receipts;

[Route("receipt")]
public class ReceiptsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost("qr")]
    public async Task<IActionResult> CreateFromQr(CreateReceiptFromQrRequest request)
    {
        CreateReceiptFromQrCommand command = request.ToCommand();
        
        ErrorOr<Receipt> result = await _mediator.Send(command);

        return result.Match(
            receiptResult => OkCreated(receiptResult.ToResponse()),
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
}