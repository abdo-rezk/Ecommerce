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
        public ProductWithTypesAndBrandsSpacification(string sort)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            ApplyOrderBy(x => x.Name);
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
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
