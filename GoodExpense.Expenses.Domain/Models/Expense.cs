using GoodExpense.Common.Domain;

namespace GoodExpense.Expenses.Domain.Models;

public class Expense : BaseTrackingEntity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required decimal Amount { get; set; }
    public required int AuthorId { get; set; }
    public ICollection<ExpenseUser> ExpenseUsers { get; set; } = [];
}