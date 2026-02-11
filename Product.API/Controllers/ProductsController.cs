using Microsoft.AspNetCore.Mvc;
using Product.Application.Interfaces;
using Product.Core.Entities;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductsDto>>> GetAll()
    {
        var products = await _repository.GetAllAsync();
        return Ok(products.Select(p => new ProductsDto(p.Id, p.Name, p.Price, p.Stock)).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductsDto>> Get(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null) return NotFound();
        return Ok(new ProductsDto(product.Id, product.Name, product.Price, product.Stock));
    }

    [HttpPost]
    public async Task<ActionResult<ProductsDto>> Create([FromBody] CreateProductsDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || dto.Price < 0 || dto.Stock < 0)
            return BadRequest("Invalid product data");

        var product = new Products
        {
            Name = dto.Name,
            Price = dto.Price,
            Stock = dto.Stock
        };

        var created = await _repository.AddAsync(product);

        return CreatedAtAction(nameof(Get), new { id = created.Id },
            new ProductsDto(created.Id, created.Name, created.Price, created.Stock));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductsDto dto)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null) return NotFound();

        if (dto.Name is not null) product.Name = dto.Name;
        if (dto.Price.HasValue) product.Price = dto.Price.Value;
        if (dto.Stock.HasValue) product.Stock = dto.Stock.Value;

        await _repository.UpdateAsync(product.Id,dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product is null) return NotFound();

        await _repository.DeleteAsync(product.Id);
        return NoContent();
    }
}