using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using Elevel.Infrastructure.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Elevel.Infrastructure.Services.Jobs
{
    public class EmailJob : IJob
    {
        private IMailService _mail;

        public EmailJob(IServiceScopeFactory serviceScopeFactory, IOptions<EmailConfiguration> emailConfiguration)
        {
            var service = (serviceScopeFactory.CreateScope()).ServiceProvider;

            var userManager = service.GetService<UserManager<User>>();

            var context = service.GetService<IApplicationDbContext>();

            _mail = new MailService(userManager, emailConfiguration, context);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _mail.SendNotificationsToHrsAndUsersAsync();
        }
    }
}
