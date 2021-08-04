using System;
using System.Threading.Tasks;
using Elevel.Domain;
using Elevel.Domain.Enums;
using Elevel.Domain.Models;
using Elevel.Infrastructure.Persistence.FillDbScript;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Elevel.Infrastructure.Persistence.Context
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedEssentialsAsync(UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            //Seed Roles
            if (! await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(UserRole.Administrator.ToString()));
                await roleManager.CreateAsync(new IdentityRole<Guid>(UserRole.User.ToString()));
                await roleManager.CreateAsync(new IdentityRole<Guid>(UserRole.Coach.ToString()));
                await roleManager.CreateAsync(new IdentityRole<Guid>(UserRole.HumanResourceManager.ToString()));
            }

            if(!await userManager.Users.AnyAsync())
            { 
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

        public static async Task SeedDatabaseDataAsync(UserManager<User> userManager, ApplicationDbContext context)
        {
            await TestDataDb.FillDB(context, userManager);
        }
    }
}