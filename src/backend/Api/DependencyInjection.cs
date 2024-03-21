using Api.Common.Errors;
using Microsoft.AspNetCore.Mvc.Infrastructure;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services
            .AddSingleton<ProblemDetailsFactory, RoomWroomProblemDetailsFactory>()
            .AddControllers();

        return services;
    }
}