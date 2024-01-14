WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure();

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
