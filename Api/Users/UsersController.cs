using Application.Users.Commands;
using Application.Users.Queries;
using Contracts.Users;
using Domain.UserAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Api.Users;

[Route("users")]
public class UsersController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        CreateUserCommand command = request.ToCommand();

        ErrorOr<User> result = await Mediator.Send(command);

        return result.Match(
            user => Ok(user.ToResponse()),
            errors => Problem(errors));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        GetUserQuery query = new(id!);

        ErrorOr<User> result = await Mediator.Send(query);
        
        return result.Match(
            user => Ok(user.ToResponse()),
            errors => Problem(errors));
    }
}