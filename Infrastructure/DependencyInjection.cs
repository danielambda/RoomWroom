using System.Reflection;
using Application.Shopping.Interfaces;
using Infrastructure.Shopping;
using Infrastructure.Shopping.Repositories;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddMappings(Assembly.GetExecutingAssembly())
            .AddSingleton<IReceiptFromQrCreator, InnReceiptFromQrCreator>()
            .AddSingleton<IReceiptRepository, MemoryReceiptRepository>();

        return services;
    }

    private static IServiceCollection AddMappings(this IServiceCollection services, Assembly assembly)
    {
        TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}