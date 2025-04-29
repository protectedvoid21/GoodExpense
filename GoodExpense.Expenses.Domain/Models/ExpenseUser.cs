using Microsoft.EntityFrameworkCore;

namespace GoodExpense.Expenses.Domain.Models;

[PrimaryKey(nameof(UserId), nameof(ExpenseId))]
public class ExpenseUser
{
    public required int UserId { get; set; }
    public required int ExpenseId { get; set; }
    public Expense Expense { get; set; }
    public required decimal Amount { get; set; }
}