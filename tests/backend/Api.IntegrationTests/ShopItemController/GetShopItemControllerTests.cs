using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Common;
using Api.IntegrationTests.Common.DataGeneration;
using Api.IntegrationTests.Common.Extensions;
using Api.IntegrationTests.Common.Models;
using Contracts.ShopItems;
using Domain.Common.Errors;
using Domain.ShopItemAggregate.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.IntegrationTests.ShopItemController;

public class GetShopItemControllerTests(IntegrationTestWebAppFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Get_ShouldReturnShopItem_ShopItemExists()
    {
        //Arrange
        var shopItem = Fakers.ShopItemFaker.Generate();
        DbContext.ShopItems.Add(shopItem);
        await DbContext.SaveChangesAsync();

        //Act
        var response = await Client.GetAsync($"shop-items/{shopItem.Id.Value}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var shopItemResponse = await response.Content.ReadFromJsonAsync<ShopItemResponse>();

        shopItemResponse.Should().NotBeNull();
        shopItemResponse!.Id.Should().Be(shopItem.Id!);
        shopItemResponse.Name.Should().Be(shopItem.Name);
        shopItemResponse.Quantity.Should().Be(shopItem.Quantity);
        shopItemResponse.Units.Should().Be(shopItem.Units.ToString());
        shopItemResponse.IngredientId.Should().Be(shopItem.IngredientId!);
    }

    [Fact]
    public async Task Get_ReturnsShopItemNotFound_ShopItemDoesNotExist()
    {
        //Act
        var response = await Client.GetAsync($"shop-items/{ShopItemId.CreateNew().Value}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails.Should().NotBeNull();
        problemDetails!.ErrorCodes().Should().Contain(Errors.ShopItem.NotFound.Code);
    }
}