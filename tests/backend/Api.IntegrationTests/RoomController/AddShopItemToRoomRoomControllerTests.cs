using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Common;
using Api.IntegrationTests.Common.DataGeneration;
using Api.IntegrationTests.Common.Extensions;
using Api.IntegrationTests.Common.Models;
using Domain.Common.Errors;
using Domain.RoomAggregate.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.IntegrationTests.RoomController;

public class AddShopItemToRoomRoomControllerTests (IntegrationTestWebAppFactory factory) 
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task AddShopItemToRoom_ShouldReturnOk_ShopItemAndRoomExist()
    {
        //Arrange
        var room = Fakers.RoomFaker.Generate();
        var shopItem = Fakers.ShopItemFaker.Generate();

        DbContext.Rooms.Add(room);
        DbContext.ShopItems.Add(shopItem);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        var request = Fakers.AddShopItemToRoomRequestFaker.Generate() with
        {
            ShopItemId = shopItem.Id!,
            PriceCurrency = room.Budget.Currency.ToString()
        };

        //Act
        var response = await Client.PostAsJsonAsync($"rooms/{room.Id.Value}/shop-item", request);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var roomFromDb = await DbContext.Rooms.FindAsync(room.Id);
        roomFromDb!.OwnedShopItems.Should().Contain(item =>
            item.ShopItemId! == request.ShopItemId &&
            item.Quantity == request.Quantity &&
            item.Price.Amount == request.PriceAmount &&
            item.Price.Currency.ToString() == request.PriceCurrency
        );
    }
    
    [Fact]
    public async Task AddShopItemToRoom_ShouldReturnRoomNotFound_WhenRoomDoesNotExist()
    {
        // Arrange
        var shopItem = Fakers.ShopItemFaker.Generate();

        DbContext.ShopItems.Add(shopItem);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();
        
        var request = Fakers.AddShopItemToRoomRequestFaker.Generate() with { ShopItemId = shopItem.Id! }; 

        // Act
        var response = await Client.PostAsJsonAsync($"rooms/{RoomId.CreateNew().Value}/shop-item", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.ErrorCodes().Should().Contain(Errors.Room.NotFound.Code);
    }

    [Fact]
    public async Task AddShopItemToRoom_ShouldReturnNotFound_WhenShopItemDoesNotExist()
    {
        // Arrange
        var room = Fakers.RoomFaker.Generate();

        await DbContext.Rooms.AddAsync(room);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        var request = Fakers.AddShopItemToRoomRequestFaker.Generate();

        // Act
        var response = await Client.PostAsJsonAsync($"rooms/{room.Id.Value}/shop-item", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.ErrorCodes().Should().Contain(Errors.ShopItem.NotFound.Code);
    }
    
    [Fact]
    public async Task AddShopItemToRoom_ShouldReturnBadRequestAndErrorCode_ForMismatchedCurrency()
    {
        // Arrange
        var room = Fakers.RoomFaker.Generate();
        var shopItem = Fakers.ShopItemFaker.Generate();

        DbContext.Rooms.Add(room);
        DbContext.ShopItems.Add(shopItem);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();
        
        var request = Fakers.AddShopItemToRoomRequestFaker.Clone()
            .RuleFor(r => r.PriceCurrency, faker =>
                faker.PickRandomWithout(room.Budget.Currency).ToString())
            .Generate() with {ShopItemId = shopItem.Id! };

        // Act
        var response = await Client.PostAsJsonAsync($"rooms/{room.Id.Value}/shop-item", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.ErrorCodes().Should().Contain(Errors.Money.MismatchedCurrency.Code);
    }
}