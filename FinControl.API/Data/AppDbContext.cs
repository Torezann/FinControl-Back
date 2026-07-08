using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinControl.API.Models;

namespace FinControl.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<BudgetLimit> BudgetLimits { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<BudgetLimit>()
            .HasIndex(b => new { b.UserId, b.Categoria })
            .IsUnique();

        builder.Entity<Category>()
            .HasIndex(c => new { c.UserId, c.Nome })
            .IsUnique();
    }
}
