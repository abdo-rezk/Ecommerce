using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastrucure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int Id)
        {
            //Include to be Eager loading instead of Lazy loading
            var product =await _context.Products
                .Include(p=>p.ProductBrand)
                .Include(p=>p.ProductType)
                .FirstOrDefaultAsync(p=>p.Id==Id);  
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            //Include to be Eager loading instead of Lazy loading
            var products =await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .ToListAsync();
            return products;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            var brands =await _context.ProductBrands.ToListAsync();
            return brands;
        }
        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            var types = await _context.ProductTypes.ToListAsync();
            return types;
        }
    }
}
