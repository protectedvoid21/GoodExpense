using GoodExpense.Common.Application;
using GoodExpense.Common.Domain.Bus;
using GoodExpense.Domain.Clients;
using Microsoft.OpenApi.Models;
using Refit;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSerilog(logger =>
{
    logger.ReadFrom.Configuration(builder.Configuration);
});

builder.Services
    .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly))
    .AddSingleton<IEventBus, RabbitMqBus>();

builder.Services.Configure<EventBusConfiguration>(builder.Configuration.GetSection("EventBus"));

builder.Services.AddRefitClient<IUsersServiceClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ExternalServicesUrls:Users"]!));

builder.Services.AddRefitClient<IExpensesServiceClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ExternalServicesUrls:Expenses"]!));

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(config =>
{
    config.Title = "GE Main API";
    config.Servers = [];
});

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

app.Run();