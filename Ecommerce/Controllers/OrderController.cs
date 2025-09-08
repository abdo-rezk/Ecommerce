using AutoMapper;
using Core.Entities.OrederAggregat;
using Core.Interfaces;
using Ecommerce.DTO;
using Ecommerce.Error;
using Ecommerce.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(OrderDto orderDto)
        {
         //   var email = User?.Claims?.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            var Email = HttpContext.User?.RetrieveEmailFromPrincipal();
            var address = _mapper.Map<AddressDto, Core.Entities.OrederAggregat.Address>(orderDto.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(Email, orderDto.DeivertMethodId, orderDto.BasketId, address);
            if (order == null) return BadRequest(new ApiResponse(400,"Problem creating order"));
            return Ok(order);

        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrderForUser()
        {
            var Email = HttpContext.User?.RetrieveEmailFromPrincipal();
            var orders = await _orderService.GetOrderForUserAsync(Email);
            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int id)
        {
            var Email = HttpContext.User?.RetrieveEmailFromPrincipal();
            var order = await _orderService.GetOrderByIdAsync(id, Email);
            if (order == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<OrderToReturnDto>(order);
        }
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }   
    }
}
