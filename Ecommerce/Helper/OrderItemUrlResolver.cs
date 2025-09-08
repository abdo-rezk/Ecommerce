using AutoMapper;
using Core.Entities;
using Core.Entities.OrederAggregat;
using Ecommerce.DTO;

namespace Ecommerce.Helper
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _config;
        public OrderItemUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOredered.PictureUrl))
            {
                return _config["ApiUrl"] + source.ItemOredered.PictureUrl;
            }
            return null;
        }
    }
}
