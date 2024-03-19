using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Contracts.Rooms;
using Domain.Common.Enums;
using Domain.Common.ValueObjects;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.RoomAggregate;
using Domain.RoomAggregate.ValueObjects;
using Domain.ShopItemAggregate.ValueObjects;
using Domain.UserAggregate;
using Domain.UserAggregate.Enums;
using Domain.UserAggregate.ValueObjects;

namespace Api.IntegrationTests;

public class RoomsControllerTests : IDisposable
{
    private static readonly JsonSerializerOptions JsonParseOptions = new(JsonSerializerDefaults.Web);
    
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public RoomsControllerTests()
    {
        _factory = new();
        _client = _factory.CreateClient();
    }

    [Fact]
    private async Task GetById_ExistingId_ReturnsRoom()
    {
        var mockId = RoomId.CreateUnique();
        var mockRoom = Room.Create
        (
            mockId,
            "TestName",
            new Money(123, Currency.Rub),
            123,
            true,
            [], []
        );

        _factory.RoomRepository
            .Setup(repo => repo.GetAsync(It.IsAny<RoomId>(), default))
            .Returns((RoomId id, CancellationToken _) =>
                Task.FromResult(id == mockId ? mockRoom : null)
            );

        var response = await _client.GetAsync($"rooms/{mockId.Value}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var roomResponse = JsonSerializer.Deserialize<RoomResponse?>
        (
            await response.Content.ReadAsStringAsync(),
            JsonParseOptions
        );
        
        Assert.NotNull(roomResponse);
        Assert.Equal(mockRoom.Name, roomResponse.Name);
        Assert.Equal((string)mockId!, roomResponse.Id);
        Assert.Equal(mockRoom.Budget.Amount, roomResponse.BudgetAmount);
    }

    [Fact]
    private async Task GetById_NonExistingId_Returns404NotFound()
    {
        _factory.RoomRepository
            .Setup(repo => repo.GetAsync(It.IsAny<RoomId>(), default))
            .Returns((RoomId _, CancellationToken _) => Task.FromResult<Room?>(null));

        var response = await _client.GetAsync($"rooms/{RoomId.CreateUnique().Value}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [InlineData(1000, true, 3, 300)]
    [InlineData(1000012, true, 42.1, 3023.1)]
    [InlineData(10012, true, 2.41, 30453.1)]
    [InlineData(10123.412, false, 23.3, 3.123)]
    [InlineData(103233.412, false, 23.312, 33.3)]
    [InlineData(903231.3412, false, 31.032, 1.2345)]
    [Theory]
    private async Task AddShopItemToRoom_BudgetShouldChangeCorrectly(
        decimal initialBudget, bool requiresRounding, decimal quantity, decimal price)
    {
        var mockRoomId = RoomId.CreateUnique();
        var mockShopItemId = ShopItemId.CreateUnique();
        var mockCurrency = Currency.Rub;
        var mockRoom = Room.Create
        (
            mockRoomId,
            "TestName",
            new Money(initialBudget, mockCurrency),
            123,
            requiresRounding,
            [], []
        );

        _factory.RoomRepository
            .Setup(repo => repo.GetAsync(It.IsAny<RoomId>(), default))
            .Returns((RoomId id, CancellationToken _) =>
                Task.FromResult(id == mockRoomId ? mockRoom : null)
            );

        AddShopItemToRoomRequest request = new 
        (
            mockShopItemId!,
            quantity,
            price,
            mockCurrency.ToString()
        );

        var response = await _client.PostAsync
        (
            $"rooms/{mockRoomId.Value}/shop-item",
            JsonContent.Create(request)
        );

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Assert.Contains(mockRoom.OwnedShopItems, item => item.ShopItemId == mockShopItemId);
        decimal expectedBudgetAmount = initialBudget;
        expectedBudgetAmount -= requiresRounding
            ? Math.Ceiling(quantity * price)
            : quantity * price;
        Assert.Equal(expectedBudgetAmount, mockRoom.Budget.Amount);
    }

    [Fact]
    private async Task AddShopItemToRoom_NonExistingRoomId_Returns404NotFount()
    {
        _factory.RoomRepository
            .Setup(repo => repo.GetAsync(It.IsAny<RoomId>(), default))
            .Returns((RoomId _, CancellationToken _) => Task.FromResult<Room?>(null));

        AddShopItemToRoomRequest request = new 
        (
            ShopItemId.CreateUnique()!,
            123,
            321,
            Currency.Eur.ToString()
        );

        var response = await _client.PostAsync
        (
            $"rooms/{RoomId.CreateUnique().Value}/shop-item",
            JsonContent.Create(request)
        );
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    private async Task AddShopItemToRoom_MismatchedCurrency_Returns400BadRequest()
    {
        var mockRoomId = RoomId.CreateUnique();
        var mockPriceCurrency = Currency.Eur.ToString();
        var mockRoomBudgetCurrency = Currency.Rub;
        var mockRoom = Room.Create(
            mockRoomId,
            "TestName",
            new Money(1234, mockRoomBudgetCurrency),
            123,
            false,
            [], []
        );

        _factory.RoomRepository
            .Setup(repo => repo.GetAsync(It.IsAny<RoomId>(), default))
            .Returns((RoomId id, CancellationToken _) =>
                Task.FromResult(id == mockRoomId ? mockRoom : null)
            );

        var request = new AddShopItemToRoomRequest(
            ShopItemId.CreateUnique()!,
            12,
            21,
            mockPriceCurrency
        );

        var response = await _client.PostAsync(
            $"rooms/{mockRoomId.Value}/shop-item",
            JsonContent.Create(request)
        );

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [InlineData(1000, true, 3, 300)]
    [InlineData(1000012, true, 42.1, 3023.1)]
    [InlineData(10012, true, 2.41, 30453.1)]
    [InlineData(10123.412, false, 23.3, 3.123)]
    [InlineData(103233.412, false, 23.312, 33.3)]
    [InlineData(903231.3412, false, 31.032, 1.2345)]
    [Theory]
    private async Task AddReceiptToRoom_BudgetShouldChangeCorrectly(
        decimal initialBudget, bool requiresRounding, decimal quantity, decimal price)
    {
        var mockRoomId = RoomId.CreateUnique();
        var mockReceiptId = ReceiptId.CreateUnique();
        var mockShopItemId = ShopItemId.CreateUnique();
        
        var mockRoom = Room.Create
        (
            mockRoomId,
            "TestName",
            new Money(initialBudget, Currency.Rub),
            123,
            requiresRounding,
            [], []
        );

        var mockReceipt = Receipt.Create
        (
            mockReceiptId,
            [
                new("TestItem1", new Money(price, Currency.Rub), quantity, mockShopItemId),
                new("TestItem2", new Money(price, Currency.Rub), quantity, mockShopItemId),
                new("TestItem3", new Money(price, Currency.Rub), quantity)
            ],
            null,
            UserId.CreateUnique()
        );

        _factory.RoomRepository
            .Setup(repo => repo.GetAsync(It.IsAny<RoomId>(), default))
            .Returns((RoomId id, CancellationToken _) =>
                Task.FromResult(id == mockRoomId ? mockRoom : null)
            );

        _factory.ReceiptRepository
            .Setup(repo => repo.GetAsync(It.IsAny<ReceiptId>(), default))
            .Returns((ReceiptId id, CancellationToken _) => 
                Task.FromResult(id == mockReceiptId ? mockReceipt : null)
            );

        var request = new AddReceiptToRoomRequest(mockReceiptId!, [1]);

        var response = await _client.PostAsync(
            $"rooms/{mockRoomId.Value}/receipt",
            JsonContent.Create(request)
        );

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Assert.Contains(mockRoom.OwnedShopItems, item => item.ShopItemId == mockShopItemId);
        decimal expectedBudgetAmount = initialBudget;
        expectedBudgetAmount -= requiresRounding
            ? Math.Ceiling(quantity * price)
            : quantity * price;
        Assert.Equal(expectedBudgetAmount, mockRoom.Budget.Amount);
    }
    
    [Fact]
    private async Task AddReceiptToRoom_NonExistingRoomId_Returns404NotFount()
    {
        _factory.RoomRepository
            .Setup(repo => repo.GetAsync(It.IsAny<RoomId>(), default))
            .Returns((RoomId _, CancellationToken _) => Task.FromResult<Room?>(null));

        AddReceiptToRoomRequest request = new(ReceiptId.CreateUnique()!, []);

        var response = await _client.PostAsync
        (
            $"rooms/{RoomId.CreateUnique().Value}/receipt",
            JsonContent.Create(request)
        );
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    private async Task AddReceiptToRoom_MismatchedCurrency_Returns400BadRequest()
    {
        var mockRoomId = RoomId.CreateUnique();
        var mockReceiptId = ReceiptId.CreateUnique();
        var mockPriceCurrency = Currency.Eur;
        var mockRoomBudgetCurrency = Currency.Rub;
        var mockRoom = Room.Create
        (
            mockRoomId,
            "TestName",
            new Money(1234, mockRoomBudgetCurrency),
            123,
            false,
            [], []
        );
        var mockReceipt = Receipt.Create
        (
            mockReceiptId,
            [new("TestItem", new Money(123, mockPriceCurrency), 321, ShopItemId.CreateUnique())],
            null,
            UserId.CreateUnique()
        );

        _factory.RoomRepository
            .Setup(repo => repo.GetAsync(It.IsAny<RoomId>(), default))
            .Returns((RoomId id, CancellationToken _) =>
                Task.FromResult(id == mockRoomId ? mockRoom : null)
            );

        _factory.ReceiptRepository
            .Setup(repo => repo.GetAsync(It.IsAny<ReceiptId>(), default))
            .Returns((ReceiptId id, CancellationToken _) =>
                Task.FromResult(id == mockReceiptId ? mockReceipt : null)
            );

        var request = new AddReceiptToRoomRequest(mockReceiptId!, []);

        var response = await _client.PostAsync(
            $"rooms/{mockRoomId.Value}/receipt",
            JsonContent.Create(request)
        );

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    private async Task AddUserToRoom_UserShouldBeAdded()
    {
        var mockRoomId = RoomId.CreateUnique();
        var mockUserId = UserId.CreateUnique();

        var mockRoom = Room.Create
        (
            mockRoomId,
            "TestName",
            new Money(1234, Currency.Usd),
            123,
            false,
            [], []
        );

        var mockUser = User.Create
        (
            mockUserId,
            "Daniel",
            UserRole.Admin,
            null,
            []
        );

        _factory.RoomRepository
            .Setup(repo => repo.GetAsync(It.IsAny<RoomId>(), default))
            .Returns((RoomId id, CancellationToken _) =>
                Task.FromResult(id == mockRoomId ? mockRoom : null)
            );

        _factory.UserRepository
            .Setup(repo => repo.GetAsync(It.IsAny<UserId>(), default))
            .Returns((UserId id, CancellationToken _) => 
                Task.FromResult(id == mockUserId ? mockUser : null)
            );
        
        AddUserToRoomRequest request = new(mockUserId!);

        var response = await _client.PostAsync($"rooms/{mockRoomId.Value}/user", JsonContent.Create(request));
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        Assert.Equal(mockRoomId, mockUser.RoomId);
        Assert.Contains(mockRoom.UserIds, userId => userId == mockUserId);
    }
    
    //TODO finish all integration tests for receipts controller
    
    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}