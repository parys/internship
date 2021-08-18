using Elevel.Application.Extensions;
using Elevel.Application.Infrastructure.Configurations;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using Elevel.Infrastructure.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MimeKit;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elevel.Infrastructure.Services.Jobs
{
    public class EmailJob : IJob
    {
        private IMailService _mail;
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public EmailJob(IServiceScopeFactory serviceScopeFactory, IOptions<EmailConfigurations> emailConfiguration)
        {
            var service = (serviceScopeFactory.CreateScope()).ServiceProvider;

            _userManager = service.GetService<UserManager<User>>();

            _context = service.GetService<IApplicationDbContext>();

            _mail = new MailService(_userManager, emailConfiguration);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var userEmails = await _context.Tests
                .Where(x => x.AssignmentEndDate.HasValue
                    && DateTimeOffset.Compare(x.AssignmentEndDate.Value.Date, DateTimeOffset.UtcNow.Date) >= 0)
                .Include(x => x.User)
                .AsNoTracking()
                .Select(x => MailboxAddress.Parse(x.User.Email))
                .AsNoTracking()
                .ToListAsync();

            var missedDeadlineUsers = await _context.Tests
                .Where(x => x.AssignmentEndDate.HasValue
                    && DateTimeOffset.Compare(x.AssignmentEndDate.Value.AddDays(1).Date, DateTimeOffset.UtcNow.Date) == 0
                    && !x.GrammarMark.HasValue)
                .Include(x=> x.Hr)
                .Include(x=> x.User)
                .Select(x => new
                {
                    HrEmail = MailboxAddress.Parse(x.Hr.Email),
                    UserName = x.User.GetUserNames()
                })
                .AsNoTracking()
                .ToListAsync();

            var missedDict = new Dictionary<MailboxAddress, string>();

            foreach (var hr in missedDeadlineUsers)
            {
                if (!missedDict.ContainsKey(hr.HrEmail))
                {
                    missedDict.Add(hr.HrEmail, hr.UserName);
                }
                else
                {
                    missedDict[hr.HrEmail] = missedDict[hr.HrEmail] + $",\n{hr.UserName}";
                }
            }

            var emailForms = new List<EmailFormConfiguration>();

            var userEmailForm = new EmailFormConfiguration();

            if (userEmails.Count > 0)
            {
                userEmailForm.ReceiverEmails.AddRange(userEmails);
                userEmailForm.Subject = "Elevel test reminder";
                userEmailForm.Body = "You have assigned for test. \nCheck your profile.";
                emailForms.Add(userEmailForm);
            }

            foreach (var hr in missedDict)
            {
                var hrEmailForm = new EmailFormConfiguration();

                hrEmailForm.ReceiverEmails.Add(hr.Key);
                hrEmailForm.Subject = "Missed deadline users";
                hrEmailForm.Body = $"There are some users have missed the deadline:\n{hr.Value}";

                emailForms.Add(hrEmailForm);
            }

            if (emailForms.Count > 0) {
                _mail.UsersEmailNotification(emailForms);
            } 
        }
    }
}
