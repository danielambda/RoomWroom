using System.Net;
using System.Net.Http.Json;
using Api.IntegrationTests.Common;
using Api.IntegrationTests.Common.DataGeneration;
using Api.IntegrationTests.Common.Extensions;
using Api.IntegrationTests.Common.Models;
using Contracts.Rooms;
using Domain.Common.Errors;
using Domain.Common.ValueObjects;
using Domain.ReceiptAggregate.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.IntegrationTests.RoomController
{
    public class AddReceiptToRoomRoomControllerTests(IntegrationTestWebAppFactory factory) 
        : IntegrationTestBase(factory)
    {
        [Fact]
        public async Task AddReceiptToRoom_ShouldAddReceiptItemsToRoom_ValidRequestWithoutExclusions()
        {
            // Arrange
            var receipt = Fakers.ReceiptFaker.Generate();
            var room = Fakers.RoomFaker.Clone()
                .RuleFor(r => r.Budget, faker =>
                    new Money(faker.Random.Decimal(), receipt.Currency))
                .Generate();

            DbContext.Receipts.Add(receipt);
            DbContext.Rooms.Add(room);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            var request = new AddReceiptToRoomRequest(receipt.Id!, []);

            // Act
            var response = await Client.PostAsJsonAsync($"rooms/{room.Id.Value}/receipt", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedRoom = await DbContext.Rooms.FindAsync(room.Id); 
            
            foreach (var item in receipt.Items)
            {
                modifiedRoom!.OwnedShopItems.Should().Contain(responseItem =>
                    responseItem.ShopItemId == item.AssociatedShopItemId &&
                    responseItem.Quantity == item.Quantity &&
                    responseItem.Price == item.Price &&
                    responseItem.Sum == item.Sum
                );
            }
            
            modifiedRoom!.Budget.Should().Be(room.Budget - receipt.Sum);
        }

        [Fact]
        public async Task AddReceiptToRoom_ShouldAddOnlyNotExcludedItemsToRoom_ValidRequestWithExclusions()
        {
            // Arrange
            var receipt = Fakers.ReceiptFaker.Generate();
            var room = Fakers.RoomFaker.Clone()
                .RuleFor(r => r.Budget, faker =>
                    new Money(faker.Random.Decimal(), receipt.Currency))
                .Generate();

            DbContext.Receipts.Add(receipt);
            DbContext.Rooms.Add(room);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            var excludedItemsIndices = Enumerable
                                            .Range(0, receipt.Items.Count)
                                            .Where(_ => Random.Shared.Next() % 2 == 0);
            var request = new AddReceiptToRoomRequest(receipt.Id!, excludedItemsIndices.ToList());

            // Act
            var response = await Client.PostAsJsonAsync($"rooms/{room.Id.Value}/receipt", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedRoom = await DbContext.Rooms.FindAsync(room.Id);
            var expectedBudget = room.Budget;
            
            for (var i = 0; i < receipt.Items.Count; i++)
            {
                if (request.ExcludedItemsIndices.Count > 0 &&
                    request.ExcludedItemsIndices.Contains(i))
                    continue;
                
                ReceiptItem item = receipt.Items[i];
                
                modifiedRoom!.OwnedShopItems.Should().Contain(responseItem =>
                    responseItem.ShopItemId == item.AssociatedShopItemId &&
                    responseItem.Quantity == item.Quantity &&
                    responseItem.Price == item.Price &&
                    responseItem.Sum == item.Sum
                );

                expectedBudget -= item.Sum;
            }
            
            modifiedRoom!.Budget.Should().Be(expectedBudget);
        }
        
        [Fact]
        public async Task AddReceiptToRoom_ShouldReturnReceiptNotFound_ReceiptDoesNotExist()
        {
            // Arrange
            var room = Fakers.RoomFaker.Generate();

            await DbContext.Rooms.AddAsync(room);
            await DbContext.SaveChangesAsync();

            var request = new AddReceiptToRoomRequest(ReceiptId.CreateNew()!, []);

            // Act
            var response = await Client.PostAsJsonAsync($"rooms/{room.Id.Value}/receipt", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            problemDetails.Should().NotBeNull();
            problemDetails!.ErrorCodes().Should().Contain(Errors.Receipt.NotFound.Code);
        }
    }
}
