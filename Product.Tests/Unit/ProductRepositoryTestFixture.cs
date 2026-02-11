using Microsoft.EntityFrameworkCore;
using Product.Core.Entities;
using Product.Infrastructure.Data;  

namespace Product.Tests.Unit.Infrastructure;

public class ProductRepositoryTestFixture : IDisposable
{
    private readonly DbContextOptions<ProductDbContext> _options;
    public ProductDbContext Context { get; }

    public ProductRepositoryTestFixture()
    {
        var databaseName = Guid.NewGuid().ToString(); // unique per test run

        _options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName)  // ya .UseSqlite("Data Source=:memory:")
            .Options;

        Context = new ProductDbContext(_options);

        
        SeedData();
    }

    private void SeedData()
    {
        if (!Context.Products.Any())
        {
            Context.Products.AddRange(
                new Products { Id = 1, Name = "Laptop", Price = 1200m, Stock = 10 },
                new Products { Id = 2, Name = "Mouse", Price = 25m, Stock = 50 },
                new Products { Id = 3, Name = "Keyboard", Price = 89m, Stock = 15 }
            );
            Context.SaveChanges();
        }
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}
