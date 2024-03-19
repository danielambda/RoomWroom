using Application.Common.Interfaces.Perception;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Api.IntegrationTests;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IRoomRepository> RoomRepository { get; } = new();
    public Mock<IReceiptRepository> ReceiptRepository { get; } = new();
    public Mock<IUserRepository> UserRepository { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<IRoomRepository>()
                .RemoveAll<IReceiptRepository>()
                .RemoveAll<IUserRepository>();
            
            services
                .AddSingleton(RoomRepository.Object)
                .AddSingleton(ReceiptRepository.Object)
                .AddSingleton(UserRepository.Object);
        });
    }
}