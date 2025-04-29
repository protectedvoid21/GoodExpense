using GoodExpense.Common.Domain;
using GoodExpense.Common.Domain.Dto;
using Refit;

namespace GoodExpense.Domain.Clients;

public interface IExpensesServiceClient
{
    [Get("/expenses/{id}")]
    Task<GetExpenseDto> GetExpenseAsync(int id);

    [Post("/expense")]
    Task<ApiResult<bool>> CreateExpenseAsync([Body] CreateExpenseDto createExpenseDto);
}