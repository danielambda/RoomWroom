using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Common;
using Api.IntegrationTests.Common.DataGeneration;
using Api.IntegrationTests.Common.Models;
using Contracts.ShopItems;

namespace Api.IntegrationTests.ShopItemController;

public class GetAllShopItemControllerTests(IntegrationTestWebAppFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetAll_ReturnsAllShopItems_ShopItemsExist()
    {
        //Arrange
        var shopItems = Fakers.ShopItemFaker.Generate(Random.Shared.Next(2, 10));
        DbContext.ShopItems.AddRange(shopItems);
        await DbContext.SaveChangesAsync();
        
        //Act
        var response = await Client.GetAsync("shop-items");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var shopItemResponses = await response.Content.ReadFromJsonAsync<List<ShopItemResponse>>();

        shopItemResponses.Should().NotBeNull();
        foreach (var shopItem in shopItems)
        {
            shopItemResponses.Should().Contain(item => item!.Id == shopItem.Id!);
            shopItemResponses.Should().Contain(item => item.Name == shopItem.Name);
            shopItemResponses.Should().Contain(item => item.Quantity == shopItem.Quantity);
            shopItemResponses.Should().Contain(item => item.Units == shopItem.Units.ToString());
            shopItemResponses.Should().Contain(item => item.IngredientId == shopItem.IngredientId!);   
        }
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithEmptyList_NoShopItems()
    {
        //Act
        var response = await Client.GetAsync("shop-items");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var shopItemResponses = await response.Content.ReadFromJsonAsync<List<ShopItemResponse>>();
        shopItemResponses.Should().BeEmpty();
    }
}