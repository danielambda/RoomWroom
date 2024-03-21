using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Common;
using Api.IntegrationTests.Common.Models;
using Contracts.Rooms;
using Domain.Common.ValueObjects;
using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

namespace Api.IntegrationTests.RoomsController;

public class GetRoomsControllerTests(IntegrationTestWebAppFactory factory) 
    : IntegrationTestBase(factory)
{
    [Fact]
    private async Task Get_ReturnsRoom_WhenRoomExists()
    {
        //Arrange
        var mockRoom = Room.CreateNew
        (
            "801", Money.Zero, 123, true,
            [UserId.CreateNew()],
            [new OwnedShopItem(ShopItemId.CreateNew(), 0, Money.Zero)]
        );

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
    private async Task Get_ReturnsNotFound_WhenRoomDoesNotExists()
    {
        //Act
        var response = await Client.GetAsync($"rooms/{RoomId.CreateNew().Value}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}