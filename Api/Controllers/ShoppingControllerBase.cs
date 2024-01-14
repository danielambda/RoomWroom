using Api.Common.Models;
using Application.Shopping.Queries;
using Contracts.Shopping;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("shop")]
public class ShoppingControllerBase(
    ISender mediator,
    IMapper mapper)
    : ApiControllerBase
{
    private readonly ISender _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    
    [HttpPost("receipt")]
    public async Task<IActionResult> GetReceiptFromQr(CreateReceiptFromQrRequest request)
    {
        CreateReceiptFromQrCommand command = _mapper.Map<CreateReceiptFromQrCommand>(request);
        
        ErrorOr<ReceiptResult> result = await _mediator.Send(command);

        return result.Match(
            receiptResult => Ok(_mapper.Map<ReceiptResponse>(receiptResult)),
            errors => Problem(errors));
    }

    [HttpGet("receipt")]
    public async Task<IActionResult> GetReceiptFromId(GetReceiptRequest request)
    {
        GetReceiptQuery query = _mapper.Map<GetReceiptQuery>(request);
        
        ErrorOr<ReceiptResult> result = await _mediator.Send(query);

        return result.Match(
            receiptResult => Ok(_mapper.Map<ReceiptResponse>(receiptResult)),
            errors => Problem(errors));
    }

}