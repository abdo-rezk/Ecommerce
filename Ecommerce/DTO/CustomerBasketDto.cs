using Core.Entities;

namespace Ecommerce.DTO
{
    public class CustomerBasketDto
    {
        public string Id { get; set; }
        public List<BasketItemDto> Item { get; set; } 
    }
}
