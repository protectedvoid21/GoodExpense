using GoodExpense.Common.Domain;
using GoodExpense.Common.Domain.Bus;
using GoodExpense.Expenses.Domain;
using GoodExpense.Expenses.Domain.Commands;
using GoodExpense.Expenses.Domain.Models;
using MediatR;

namespace GoodExpense.Expenses.Application.Handlers;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, bool>
{
    private readonly ExpenseDbContext _dbContext;
    private readonly IGoodExpenseClient _client;

    public CreateExpenseCommandHandler(ExpenseDbContext dbContext, IGoodExpenseClient client)
    {
        _dbContext = dbContext;
        _client = client;
    }

    public async Task<bool> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        
        decimal amount = request.Participants.Sum(p => p.Amount);
        
        var expense = new Expense
        {
            Title = request.Title,
            Description = request.Description,
            AuthorId = request.AuthorId,
            Amount = amount,
        };
        _dbContext.Add(expense);
        await _dbContext.SaveChangesAsync(cancellationToken);
         
        return true;
    }
}