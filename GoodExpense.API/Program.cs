using GoodExpense.Common.Application;
using GoodExpense.Common.Domain.Bus;
using GoodExpense.Domain.Clients;
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
    .AddSingleton<IEventBus, RabbitMQBus>();

builder.Services.AddRefitClient<IUsersServiceClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["ExternalServicesUrls:Users"]!));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/scalar")).ExcludeFromDescription();

app.Run();