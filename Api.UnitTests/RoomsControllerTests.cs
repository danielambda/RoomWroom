using Api.Rooms;
using Application.Rooms.Commands;
using Contracts.Rooms;
using Domain.Common.Enums;
using Domain.Common.Errors;
using Domain.Common.ValueObjects;
using Domain.ReceiptAggregate;
using Domain.RoomAggregate;
using FakeItEasy;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Patterns;

namespace Api.UnitTests;

public class RoomsControllerTests
{
    [Fact]
    public async Task Create_ValidRequest_ReturnsCreatedResponse()
    {
        // Arrange
        var fakeSender = A.Fake<ISender>();
        RoomsController controller = new(fakeSender);
        CreateRoomRequest request = new 
        (
            "Test Room",
            1000,
            "Rub",
            0,
            false,
            [],
            []
        );
        var expectedRoom = Room.CreateNew
        (
            request.Name,
            new Money(1000, Currency.Rub),
            0,
            false,
            [],
            []
        );
        A.CallTo(() => fakeSender.Send(request.ToCommand(), default))
            .Returns(expectedRoom);
        // Act
        IActionResult actionResult = await controller.Create(request);

        // Assert
        Assert.IsType<ObjectResult>(actionResult);
        var objectResult = (ObjectResult)actionResult;
        Assert.Equal(objectResult.StatusCode, StatusCodes.Status201Created);
        Assert.IsType<RoomResponse>(objectResult.Value);

        var room = (RoomResponse)objectResult.Value;
        
        Assert.Equal(room.Name, expectedRoom.Name);
        Assert.Equal(room.BudgetAmount, expectedRoom.ToResponse().BudgetAmount);
        Assert.Equal(room.BudgetCurrency, expectedRoom.ToResponse().BudgetCurrency);
    }
    
    [Fact]
    public async Task Create_InvalidRequest_ReturnsProblemResponse()
    {
        // Arrange
        var request = new CreateRoomRequest("", 0, "", 0, false, [], []);
        var fakeSender = A.Fake<ISender>();
        A.CallTo(() => fakeSender.Send(request.ToCommand(), default))
            .Returns(Errors.Room.EmptyName);
        var controller = new RoomsController(fakeSender);

        // Act
        var result = await controller.Create(request);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var objectResult = (ObjectResult)result;
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        Assert.IsType<ProblemDetails>(objectResult.Value);
}
}