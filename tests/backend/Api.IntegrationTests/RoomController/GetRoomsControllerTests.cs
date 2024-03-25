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

namespace Api.IntegrationTests.RoomController;

public class GetRoomsControllerTests(IntegrationTestWebAppFactory factory) 
    : IntegrationTestBase(factory)
{
    [Fact]
    private async Task Get_ReturnsRoom_RoomExists()
    {
        //Arrange
        var room = Fakers.RoomFaker.Generate();

        DbContext.Rooms.Add(room);
        await DbContext.SaveChangesAsync();
        
        //Act
        var response = await Client.GetAsync($"rooms/{room.Id.Value}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var roomResponse = await response.Content.ReadFromJsonAsync<RoomResponse>();

        roomResponse.Should().NotBeNull();
        roomResponse!.Id.Should().Be(room.Id.Value.ToString());
        roomResponse.BudgetAmount.Should().Be(room.Budget.Amount);
        roomResponse.BudgetCurrency.Should().Be(room.Budget.Currency.ToString());
        roomResponse.BudgetLowerBound.Should().Be(room.BudgetLowerBound);
        roomResponse.MoneyRoundingRequired.Should().Be(room.MoneyRoundingRequired);
        
        foreach (var userId in room.UserIds) 
            roomResponse.UserIds.Should().Contain(userId!);
        
        roomResponse.OwnedShopItems.Count.Should().Be(room.OwnedShopItems.Count);
        for (int i = 0; i < roomResponse.OwnedShopItems.Count; i++)
        {
            var actualItem = roomResponse.OwnedShopItems[i];
            var expectedItem = room.OwnedShopItems[i];

            actualItem.ShopItemId.Should().Be(expectedItem.ShopItemId!);
            actualItem.Quantity.Should().Be(expectedItem.Quantity);
            actualItem.PriceAmount.Should().Be(expectedItem.Price.Amount);
            actualItem.PriceCurrency.Should().Be(expectedItem.Price.Currency.ToString());
        }
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