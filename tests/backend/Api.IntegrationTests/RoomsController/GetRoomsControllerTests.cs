using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Common;
using Api.IntegrationTests.Common.DataGeneration;
using Api.IntegrationTests.Common.Extensions;
using Api.IntegrationTests.Common.Models;
using Contracts.Rooms;
using Domain.Common.Errors;
using Domain.RoomAggregate.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.IntegrationTests.RoomsController;

public class GetRoomsControllerTests(IntegrationTestWebAppFactory factory) 
    : IntegrationTestBase(factory)
{
    [Fact]
    private async Task Get_ReturnsRoom_RoomExists()
    {
        //Arrange
        var mockRoom = Fakers.RoomFaker.Generate();

        DbContext.Rooms.Add(mockRoom);
        await DbContext.SaveChangesAsync();
        
        //Act
        var response = await Client.GetAsync($"rooms/{mockRoom.Id.Value}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var roomResponse = await response.Content.ReadFromJsonAsync<RoomResponse>();

        roomResponse.Should().NotBeNull();
        roomResponse!.Id.Should().Be(mockRoom.Id.Value.ToString());
        roomResponse.BudgetAmount.Should().Be(mockRoom.Budget.Amount);
        roomResponse.BudgetCurrency.Should().Be(mockRoom.Budget.Currency.ToString());
        roomResponse.BudgetLowerBound.Should().Be(mockRoom.BudgetLowerBound);
    }

    [Fact]
    private async Task Get_ReturnsRoomNotFound_RoomDoesNotExists()
    {
        //Act
        var response = await Client.GetAsync($"rooms/{RoomId.CreateNew().Value}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails.Should().NotBeNull();
        problemDetails!.ErrorCodes().Should().Contain(Errors.Room.NotFound.Code);
    }
}