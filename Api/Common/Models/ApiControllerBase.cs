using Microsoft.AspNetCore.Mvc;

namespace Api.Common.Models;

[ApiController]
public abstract class ApiControllerBase(ISender mediator) : ControllerBase
{
    protected readonly ISender Mediator = mediator;
    
    protected IActionResult Problem(IEnumerable<Error> errors)
    {
        Error firstError = errors.First();
        int statusCode = firstError.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }

    protected IActionResult OkCreated(object? value) => StatusCode(StatusCodes.Status201Created, value);
}