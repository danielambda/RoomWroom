using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Common;
using Api.IntegrationTests.Common.DataGeneration;
using Api.IntegrationTests.Common.Models;
using Contracts.ShopItems;

namespace Api.IntegrationTests.ShopItemController;

public class CreateShopItemControllerTests(IntegrationTestWebAppFactory factory) 
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Create_ShouldAddShopItem_ValidShopItem()
    {
        //Arrange
        var request = Fakers.CreateShopItemRequestFaker.Generate();
        
        //Act
        var response = await Client.PostAsJsonAsync("shop-items", request);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var shopItemResponse = await response.Content.ReadFromJsonAsync<ShopItemResponse>();

        shopItemResponse.Should().BeEquivalentTo(request);
    }
}