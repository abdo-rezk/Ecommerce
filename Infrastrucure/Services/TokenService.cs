using Core.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
        }
        public string CreateToken(AppUser user)
        {
            var claims =new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
               // new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.GivenName, user.DisplayName),
            };
            var crad= new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var TokenDiscripror = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = crad,
                Issuer = _config["Token:Issuer"],
                Audience = _config["Token:Audience"]

            };
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(TokenDiscripror);
            return tokenHandler.WriteToken(token);
        }
        
    }
}
