using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Common;
using Api.IntegrationTests.Common.Models;
using Contracts.Receipts;
using Domain.UserAggregate.ValueObjects;
using Api.IntegrationTests.Common.DataGeneration;

namespace Api.IntegrationTests.ReceiptsController;

public class CreateReceiptsControllerTests(IntegrationTestWebAppFactory factory) 
    : IntegrationTestBase(factory)
{   
    [Fact]
    public async Task Create_ShouldAddReceipt_ValidReceipt()
    {
        //Arrange
        var creatorId = UserId.CreateNew();
        var request = Fakers.CreateReceiptRequestFaker.Generate();

        //Act
        var response = await Client.PostAsJsonAsync($"users/{creatorId.Value}/receipts", request);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var receiptResponse = await response.Content.ReadFromJsonAsync<ReceiptResponse>();
        receiptResponse.Should().NotBeNull();
        receiptResponse!.Qr.Should().BeNull();
        receiptResponse.CreatorId.Should().Be(creatorId!);
        foreach (var receiptItem in receiptResponse.Items)
        {
            receiptItem.Name.Should().BeOneOf(
                request.Items.Select(item => item.Name));
            receiptItem.PriceAmount.Should().BeOneOf(
                request.Items.Select(item => item.PriceAmount));
            receiptItem.PriseCurrency.Should().Be(request.Items.First().PriceCurrency);
            receiptItem.Quantity.Should().BeOneOf(
                request.Items.Select(item => item.Quantity));
            receiptItem.AssociatedShopItemId.Should().BeOneOf(
                request.Items.Select(item => item.AssociatedShopItemId));
        }
    }
}