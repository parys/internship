using Elevel.Domain.Models;
using Elevel.Infrastructure.Persistence.Context;
using Elevel.Infrastructure.Services.Jobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Elevel.Api
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var context = services.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                var serviceProvider = scope.ServiceProvider;

                try
                {
                    //Seed Default Users
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                    EmailScheduler.Start(serviceProvider, context, userManager);

                    await ApplicationDbContextSeed.SeedEssentialsAsync(userManager, roleManager);
                    
                    await ApplicationDbContextSeed.SeedDatabaseDataAsync(userManager, context);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
            host.Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
    }
}
