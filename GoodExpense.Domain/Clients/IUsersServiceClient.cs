using GoodExpense.Users.Domain.Dto;
using Refit;

namespace GoodExpense.Domain.Clients;

public interface IUsersServiceClient
{
    [Get("/users/{id}")]
    Task<GetUserDto> GetUserAsync(int id);

    [Post("/users/register")]
    Task<bool> RegisterUserAsync([Body] RegisterUserDto registerDto);
}