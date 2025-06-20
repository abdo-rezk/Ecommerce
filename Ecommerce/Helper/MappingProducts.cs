using AutoMapper;
using Core.Entities;
using Ecommerce.DTO;
namespace Ecommerce.Helper
{
    public class MappingProducts: Profile
    {
        public MappingProducts()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                // to return picture name with Domain
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<ProductUrlResolver>());
        }
    }
}
