using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinControl.API.Models;

namespace FinControl.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityUserContext<ApplicationUser, Guid>(options)
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

        builder.Entity<Transaction>().ToTable("tb_transacoes");
        builder.Entity<Goal>().ToTable("tb_metas");
        builder.Entity<BudgetLimit>().ToTable("tb_limites_orcamento");
        builder.Entity<Category>().ToTable("tb_categorias");

        builder.Entity<ApplicationUser>().ToTable("tb_usuarios");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("tb_usuario_claims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("tb_usuario_tokens");

        builder.Ignore<IdentityUserLogin<Guid>>();
    }
}
