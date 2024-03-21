using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Common;
using Api.IntegrationTests.Common.Models;
using Contracts.Rooms;
using Domain.Common.Enums;
using Domain.Common.Errors;
using Domain.ShopItemAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.IntegrationTests.RoomsController;

public class CreateRoomsControllerTests(IntegrationTestWebAppFactory factory)
    : IntegrationTestBase(factory)
{
    private readonly Faker<CreateRoomRequest> _createRoomRequestFaker =
        new Faker<CreateRoomRequest>()
            .CustomInstantiator(faker =>
            {
                string currency = faker.PickRandom<Currency>().ToString();
                
                return new CreateRoomRequest
                (
                    faker.Company.CompanyName(),
                    faker.Random.Decimal(),
                    currency,
                    faker.Random.Decimal(),
                    faker.Random.Bool(),
                    faker.Make(faker.Random.Number(10), () => UserId.CreateNew().Value.ToString()),
                    faker.Make(faker.Random.Number(10), () =>
                        new OwnedShopItemRequest
                        (
                            ShopItemId.CreateNew()!, faker.Random.Decimal(),
                            faker.Random.Decimal(), currency
                        )
                    )
                );
            });
    
    [Fact]
    public async Task Create_ShouldAddRoom_ValidRoom()
    {
        //Arrange
        CreateRoomRequest request = _createRoomRequestFaker.Generate();
        
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
    public async Task Create_ShouldReturn400BadRequest_EmptyRoomName()
    {
        //Arrange
        CreateRoomRequest request = _createRoomRequestFaker.Generate() with { Name = "" };
        
        //Act
        var response = await Client.PostAsJsonAsync("rooms", request);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails!.Type.Should().Be(Errors.Room.EmptyName.Code);
    }
}