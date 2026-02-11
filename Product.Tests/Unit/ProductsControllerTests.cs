   using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Product.Application.Interfaces;
    using Product.Core.Entities;
    using Xunit;

namespace Product.Tests.Unit
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _controller = new ProductsController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Products>
        {
            new() { Id = 1, Name = "Laptop", Price = 1200m, Stock = 10 },
            new() { Id = 2, Name = "Mouse",  Price = 25m,   Stock = 50 }
        };

            _mockRepo.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(products);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dtos = Assert.IsType<List<ProductsDto>>(okResult.Value);

            Assert.Equal(2, dtos.Count);
            Assert.Equal("Laptop", dtos[0].Name);
        }

        [Fact]
        public async Task Get_ExistingId_ReturnsProduct()
        {
            // Arrange
            var product = new Products { Id = 42, Name = "Keyboard", Price = 89m, Stock = 15 };
            _mockRepo.Setup(r => r.GetByIdAsync(42)).ReturnsAsync(product);

            // Act
            var result = await _controller.Get(42);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<ProductsDto>(okResult.Value);

            Assert.Equal(42, dto.Id);
            Assert.Equal("Keyboard", dto.Name);
        }

        [Fact]
        public async Task Get_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Products?)null);

            // Act
            var result = await _controller.Get(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidData_ReturnsCreated()
        {
            // Arrange
            var createDto = new CreateProductsDto("Monitor", 299m, 8);

            var savedProduct = new Products
            {
                Id = 100,
                Name = createDto.Name,
                Price = createDto.Price,
                Stock = createDto.Stock
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Products>()))
                     .ReturnsAsync(savedProduct);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(ProductsController.Get), createdResult.ActionName);
            Assert.Equal(100, createdResult.RouteValues!["id"]);

            var returnedDto = Assert.IsType<ProductsDto>(createdResult.Value);
            Assert.Equal("Monitor", returnedDto.Name);
            Assert.Equal(299m, returnedDto.Price);
        }

        [Theory]
        [InlineData("", 100, 5)]           // empty name
        [InlineData("TV", -50, 10)]         // negative price
        [InlineData("Phone", 500, -3)]      // negative stock
        public async Task Create_InvalidData_ReturnsBadRequest(string name, decimal price, int stock)
        {
            // Arrange
            var invalidDto = new CreateProductsDto(name, price, stock);

            // Act
            var result = await _controller.Create(invalidDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Update_ExistingProduct_ReturnsNoContent()
        {
            // Arrange
            var updateDto = new UpdateProductsDto("New Chair", null, 15);

            _mockRepo.Setup(r => r.GetByIdAsync(5))
                     .ReturnsAsync(new Products { Id = 5, Name = "Old Chair", Price = 45m, Stock = 20 });

            _mockRepo.Setup(r => r.UpdateAsync(5, updateDto))
                     .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(5, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);

            _mockRepo.Verify(r => r.UpdateAsync(5, updateDto), Times.Once());
        }
        [Fact]
        public async Task Update_NonExisting_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Products?)null);

            // Act
            var result = await _controller.Update(999, new UpdateProductsDto("Test", 10m, 1));

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Existing_ReturnsNoContent()
        {
            // Arrange
            var product = new Products { Id = 7, Name = "Lamp" };
            _mockRepo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(product);
            _mockRepo.Setup(r => r.DeleteAsync(product.Id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(7);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_NonExisting_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Products?)null);

            // Act
            var result = await _controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
