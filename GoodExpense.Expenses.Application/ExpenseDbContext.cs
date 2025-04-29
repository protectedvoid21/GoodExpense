using GoodExpense.Expenses.Domain;
using GoodExpense.Expenses.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodExpense.Expenses.Application;

public class ExpenseDbContext : DbContext
{
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options) { }
    
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpenseUser> ExpenseUsers { get; set; }
}