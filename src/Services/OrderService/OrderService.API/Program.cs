var builder = WebApplication.CreateBuilder(args);

builder.AddMicroserviceRegistration();

var services = builder.Services;
var configuration = builder.Configuration;
services.AddLogging(builder =>
{
    builder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None)
        .AddConsole();
});

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services
    .AddBehaviors()
    .AddEventBusEventHandlers()
    .AddApplicationRegistration()
    .AddIntegrationServices(configuration)
    .AddInfrastructureRegistration(configuration);

var app = builder.Build();

app.Services.AddEventBusSubcribes();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
