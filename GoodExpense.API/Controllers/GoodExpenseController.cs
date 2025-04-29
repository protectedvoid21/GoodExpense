using GoodExpense.Domain.Clients;
using GoodExpense.Users.Domain.Dto;
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
    
    [HttpPost("users/register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        var result = await _usersServiceClient.RegisterUserAsync(registerDto);
        return Ok(result);
    }
}