using Product.Application.Interfaces;
using Product.Core.Entities;
using Product.Infrastructure.Implementation;
using Xunit;

namespace Product.Tests.Unit.Infrastructure;

public class ProductRepositoryTests : IClassFixture<ProductRepositoryTestFixture>
{
    private readonly ProductRepositoryTestFixture _fixture;
    private readonly IProductRepository _repository;

    public ProductRepositoryTests(ProductRepositoryTestFixture fixture)
    {
        _fixture = fixture;
        _repository = new ProductRepository(_fixture.Context);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsProduct()
    {
        // Act
        var result = await _repository.GetByIdAsync(2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Id);
        Assert.Equal("Mouse", result.Name);
        Assert.Equal(25m, result.Price);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllActiveProducts()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Contains(result, p => p.Name == "Laptop");
    }

    [Fact]
    public async Task AddAsync_AddsProductAndSetsId()
    {
        // Arrange
        var newProduct = new Products { Name = "Monitor", Price = 299m, Stock = 8 };

        // Act
        var added = await _repository.AddAsync(newProduct);

        // Assert
        Assert.NotEqual(0, added.Id);           // Id auto-generated
        Assert.Equal("Monitor", added.Name);

        var fromDb = await _fixture.Context.Products.FindAsync(added.Id);
        Assert.NotNull(fromDb);
        Assert.Equal(299m, fromDb.Price);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingProduct()
    {
        // Arrange – create fresh entity
        var product = new Products
        {
            Id = 100,
            Name = "Old Laptop",
            Price = 1200m,
            Stock = 10
        };

        _fixture.Context.Products.Add(product);
        await _fixture.Context.SaveChangesAsync();

        var updateDto = new UpdateProductsDto("Old Laptop", 1200m, 10);

        // Act
        await _repository.UpdateAsync(100, updateDto);

        // Assert
        var updated = await _repository.GetByIdAsync(100);
        Assert.NotNull(updated);
        Assert.Equal("Old Laptop", updated.Name);
        Assert.Equal(1200m, updated.Price);
        Assert.Equal(10, updated.Stock);
    }

    [Fact]
    public async Task DeleteAsync_RemovesProduct()
    {
        // Arrange
        var product = await _repository.GetByIdAsync(3);
        Assert.NotNull(product);

        // Act
        await _repository.DeleteAsync(product.Id);

        // Assert
        var deleted = await _repository.GetByIdAsync(3);
        Assert.Null(deleted);
    }


    [Fact]
    public async Task ExistsAsync_ExistingId_ReturnsTrue()
    {
        // Act + Assert
        Assert.True(await _repository.ExistsAsync(1));
        Assert.False(await _repository.ExistsAsync(999));
    }
}