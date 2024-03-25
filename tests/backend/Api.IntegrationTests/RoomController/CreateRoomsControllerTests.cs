using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Common;
using Api.IntegrationTests.Common.DataGeneration;
using Api.IntegrationTests.Common.Extensions;
using Api.IntegrationTests.Common.Models;
using Contracts.Rooms;
using Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Api.IntegrationTests.RoomController;

public class CreateRoomsControllerTests(IntegrationTestWebAppFactory factory)
    : IntegrationTestBase(factory)
{   
    [Fact]
    public async Task Create_ShouldAddRoom_ValidRoom()
    {
        //Arrange
        var request = Fakers.CreateRoomRequestFaker.Generate();
        
        //Act
        var response = await Client.PostAsJsonAsync("rooms", request);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var roomResponse = await response.Content.ReadFromJsonAsync<RoomResponse>();
        roomResponse.Should().NotBeNull();
        roomResponse!.Name.Should().Be(request.Name);
        roomResponse.BudgetAmount.Should().Be(request.BudgetAmount);
        roomResponse.BudgetCurrency.Should().Be(request.BudgetCurrency);
        roomResponse.BudgetLowerBound.Should().Be(request.BudgetLowerBound);
        roomResponse.MoneyRoundingRequired.Should().Be(request.MoneyRoundingRequired);
        roomResponse.UserIds.Should().BeEquivalentTo(request.UserIds);
        foreach (var ownedShopItem in roomResponse.OwnedShopItems)
        {
            ownedShopItem.ShopItemId.Should().BeOneOf(
                request.OwnedShopItems.Select(item => item.ShopItemId));
            ownedShopItem.Quantity.Should().BeOneOf(
                request.OwnedShopItems.Select(item => item.Quantity));
        }
    }

    [Fact]
    public async Task Create_ShouldReturnEmptyNameError_EmptyRoomName()
    {
        //Arrange
        CreateRoomRequest request = Fakers.CreateRoomRequestFaker.Generate() with { Name = "" };
        
        //Act
        var response = await Client.PostAsJsonAsync("rooms", request);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails.Should().NotBeNull();
        problemDetails!.ErrorCodes().Should().Contain(Errors.Room.EmptyName.Code);
    }
}