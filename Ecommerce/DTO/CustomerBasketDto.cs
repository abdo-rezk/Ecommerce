using Core.Entities;

namespace Ecommerce.DTO
{
    public class CustomerBasketDto
    {
        public string Id { get; set; }
        public List<BasketItemDto> Item { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
