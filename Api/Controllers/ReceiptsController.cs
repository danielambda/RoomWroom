using Api.Common.Mapping;
using Api.Common.Models;
using Application.Receipts.Commands;
using Application.Receipts.Queries;
using Contracts.Receipts;
using Domain.ReceiptAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("receipt")]
public class ReceiptsController(
    ISender mediator,
    IMapper mapper)
    : ApiControllerBase
{
    private readonly ISender _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    
    [HttpPost("qr")]
    public async Task<IActionResult> CreateFromQr(CreateReceiptFromQrRequest request)
    {
        CreateReceiptFromQrCommand command = _mapper.MapToCommand(request);
        
        ErrorOr<Receipt> result = await _mediator.Send(command);

        return result.Match(
            receiptResult => OkCreated(_mapper.MapToResponse(receiptResult)),
            errors => Problem(errors));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        GetReceiptQuery query = new(id);

        ErrorOr<Receipt> result = await _mediator.Send(query);

        return result.Match(
            receipt => Ok(_mapper.MapToResponse(receipt)),
            errors => Problem(errors));
    }
}