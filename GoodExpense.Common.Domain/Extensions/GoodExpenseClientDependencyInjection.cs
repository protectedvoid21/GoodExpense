using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace GoodExpense.Common.Domain.Extensions;

public static class GoodExpenseClientDependencyInjection
{
    public static IServiceCollection AddGoodExpenseClient(this IServiceCollection services, IConfiguration configuration)
    {
        string baseUrl = configuration.GetRequiredSection("ApiGatewayConfiguration")
            .GetRequiredSection("BaseUrl").Value!;
        
        services.AddRefitClient<IGoodExpenseClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));

        return services;
    }
}