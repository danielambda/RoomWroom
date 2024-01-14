using System.Reflection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Api.Common.Errors;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services
            .AddMappings(Assembly.GetExecutingAssembly())
            .AddSingleton<ProblemDetailsFactory, RoomWroomProblemDetailsFactory>()
            .AddControllers();

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