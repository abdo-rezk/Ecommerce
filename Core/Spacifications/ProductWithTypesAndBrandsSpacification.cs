using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Spacifications
{
    public class ProductWithTypesAndBrandsSpacification : BaseSpacification<Product>
    {
        // PrandId && TypeId && Search given to base constractor BaseSpacification to filter products by brand and type by Criteria
        public ProductWithTypesAndBrandsSpacification(ProductSpecParams productParams) :base(x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search.Trim())) &&
            (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
            (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId))
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            ApplyOrderBy(x => x.Name);
            ApplyPaging(productParams.PageSize *(productParams.PageIndex - 1), productParams.PageSize);
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        ApplyOrderBy(x => x.Price);
                        break;
                    case "priceDesc":
                        ApplyOrderByDescending(x => x.Price);
                        break;
                    default:
                        ApplyOrderBy(x => x.Name);
                        break;
                }
            }
        }
        public ProductWithTypesAndBrandsSpacification(int id) : base(x=>x.Id==id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);    
        }
    }
}
