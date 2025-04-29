using GoodExpense.Expenses.Domain.Dto;
using GoodExpense.Expenses.Domain.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodExpense.Expenses.Application.Handlers;

public class GetExpenseQueryHandler : IRequestHandler<GetExpenseQuery, GetExpenseDto?>
{
    private readonly ExpenseDbContext _expenseDbContext;

    public GetExpenseQueryHandler(ExpenseDbContext expenseDbContext)
    {
        _expenseDbContext = expenseDbContext;
    }

    public async Task<GetExpenseDto?> Handle(GetExpenseQuery request, CancellationToken cancellationToken)
    {
        return await _expenseDbContext.Expenses
            .Where(e => e.Id == request.Id)
            .Select(e => new GetExpenseDto
            {
                Id = e.Id,
                Amount = e.Amount,
                Name = e.Title,
                CreatedOn = e.CreatedAt,
            })
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
    }
}