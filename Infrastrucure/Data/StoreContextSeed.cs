using Core.Entities;
using Core.Entities.OrederAggregat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastrucure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.Products.Any())
            {
                var ProductData = File.ReadAllText("../Infrastrucure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                context.Products.AddRange(products);
            }
            if (!context.DeliveryMethods.Any())
            {
                var DeliveryData = File.ReadAllText("../Infrastrucure/Data/SeedData/delivery.json");
                var Methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);
                context.DeliveryMethods.AddRange(Methods);
            }
            if (!context.ProductBrands.Any())
            {
                var BrandData = File.ReadAllText("../Infrastrucure/Data/SeedData/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                context.ProductBrands.AddRange(Brands);
            }
            if (!context.ProductTypes.Any())
            {
                var TypeData = File.ReadAllText("../Infrastrucure/Data/SeedData/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypeData);
                context.ProductTypes.AddRange(Types);
            }
            if (context.ChangeTracker.HasChanges())await context.SaveChangesAsync();

        }
    }
}
