using GoodExpense.Common.Domain.Dto;
using Refit;

namespace GoodExpense.Common.Domain;

public interface IGoodExpenseClient
{
    [Get("/users/{id}")]
    Task<ApiResult<GetUserDto?>> GetUserAsync(int id);
}