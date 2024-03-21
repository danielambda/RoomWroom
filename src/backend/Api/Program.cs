WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration.GetConnectionString("RoomWroomDb"));

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();


app.Run();
