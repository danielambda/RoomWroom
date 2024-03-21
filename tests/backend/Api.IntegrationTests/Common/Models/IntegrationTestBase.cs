using Domain.Common.ValueObjects;
using Domain.RoomAggregate;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.IntegrationTests.Common.Models;

public abstract class IntegrationTestBase : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly RoomWroomDbContext DbContext;
    protected readonly HttpClient Client;
    protected IntegrationTestBase(IntegrationTestWebAppFactory factory)
    {
        Client = factory.CreateClient();
        
        var scope = factory.Services.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<RoomWroomDbContext>();
        DbContext.Database.Migrate();
    }
}