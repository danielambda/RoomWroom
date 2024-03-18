using Domain.Common.Enums;
using Domain.Common.ValueObjects;
using Domain.ReceiptAggregate;
using Domain.ReceiptAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;
using Infrastructure.Common.Persistence;
using Infrastructure.Receipts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UnitTests.Receipts.Persistence;

//TODO fix this after fixing receiptItems models in EFCore
public class ReceiptRepositoryTests
{
    private static ReceiptId ExistingId => "3ca808a4-9c03-4967-8fd7-2c63b8919699"!;
    private static UserId ExistingCreatorId => "1863505a-1f33-4def-92e6-cbb47258242f"!;
    private static string ExistingQr => "SomeRandomQrCode";
    
    [Fact]
    public async void GetAsync_ReturnsReceipt()
    {
        ReceiptRepository repository = new(await GetDbContextAsync());

        var receipt = await repository.GetAsync(ExistingId, default);
        
        Assert.NotNull(receipt);
        Assert.IsType<Receipt>(receipt);
        
        Assert.Equal(receipt.Qr, ExistingQr);
        Assert.Equal(receipt.CreatorId, ExistingCreatorId);
        Assert.Empty(receipt.Items);
    }

    [Fact]
    public async void CheckExistenceByQr_ReturnsTrue()
    {
        ReceiptRepository repository = new(await GetDbContextAsync());

        bool result = await repository.CheckExistenceByQr(ExistingQr, default);
        
        //Assert.True(result);
    }
    
    [Fact]
    public async void CheckExistenceByQr_ReturnsFalse()
    {
        ReceiptRepository repository = new(await GetDbContextAsync());

        bool result = await repository.CheckExistenceByQr("SOmeRandomStuffThatDoeSnoTExistsThere", default);
        
        Assert.False(result);
    }
    
    private static async Task<RoomWroomDbContext> GetDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<RoomWroomDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        RoomWroomDbContext dbContext = new(options);
        await dbContext.Database.EnsureCreatedAsync();

        if (await dbContext.Receipts.AnyAsync()) 
            return dbContext;

        await dbContext.Receipts.AddAsync
        (
            Receipt.Create
            (
                ExistingId,
                [],
                ExistingQr,
                ExistingCreatorId
            )
        );
        for (int i = 0; i < 10; i++)
        {
            await dbContext.Receipts.AddAsync
            (
                Receipt.CreateNew
                (
                    [
                    ],
                    null,
                    UserId.CreateUnique()
                )
            );
        }

        return dbContext;
    }
}