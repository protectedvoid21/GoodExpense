using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace GoodExpense.Common.Domain.Extensions;

public static class GoodExpenseClientDependencyInjection
{
    public static IServiceCollection AddGoodExpenseClient(this IServiceCollection services)
    {
        services.AddRefitClient<IGoodExpenseClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7000/good-expense"));

        return services;
    }
}