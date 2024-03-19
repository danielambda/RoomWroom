using Application.Rooms.Commands;
using Application.Rooms.Commands.WithReceipts;
using Application.Rooms.Commands.WithShopItems;
using Application.Rooms.Commands.WithUsers;
using Application.Rooms.Queries;
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

        ErrorOr<Room> result = await Mediator.Send(command);

        return result.Match
        (
            room => OkCreated(room.ToResponse()),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        GetRoomQuery command = new(id!);

        ErrorOr<Room> result = await Mediator.Send(command);
        
        return result.Match
        (
            room => Ok(room.ToResponse()),
            errors => Problem(errors)
        );
    }
    
    [HttpPost("{roomId}/shop-item")]
    public async Task<IActionResult> AddShopItemToRoom(string roomId, AddShopItemToRoomRequest request)
    {
        AddShopItemToRoomCommand command = request.ToCommand(roomId);

        ErrorOr<Success> result = await Mediator.Send(command);

        return result.Match
        (
            _ => Ok(),
            errors => Problem(errors)
        );
    }

    [HttpPost("{roomId}/receipt")]
    public async Task<IActionResult> AddReceiptToRoom(string roomId, AddReceiptToRoomRequest request)
    {
        AddReceiptToRoomCommand command = request.ToCommand(roomId);

        ErrorOr<Success> result = await Mediator.Send(command);

        return result.Match
        (
            _ => Ok(),
            errors => Problem(errors)
        );
    }

    [HttpPost("{roomId}/user")]
    public async Task<IActionResult> AddUserToRoom(string roomId, AddUserToRoomRequest request)
    {
        AddUserToRoomCommand command = request.ToCommand(roomId);

        ErrorOr<Success> result = await Mediator.Send(command);
        
        return result.Match
        (
            _ => Ok(),
            errors => Problem(errors)
        );
    }
}