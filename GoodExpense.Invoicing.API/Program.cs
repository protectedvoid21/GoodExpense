using GoodExpense.Common.Application;
using GoodExpense.Common.Domain.Bus;
using GoodExpense.Invoicing.Application;
using GoodExpense.Invoicing.Domain;
using GoodExpense.Invoicing.Domain.Events;
using GoodExpense.Invoicing.Domain.Handlers;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSerilog(logger =>
{
    logger.ReadFrom.Configuration(builder.Configuration);
});
builder.Services.AddSingleton<IEventBus, RabbitMqBus>();
builder.Services.AddScoped<IInvoiceGenerator, PdfInvoiceGenerator>();

builder.Services.AddScoped<GenerateInvoiceHandler>();

var app = builder.Build();

var eventBus = app.Services.GetRequiredService<IEventBus>();
await eventBus.Subscribe<GenerateInvoiceEvent, GenerateInvoiceHandler>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(config =>
    {
        config.Title = "GE Invoicing API";
    });
}

app.MapPost("/invoicing/", async (
    [FromServices] GenerateInvoiceHandler invoiceHandler, 
    [FromBody] CreateInvoiceRequest request) =>
{
    await invoiceHandler.Handle(new GenerateInvoiceEvent
    {
        CreateInvoiceRequest = request,
    });
}).WithSummary("Generate invoice");

app.UseSerilogRequestLogging();
app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

app.UseHttpsRedirection();

app.Run();