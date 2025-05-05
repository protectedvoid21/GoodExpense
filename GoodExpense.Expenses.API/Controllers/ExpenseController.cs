using GoodExpense.Common.Domain;
using GoodExpense.Expenses.Domain.Commands;
using GoodExpense.Expenses.Domain.Dto;
using GoodExpense.Expenses.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoodExpense.Expenses.API.Controllers;

[ApiController]
[Route("expense")]
public class ExpenseController : ControllerBase
{
    private readonly ILogger<ExpenseController> _logger;
    private readonly IMediator _mediator;

    public ExpenseController(ILogger<ExpenseController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType<GetExpenseDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpense(int id)
    {
        GetExpenseDto? expense = await _mediator.Send(new GetExpenseQuery { Id = id });
        return Ok(expense);
    }
    
    [HttpPost]
    [ProducesResponseType<ApiResult>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto createExpenseDto)
    {
        try
        {
            bool isExpenseCreatedSuccessfully = await _mediator.Send(new CreateExpenseCommand
            {
                Description = createExpenseDto.Description,
                Title = createExpenseDto.Title,
                AuthorId = createExpenseDto.AuthorId,
                Participants = createExpenseDto.Participants.Select(p => new ExpenseParticipant
                {
                    UserId = p.UserId,
                    Amount = p.Amount,
                }),
            });
            return Ok(new ApiResult<bool>(isExpenseCreatedSuccessfully));
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("Validation failed for expense creation. Message: {Message}", ex.Message);
            return BadRequest(new ApiResult { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError("Unhandled exception during expense creation. Message: {Message}", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResult { Message = "An error occurred while creating the expense." });
        }
    }
}