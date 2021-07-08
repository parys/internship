using System;
using System.Threading.Tasks;
using Elevel.Domain;
using Elevel.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Elevel.Infrastructure.Persistence.Context
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedEssentialsAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole<Guid>(Authorization.Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole<Guid>(Authorization.Roles.User.ToString()));
            await roleManager.CreateAsync(new IdentityRole<Guid>(Authorization.Roles.Coach.ToString()));
            await roleManager.CreateAsync(new IdentityRole<Guid>(Authorization.Roles.HumanResourceManager.ToString()));

            //Seed Default User
            foreach (var userSet in Authorization.DefaultUsers)
            {
                foreach (var user in userSet.Value)
                {
                    await userManager.CreateAsync(user, Authorization.DefaultPassword);
                    await userManager.AddToRoleAsync(user, userSet.Key.ToString());
                }
                
            }
        }
    }
}