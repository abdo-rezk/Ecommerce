using Core.Entities;
using Core.Entities.OrederAggregat;
using Core.Interfaces;
using Core.Spacifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucure.Services
{
    public class OrderService : IOrderService
    {
        /*private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<DeliveryMethod> _deliveryRepo;*/
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService( IBasketRepository basketRepo, IUnitOfWork unitOfWork)
        {
          /*  _orderRepo = orderRepo;
            _productRepo = productRepo;
            _deliveryRepo = deliveryRepo;*/
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }


        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId, Address shippingAddess)
        {
            //get basket from basket repo
            var basket=await _basketRepo.GetBasketAsync(basketId);

            //get items from product repo 
            var items= new List<OrderItem>();
            foreach (var item in basket.Item)
            {
                //var productItem = await _productRepo.GetByIdAsync(item.Id);
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOredered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            //get deliveryMethod for deliveary repo 
           // var dlvMethod = await _deliveryRepo.GetByIdAsync(deliveryMethod);
            var dlvMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethod);

            //calc subTotal
            var subTotal = items.Sum(i => i.Quantity * i.Price);

            //create order 
            var order = new Order(items, buyerEmail,shippingAddess, dlvMethod,  subTotal);
            _unitOfWork.Repository<Order>().Add(order);
            //save to db
            var result=await _unitOfWork.Complete();
            if (result <= 0) return null;

            //delete basket
            await _basketRepo.DeleteBasketAsync(basketId);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrderWithItemAndOrderingSpecification(id, buyerEmail);
            return _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemAndOrderingSpecification(buyerEmail);
            return _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}
