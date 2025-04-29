using GoodExpense.Common.Domain;
using GoodExpense.Common.Domain.Dto;
using GoodExpense.Domain.Clients;
using Microsoft.AspNetCore.Mvc;
using RegisterUserDto = GoodExpense.Common.Domain.Dto.RegisterUserDto;

namespace GoodExpense.API.Controllers;

[ApiController]
[Route("good-expense/")]
public class GoodExpenseController : ControllerBase
{
    private readonly IUsersServiceClient _usersServiceClient;
    private readonly IExpensesServiceClient _expensesServiceClient;

    public GoodExpenseController(
        IUsersServiceClient usersServiceClient,
        IExpensesServiceClient expensesServiceClient)
    {
        _usersServiceClient = usersServiceClient;
        _expensesServiceClient = expensesServiceClient;
    }

    [HttpGet("users/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser([FromRoute] int id)
    {
        var user = await _usersServiceClient.GetUserAsync(id);
        return Ok(user);
    }
    
    [HttpPost("users/register")]
    [ProducesResponseType<ApiResult<bool>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        var result = await _usersServiceClient.RegisterUserAsync(registerDto);
        return Ok(result);
    }
    
    [HttpGet("expenses/{id}")]
    [ProducesResponseType<GetExpenseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpense([FromRoute] int id)
    {
        var expense = await _usersServiceClient.GetUserAsync(id);
        return Ok(expense);
    }
}