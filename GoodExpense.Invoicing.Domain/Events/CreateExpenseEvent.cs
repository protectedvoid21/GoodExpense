using GoodExpense.Common.Domain.Events;

namespace GoodExpense.Invoicing.Domain.Events;

public record CreateExpenseEvent : Event
{
    public required CreateInvoiceRequest Expense { get; set; }
}

public record ExpenseInfo
{
    public required string Title { get; set; } 
    public required string Description { get; set; }
    public required DateTime CreatedDate { get; set; }
    public required IEnumerable<ExpenseParticipant> ParticipantUsers { get; set; }
}

public record ExpenseParticipant
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required decimal Amount { get; set; }
}