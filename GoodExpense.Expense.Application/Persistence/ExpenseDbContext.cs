using Microsoft.EntityFrameworkCore;

namespace GoodExpense.Expense.Application.Persistence;

public class ExpenseDbContext : DbContext
{
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options) { }
}