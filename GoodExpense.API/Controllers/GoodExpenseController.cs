using GoodExpense.Domain.Clients;
using Microsoft.AspNetCore.Mvc;

namespace GoodExpense.API.Controllers;

[ApiController]
[Route("good-expense/")]
public class GoodExpenseController : ControllerBase
{
    private readonly IUsersServiceClient _usersServiceClient;

    public GoodExpenseController(IUsersServiceClient usersServiceClient)
    {
        _usersServiceClient = usersServiceClient;
    }

    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUser([FromRoute] int id)
    {
        var user = await _usersServiceClient.GetUserAsync(id);
        return Ok(user);
    }
}