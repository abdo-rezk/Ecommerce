﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int Id);
        Task<IReadOnlyList<Product>> GetProductsAsync(); 
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
        /*
                Task<IEnumerable<Product>> GetAllProductsAsync();
                Task<Product> GetProductByIdAsync(int id);
                Task AddProductAsync(Product product);
                Task UpdateProductAsync(Product product);
                Task DeleteProductAsync(int id);*/

    }
}
