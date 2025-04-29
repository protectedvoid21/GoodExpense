using GoodExpense.Common.Application;
using GoodExpense.Common.Domain.Bus;
using GoodExpense.Common.Domain.Extensions;
using GoodExpense.Expenses.Application;
using GoodExpense.Expenses.Application.Handlers;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSerilog(logger => { logger.ReadFrom.Configuration(builder.Configuration); });

builder.Services.AddNpgsql<ExpenseDbContext>(
    builder.Configuration.GetConnectionString("DbConnection"),
    _ => { },
    options =>
    {
        options.UseSnakeCaseNamingConvention();
    }
);

builder.Services.AddGoodExpenseClient();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CreateExpenseCommandHandler).Assembly));
builder.Services.AddSingleton<IEventBus, RabbitMqBus>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(config =>
    {
        config.Title = "GE Expenses API";
    });
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/scalar"));

app.Run();