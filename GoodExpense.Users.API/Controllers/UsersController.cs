using GoodExpense.Common.Domain;
using GoodExpense.Users.Domain.Commands;
using GoodExpense.Users.Domain.Dto;
using GoodExpense.Users.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoodExpense.Users.API.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IMediator _mediator;

    public UsersController(ILogger<UsersController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get user")]
    [ProducesResponseType<ApiResult<GetUserDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser(int id)
    {
        GetUserDto? result = await _mediator.Send(new GetUserQuery(id));
        string? message = result == null ? "User not found" : null;
        
        return Ok(new ApiResult<GetUserDto>(result, message));
    }

    [HttpPost("register")]
    [EndpointSummary("Register user")]
    [ProducesResponseType<ApiResult<bool>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        (string Message, bool IsSuccess) result = await _mediator.Send(new CreateUserCommand
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            Password = registerDto.Password,
        });
        
        return Ok(new ApiResult<bool>
        {
            Data = result.IsSuccess,
            Message = result.Message,
        });
    }
}