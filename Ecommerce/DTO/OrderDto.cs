namespace Ecommerce.DTO
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public int DeivertMethodId { get; set; }
        public AddressDto ShipToAddress { get; set; }
    }
}
