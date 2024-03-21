using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Common;
using Api.IntegrationTests.Common.DataGeneration;
using Api.IntegrationTests.Common.Extensions;
using Api.IntegrationTests.Common.Models;
using Contracts.Receipts;
using Domain.Common.Errors;
using Domain.ReceiptAggregate.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.IntegrationTests.ReceiptsController;

public class GetReceiptsControllerTests(IntegrationTestWebAppFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task Get_ReturnsReceipt_ReceiptExists()
    {
        //Arrange
        var user = Fakers.UserFaker.Generate();
        var receipt = Fakers.ReceiptFaker.Clone().RuleFor(r => r.CreatorId, _ => user.Id).Generate();
        DbContext.Users.Add(user);
        DbContext.Receipts.Add(receipt);
        await DbContext.SaveChangesAsync();

        //Act
        var receiptResponse = await Client.GetFromJsonAsync<ReceiptResponse>(
                $"users/{receipt.CreatorId.Value}/receipts/{receipt.Id.Value}");

        //Assert
        receiptResponse.Should().NotBeNull();
        receiptResponse!.Id.Should().Be(receipt.Id!);
        receiptResponse.Qr.Should().Be(receipt.Qr);
        receiptResponse.CreatorId.Should().Be(receipt.CreatorId!);
        foreach (var receiptItem in receiptResponse.Items)
        {
            receiptItem.Name.Should().BeOneOf(
                receipt.Items.Select(item => item.Name));
            receiptItem.PriceAmount.Should().BeOneOf(
                receipt.Items.Select(item => item.Price.Amount));
            receiptItem.PriseCurrency.Should().Be(receipt.Items[0].Price.Currency.ToString());
            receiptItem.Quantity.Should().BeOneOf(
                receipt.Items.Select(item => item.Quantity));
            receiptItem.AssociatedShopItemId.Should().BeOneOf(
                receipt.Items.Select(item => item.AssociatedShopItemId?.Value.ToString()));
        }
    }

    [Fact]
    public async Task Get_ReturnsReceiptNotFound_ReceiptDoesNotExist()
    {
        //Arrange
        var user = Fakers.UserFaker.Generate();
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();
        
        //Act
        var response = await Client.GetAsync($"users/{user.Id.Value}/receipts/{ReceiptId.CreateNew().Value}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails!.ErrorCodes().Should().Contain(Errors.Receipt.NotFound.Code);
    }
    
    [Fact]
    public async Task Get_Returns404NotFound_CreatorDoesNotExist()
    {
        //Arrange
        var receipt = Fakers.ReceiptFaker.Generate();
        DbContext.Receipts.Add(receipt);
        await DbContext.SaveChangesAsync();
        
        //Act
        var response = await Client.GetAsync($"users/{receipt.CreatorId.Value}/receipts/{receipt.Id.Value}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        problemDetails.Should().NotBeNull();
        problemDetails!.ErrorCodes().Should().Contain(Errors.User.NotFound.Code);
    }
}