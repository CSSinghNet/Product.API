using System;
using System.Collections.Generic;
using System.Text;

namespace Product.Core.Entities
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    // DTOs
    public record ProductsDto(int Id, string Name, decimal Price, int Stock);
    public record CreateProductsDto(string Name, decimal Price, int Stock);
    public record UpdateProductsDto(string? Name, decimal? Price, int? Stock);
}
