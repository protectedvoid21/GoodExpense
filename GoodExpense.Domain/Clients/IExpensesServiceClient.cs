using GoodExpense.Common.Domain.Dto;
using Refit;

namespace GoodExpense.Domain.Clients;

public interface IExpensesServiceClient
{
    [Get("/expenses/{id}")]
    Task<GetExpenseDto> GetExpense(int id);

    [Post("/expense")]
    Task<bool> CreateExpense([Body] CreateExpenseDto createExpenseDto);
}