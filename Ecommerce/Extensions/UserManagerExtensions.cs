using Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecommerce.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUSerByClaimsPrincipleWithAddress(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return await userManager.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);
        }
        public static async Task<AppUser> FindByEmailFromClaimsPrinciple(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return await userManager.FindByEmailAsync(email);
        }
    }
}
