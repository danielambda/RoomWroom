using Application.Common.Interfaces.Perception;
using Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace Api.IntegrationTests.RoomsController;

public class RoomsWebApplicationFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer =
        new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("RoomWroom")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    
    public Mock<IRoomRepository> RoomRepository { get; } = new();
    public Mock<IReceiptRepository> ReceiptRepository { get; } = new();
    public Mock<IUserRepository> UserRepository { get; } = new();
    public Mock<IShopItemRepository> ShopItemRepository { get; } = new();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services
                //.RemoveAll<IRoomRepository>()
                .RemoveAll<IReceiptRepository>()
                .RemoveAll<IUserRepository>()
                .RemoveAll<IShopItemRepository>();

            services
                //.AddSingleton(RoomRepository.Object)
                .AddSingleton(ReceiptRepository.Object)
                .AddSingleton(UserRepository.Object)
                .AddSingleton(ShopItemRepository.Object);

            services.RemoveAll<DbContextOptions<RoomWroomDbContext>>();
            services.AddDbContext<RoomWroomDbContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));

            services.AddDbContext<RoomWroomDbContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));
        });
    }

    public Task InitializeAsync() => _dbContainer.StartAsync();

    public new Task DisposeAsync() => _dbContainer.StopAsync();
}