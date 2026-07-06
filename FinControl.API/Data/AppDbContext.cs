using Microsoft.EntityFrameworkCore;
using FinControl.API.Models;

namespace FinControl.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; }
}
