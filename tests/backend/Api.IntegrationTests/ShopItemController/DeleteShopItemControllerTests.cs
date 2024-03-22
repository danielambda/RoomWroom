using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Common;
using Api.IntegrationTests.Common.DataGeneration;
using Api.IntegrationTests.Common.Extensions;
using Api.IntegrationTests.Common.Models;
using Domain.Common.Errors;
using Domain.ShopItemAggregate.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.IntegrationTests.ShopItemController;

public class DeleteShopItemControllerTests(IntegrationTestWebAppFactory factory) 
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Delete_ReturnsOk_ShopItemExists()
    {
        //Arrange
        var shopItem = Fakers.ShopItemFaker.Generate();

        DbContext.ShopItems.Add(shopItem);
        await DbContext.SaveChangesAsync();
        
        //Act
        var response = await Client.DeleteAsync($"shop-items/{shopItem.Id.Value}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        bool shopItemExists = await DbContext.ShopItems.ContainsAsync(shopItem);
        shopItemExists.Should().Be(false);
    }
    
    [Fact]
    public async Task Delete_ReturnsShopItemNotFound_ShopItemDoesNotExists()
    {
        //Act
        var response = await Client.DeleteAsync($"shop-items/{ShopItemId.CreateNew().Value}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.ErrorCodes().Should().Contain(Errors.ShopItem.NotFound.Code);
    }
}