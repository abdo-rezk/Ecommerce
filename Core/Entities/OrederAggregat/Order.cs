using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrederAggregat
{
    public class Order:BaseEntity
    {
        public Order()
        {
            
        }
        public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, Address shipToAddress, DeliveryMethod deliveryMethod, decimal subTotal)
        {
            OrderItems = orderItems;
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; }= DateTime.Now;
        public Address ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem>  OrderItems{ get; set; }
        public decimal SubTotal{ get; set; }
        public OrderStatues Statues { get; set; } = OrderStatues.Pending;
        public string PaymentIntentId { get; set; } = "";
        public decimal GetTotal()
        {
            return SubTotal + DeliveryMethod.price;
        }
    }
}
