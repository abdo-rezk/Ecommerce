using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Ecommerce.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static StackExchange.Redis.Role;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            if (basket == null) return NotFound();
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            var result =await _basketRepository.DeleteBasketAsync(id);
            if (!result) return NotFound();
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            if (basket == null || string.IsNullOrEmpty(basket.Id))
            {
                return BadRequest("Invalid basket data.");
            }
            var CustomerBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var updatedBasket = await _basketRepository.UpdateBasketAsync(CustomerBasket);
            if (updatedBasket == null) return NotFound();
            return Ok(updatedBasket);
        }
    }
                         
}
