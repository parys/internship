using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Elevel.Infrastructure.Services.Jobs
{
    public class EmailJob : IJob
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public EmailJob(IServiceScopeFactory serviceScopeFactory, IApplicationDbContext context, UserManager<User> userManager)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            _context = context;
            _userManager = userManager;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var emailsender = scope.ServiceProvider.GetService<IEmailSender>();
                try
                {
                    //await emailsender.SendEmailAsync("arohau@exadel.com", "example", "hello");
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}
