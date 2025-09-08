using AutoMapper;
using Core.Entities;
using Core.Entities.OrederAggregat;
using Core.Identity;
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

            CreateMap<Core.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
            CreateMap<AddressDto,Core.Entities.OrederAggregat.Address>();

            CreateMap<Order,OrderToReturnDto>()
                .ForMember(d=>d.DeliveryMethod,o=>o.MapFrom(s=>s.DeliveryMethod.ShortName))
                .ForMember(d=>d.ShipingPrice,o=>o.MapFrom(s=>s.DeliveryMethod.price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOredered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOredered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOredered.ProductUrl)) // pic Url
                // to return picture name with Domain
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
               
        }
    }
}
