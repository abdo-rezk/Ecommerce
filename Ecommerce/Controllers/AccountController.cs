using AutoMapper;
using Core.Identity;
using Core.Interfaces;
using Ecommerce.DTO;
using Ecommerce.Error;
using Ecommerce.Extensions;
using Infrastrucure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet("secret")]
        public string getsecret()
        {
            return "the secret";
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));
            // false in 3rd parameter means that we don't want to lock the user out after a failed login attempt
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            var x = new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
            return x;

        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email

            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
            };

        }
        [Authorize]
        [HttpGet("GetcurrentUSer")]
        public async Task<ActionResult<UserDto>> GetcurrentUSer()
        {
            //way 1 to get user
            //var email = HttpContext.User?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email)?.Value;
            //var user = await _userManager.FindByEmailAsync(email);
            
            // with extension method
            var user= await _userManager.FindByEmailFromClaimsPrinciple(User);
            if (user == null) return Unauthorized(new ApiResponse(401));
            return new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
            };

        }
        [HttpGet("emailexist")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {   // way 1 to get user address
            /* var email = HttpContext.User?.Claims.FirstOrDefault(e => e.Type == ClaimTypes.Email)?.Value;
             var user = await _userManager.FindByEmailAsync(email);*/

            // with extension method
            var user =await _userManager.FindUSerByClaimsPrincipleWithAddress(User);
            if (user == null) return Unauthorized(new ApiResponse(401));
            return _mapper.Map<Address,AddressDto>(user.Address);
        }
        [HttpPut("address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindUSerByClaimsPrincipleWithAddress(User);
            if(user == null) return Unauthorized(new ApiResponse(401));
            user.Address = _mapper.Map<AddressDto, Address>(address);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));
            return BadRequest("Problem updating the user");
        } 
    }
}