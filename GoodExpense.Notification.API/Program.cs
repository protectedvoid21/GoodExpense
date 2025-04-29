using GoodExpense.Common.Application;
using GoodExpense.Common.Domain.Bus;
using GoodExpense.Notification.Application;
using GoodExpense.Notification.Application.Handlers;
using GoodExpense.Notification.Domain;
using GoodExpense.Notification.Domain.Events;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSerilog(logger =>
{
    logger.ReadFrom.Configuration(builder.Configuration);
});
builder.Services.AddScoped<INotificationService, AzureEmailNotificationService>();
builder.Services.Configure<AzureNotificationConfiguration>(builder.Configuration.GetSection("AzureNotification"));

builder.Services.AddSingleton<IEventBus, RabbitMqBus>();
builder.Services.AddScoped<NotifyEventHandler>();

var app = builder.Build();

var eventBus = app.Services.GetRequiredService<IEventBus>();
await eventBus.Subscribe<NotifyEvent, NotifyEventHandler>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(config =>
    {
        config.Title = "GE Notifications API";
    });
}

app.UseHttpsRedirection();

app.MapPost("/", async (
        [FromServices] INotificationService notificationService, 
        [FromBody] NotifyRequest request
    ) =>
    {
        await notificationService.SendNotificationAsync(request);
    })
    .WithName("Send email")
    .WithDescription("Use for testing purposes only.");

app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

app.Run();