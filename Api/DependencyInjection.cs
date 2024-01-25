using Api.Common.Mapping;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services
            .AddMappings()
            .AddControllers();

        return services;
    }

    private static IServiceCollection AddMappings(this IServiceCollection services)
    {
        services.AddScoped<IMapper, MapperlyMapper>();

        return services;
    }
}