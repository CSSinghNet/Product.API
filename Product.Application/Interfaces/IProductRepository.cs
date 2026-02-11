using System;
using System.Collections.Generic;
using System.Text;
using Product.Core.Entities;

namespace Product.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Products?> GetByIdAsync(int id);
        Task<IReadOnlyList<Products>> GetAllAsync();
        Task<Products> AddAsync(Products product);
        Task UpdateAsync(int id, UpdateProductsDto product);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
