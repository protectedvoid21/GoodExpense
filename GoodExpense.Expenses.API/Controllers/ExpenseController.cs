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
    
    [HttpGet]
    public async Task<IActionResult> GetExpense(int id)
    {
        var expense = await _mediator.Send(new GetExpenseQuery { Id = id });
        return Ok(expense);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto createExpenseDto)
    {
        var result = await _mediator.Send(new CreateExpenseCommand
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
        return Ok(result);
    }
}