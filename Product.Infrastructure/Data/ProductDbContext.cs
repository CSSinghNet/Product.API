using Microsoft.EntityFrameworkCore;
using Product.Core.Entities;

namespace Product.Infrastructure.Data;

public class ProductDbContext : DbContext
{
    public DbSet<Products> Products => Set<Products>();  // ← singular DbSet, plural table name by convention

    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Fluent configuration (keeps entity clean - no [Key], [Required], etc. in domain)
        modelBuilder.Entity<Products>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(p => p.Price)
                  .HasPrecision(18, 2)
                  .IsRequired();

            entity.Property(p => p.Stock)
                  .IsRequired();

          

           

            // Optional: table name override
            // entity.ToTable("Products", "catalog");  // schema + table
        });

        // You can add more entities here later (Category, Supplier, etc.)
    }

    // Optional: override SaveChanges to add auditing, soft-delete logic, etc.
    public override int SaveChanges()
    {
        // Example: auto-set timestamps, created-by, etc. if you add those properties later
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}