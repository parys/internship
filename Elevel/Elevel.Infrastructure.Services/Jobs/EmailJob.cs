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
        private IApplicationDbContext _context;
        private IMailService _mail;

        public EmailJob(IServiceScopeFactory serviceScopeFactory, IOptions<EmailConfiguration> emailConfiguration)
        {
            var service = (serviceScopeFactory.CreateScope()).ServiceProvider;

            var userManager = service.GetService<UserManager<User>>();

            _context = service.GetService<IApplicationDbContext>();

            _mail = new MailService(userManager, emailConfiguration);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var userIds = await _context.Tests
                .Where(x => x.AssignmentEndDate.HasValue
                    && DateTimeOffset.Compare(x.AssignmentEndDate.Value.Date, DateTimeOffset.UtcNow.Date) >= 0)
                .AsNoTracking()
                .Select(x => x.UserId)
                .ToListAsync();

            var missedDeadlineUserIds = await _context.Tests
                .Where(x => x.AssignmentEndDate.HasValue
                    && DateTimeOffset.Compare(x.AssignmentEndDate.Value.Date, DateTimeOffset.UtcNow.Date) < 0
                    && !x.GrammarMark.HasValue)
                .AsNoTracking().ToListAsync();

            _mail.Connect();
            _mail.UsersEmailNotification(userIds,
                "Elevel test",
                "Hi!\nYou have a test assigned on you");

            _mail.MissedDeadlineEmailNotification(missedDeadlineUserIds,
                "Elevel test",
                "User {FirstName} {LastName} with email {Email} has lost the assignment end date!");
            _mail.Disconnect();
        }
    }
}
