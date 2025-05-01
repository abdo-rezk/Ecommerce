using Ecommerce.Error;
using Infrastrucure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;
        public BuggyController(StoreContext context)
        {
            _context = context;
        }
        [HttpGet("notfound")]
        public IActionResult NotFound()
        {
            var thing = _context.Products.Find(42);
            if(thing == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(thing);
        }
        [HttpGet("servererror")]
        public IActionResult ServerError()
        {
            var thing = _context.Products.Find(42);
            var thingToReturn = thing.ToString();
            return Ok(thingToReturn);
        }
        [HttpGet("badrequest")]
        public IActionResult BadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("unauthorized")]
        public IActionResult Unauthorized()
        {
            return Unauthorized("You are not authorized to access this resource");
        }
        [HttpGet("testauth")]
        [Authorize]
        public IActionResult TestAuth()
        {
            return Ok("You are authorized");
        }


    }
}
