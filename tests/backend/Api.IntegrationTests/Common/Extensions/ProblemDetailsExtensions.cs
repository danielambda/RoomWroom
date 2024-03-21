using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Api.IntegrationTests.Common.Extensions;

public static class ProblemDetailsExtensions
{
    public static List<string> ErrorCodes(this ProblemDetails problemDetails) 
        => ((JsonElement?)problemDetails.Extensions["errorCodes"])?
            .Deserialize<List<string>>() ?? [];
}