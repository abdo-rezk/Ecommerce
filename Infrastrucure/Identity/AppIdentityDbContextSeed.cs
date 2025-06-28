using Core.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Rezk",
                    Email = "abdo2rezk@gmail.com",
                    UserName = "abdo2rezk@gmail.com",
                    Address = new Address
                    {
                        FirstName = "Abdo",
                        LastName = "Rezk",
                        Street = "Dronkka",
                        City = "Assiut",
                        State = "Egyption",
                        ZipCode = "71707"
                    }

                };
                await userManager.CreateAsync(user, "Pa$$w0rd");

            }
        }
    }
}
