using GoodExpense.Common.Domain;
using GoodExpense.Common.Domain.Dto;
using GoodExpense.Domain.Clients;
using Microsoft.AspNetCore.Mvc;
using Refit;
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
    [EndpointSummary("Get user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser([FromRoute] int id)
    {
        var user = await _usersServiceClient.GetUserAsync(id);
        return Ok(user);
    }
    
    [HttpPost("users/register")]
    [EndpointSummary("Register user")]
    [ProducesResponseType<ApiResult<bool>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        var result = await _usersServiceClient.RegisterUserAsync(registerDto);
        return Ok(result);
    }
    
    [HttpGet("expenses/{id}")]
    [EndpointSummary("Get expense")]
    [ProducesResponseType<GetExpenseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpense([FromRoute] int id)
    {
        var expense = await _usersServiceClient.GetUserAsync(id);
        return Ok(expense);
    }
    
    [HttpPost("expenses")]
    [EndpointSummary("Create expense")]
    [ProducesResponseType<ApiResult<bool>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto createExpenseDto)
    {
        try
        {
            ApiResult<bool> result = await _expensesServiceClient.CreateExpenseAsync(createExpenseDto);
            return Ok(result);
        }
        catch (ApiException ex)
        {
            return BadRequest(ex.Content);
        }
    }
}