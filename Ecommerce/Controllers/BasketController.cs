using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository; 
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
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            if (basket == null || string.IsNullOrEmpty(basket.Id))
            {
                return BadRequest("Invalid basket data.");
            }
            var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);
            if (updatedBasket == null) return NotFound();
            return Ok(updatedBasket);
        }
    }
                         
}
