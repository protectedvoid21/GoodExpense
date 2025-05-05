using GoodExpense.Common.Application;
using GoodExpense.Common.Domain.Bus;
using GoodExpense.Common.Domain.Extensions;
using GoodExpense.Users.Application;
using GoodExpense.Users.Application.CommandHandlers;
using GoodExpense.Users.Application.Services;
using GoodExpense.Users.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSerilog(logger => { logger.ReadFrom.Configuration(builder.Configuration); });
builder.Services
    .AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly); })
    .AddSingleton<IEventBus, RabbitMqBus>();

builder.Services.AddScoped<IPasswordEncrypter, PasswordSHA256Encrypter>();

builder.Services.AddNpgsql<UsersDbContext>(
    builder.Configuration.GetConnectionString("DbConnection"),
    _ => { },
    options =>
    {
        options.UseSnakeCaseNamingConvention();
    }
);

builder.Services.Configure<EventBusConfiguration>(builder.Configuration.GetSection("EventBus"));
builder.Services.AddGoodExpenseClient(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(config =>
    {
        config.Title = "GE Users API";
    });
}
app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();