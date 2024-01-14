using Microsoft.AspNetCore.Mvc;

namespace Api.Common.Models;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult Problem(IEnumerable<Error> errors)
    {
        Error firstError = errors.First();
        int statusCode = firstError.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Failure => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}