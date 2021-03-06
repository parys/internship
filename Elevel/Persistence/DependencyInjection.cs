using Elevel.Application.Infrastructure.Configurations;
using Elevel.Application.Interfaces;
using Elevel.Infrastructure.Persistence.Context;
using Elevel.Infrastructure.Services.Implementation;
using Elevel.Infrastructure.Services.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elevel.Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = string.Empty;
#if DEBUG
            connectionString = configuration.GetConnectionString("DefaultConnection");
#else
            connectionString = configuration.GetConnectionString("ProductionConnection");
#endif

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();

            services.AddTransient<JobFactory>();
            services.AddScoped<EmailJob>();
            services.AddScoped<IMailService, MailService>();
            services.Configure<EmailConfigurations>(configuration.GetSection("EmailSender"));
            services.Configure<SchedulerConfigurations>(configuration.GetSection("SchedulerConfiguration"));
        }
    }
}