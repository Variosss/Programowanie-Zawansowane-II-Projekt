using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class CurrencyDbContext : DbContext
{
    public CurrencyDbContext(DbContextOptions<CurrencyDbContext> options)
        : base(options)
    {
    }

    public DbSet<CurrencyRate> CurrencyRates => Set<CurrencyRate>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CurrencyRate>()
            .HasIndex(x => new { x.CurrencyCode, x.EffectiveDate })
            .IsUnique();
    }
}
