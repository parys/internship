using System.Threading.Tasks;
using Elevel.Domain;
using Elevel.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Elevel.Persistence.Context
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedEssentialsAsync(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.User.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.Coach.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.HumanResourceManager.ToString()));

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