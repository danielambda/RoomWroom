using Application.Rooms.Commands;
using Contracts.Rooms;
using Domain.RoomAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Api.Rooms;

[Route("rooms")]
public class RoomsController(ISender mediator) : ApiControllerBase(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateRoomRequest request)
    {
        CreateRoomCommand command = request.ToCommand();

        ErrorOr<Room> result = await _mediator.Send(command);

        return result.Match(
            room => OkCreated(room.ToResponse()),
            errors => Problem(errors));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        GetRoomCommand command = new(id);

        ErrorOr<Room> result = await _mediator.Send(command);
        
        return result.Match(
            room => Ok(room.ToResponse()),
            errors => Problem(errors));
    }
    
    [HttpPost("{roomId}/shop-item")]
    public async Task<IActionResult> AddShopItemToRoom(string roomId, AddShopItemToRoomRequest request)
    {
        AddShopItemToRoomCommand command = (roomId, request).ToCommand();

        await _mediator.Send(command);

        return Ok();
    }
}