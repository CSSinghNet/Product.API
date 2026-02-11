using Microsoft.EntityFrameworkCore;
using Product.Application.Interfaces;
using Product.Core.Entities;
using Product.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Product.Infrastructure.Implementation
{
  
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Products?> GetByIdAsync(int id)
        {
            return await _context.Products
                .AsNoTracking()           // ← good default for read-only queries
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Products>> GetAllAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .OrderBy(p => p.Name)     // ← optional: default sorting
                .ToListAsync();
        }

        public async Task<Products> AddAsync(Products product)
        {
            ArgumentNullException.ThrowIfNull(product);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product; // ← now has the generated Id
        }

        public async Task UpdateAsync(int id, UpdateProductsDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) throw new KeyNotFoundException();

            // Apply changes
            if (dto.Name != null) product.Name = dto.Name;
         

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)  
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products
                .AsNoTracking()
                .AnyAsync(p => p.Id == id);
        }
    }
}
