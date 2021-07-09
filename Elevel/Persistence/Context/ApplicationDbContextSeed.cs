using System;
using System.Threading.Tasks;
using Elevel.Domain;
using Elevel.Domain.Enums;
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
            await roleManager.CreateAsync(new IdentityRole<Guid>(UserRole.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole<Guid>(UserRole.User.ToString()));
            await roleManager.CreateAsync(new IdentityRole<Guid>(UserRole.Coach.ToString()));
            await roleManager.CreateAsync(new IdentityRole<Guid>(UserRole.HumanResourceManager.ToString()));

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