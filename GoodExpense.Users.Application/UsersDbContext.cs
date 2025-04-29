using GoodExpense.Common.Domain;
using GoodExpense.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace GoodExpense.Users.Application;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    
    private void UpdateBaseTrackingEntities()
    {
        var entries = ChangeTracker.Entries<BaseTrackingEntity>().ToList();
        var now = DateTime.UtcNow;
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    break;
            }
        }
    }
    
    public override int SaveChanges()
    {
        UpdateBaseTrackingEntities();
        
        return base.SaveChanges();
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateBaseTrackingEntities();

        return base.SaveChangesAsync(cancellationToken);
    }
}