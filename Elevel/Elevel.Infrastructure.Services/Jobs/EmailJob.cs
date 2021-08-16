using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using Elevel.Infrastructure.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Elevel.Infrastructure.Services.Jobs
{
    public class EmailJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IApplicationDbContext _context;
        private UserManager<User> _userManager;
        private IMailService _mail;

        public EmailJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mail = new MailService();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider;
                _context = service.GetService<IApplicationDbContext>();
                _userManager = service.GetService<UserManager<User>>();
                
                var userIds = await _context.Tests
                    .Where(x => x.AssignmentEndDate.HasValue
                        && DateTimeOffset.Compare(x.AssignmentEndDate.Value.Date, DateTimeOffset.UtcNow.Date) >= 0)
                    .AsNoTracking()
                    .Select(x => x.UserId)
                    .ToListAsync();

                try
                {
                    _mail.UsersEmailNotification(_userManager,userIds,
                        "Elevel test",
                        "Hi!\nYou have a test assigned on you");
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }
        }
    }
}
