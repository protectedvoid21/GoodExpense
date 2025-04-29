using GoodExpense.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace GoodExpense.Users.Application;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
}