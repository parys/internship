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
                .AsNoTracking()
                .Select(x => MailboxAddress.Parse(
                    _userManager.Users.FirstOrDefault(u => u.Id == x.UserId).Email))
                .AsNoTracking()
                .ToListAsync();

            var missedDeadlineUsers = await _context.Tests
                .Where(x => x.AssignmentEndDate.HasValue
                    && DateTimeOffset.Compare(x.AssignmentEndDate.Value.AddDays(1).Date, DateTimeOffset.UtcNow.Date) == 0
                    && !x.GrammarMark.HasValue)
                .Select(x => new
                {
                    HrEmail = MailboxAddress.Parse(
                    _userManager.Users.FirstOrDefault(u => u.Id == x.HrId).Email),
                    UserName = _userManager.Users.FirstOrDefault(u => u.Id == x.UserId).GetUserNames()
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
                userEmailForm.ReceiverEmail.AddRange(userEmails);
                userEmailForm.Subject = "Elevel test reminder";
                userEmailForm.Body = "You have assigned for test. \nCheck your profile.";
                emailForms.Add(userEmailForm);
            }
            if (missedDict.Count > 0) {
                foreach (var hr in missedDict)
                {
                    var hrEmailForm = new EmailFormConfiguration();

                    hrEmailForm.ReceiverEmail.Add(hr.Key);
                    hrEmailForm.Subject = "Missed deadline users";
                    hrEmailForm.Body = $"There are some users have missed the deadline:\n{hr.Value}";

                    emailForms.Add(hrEmailForm);
                }
            }
            if (emailForms.Count > 0) {
                _mail.UsersEmailNotification(emailForms);
            } 
        }
    }
}
