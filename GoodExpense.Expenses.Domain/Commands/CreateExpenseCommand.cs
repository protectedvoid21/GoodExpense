using MediatR;

namespace GoodExpense.Expenses.Domain.Commands;

public class CreateExpenseCommand : IRequest<bool>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int AuthorId { get; set; }
    public required IEnumerable<ExpenseParticipant> Participants { get; set; }
}

public class ExpenseParticipant
{
    public required int UserId { get; set; }
    public required decimal Amount { get; set; }
}