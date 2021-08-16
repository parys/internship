using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevel.Infrastructure.Services.Implementation
{
    public class MailService : IMailService
    {
        private MimeMessage _message;
        private SmtpClient _smtpClient;
        private readonly UserManager<User> _userManager;
        private readonly EmailConfiguration _emailConfiguration;

        public MailService(UserManager<User> userManager, IOptions<EmailConfiguration> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
            _userManager = userManager;
            _message = new MimeMessage();
            _smtpClient = new SmtpClient();
        }

        public void Connect()
        {
            try
            {
                _smtpClient.Connect("smtp.gmail.com", 465, true);
                _smtpClient.Authenticate(_emailConfiguration.Email,
                    _emailConfiguration.Password);
            }
            catch (Exception ex)
            {
                _smtpClient.Disconnect(true);
                _smtpClient.Dispose();
            }
        }

        public void Disconnect()
        {
            _smtpClient.Disconnect(true);
            _smtpClient.Dispose();
        }

        public string SendMessage(Guid receiverId, string subject, string body)
        {
            var userEmail = _userManager.Users.FirstOrDefault(x => x.Id == receiverId).Email;
            if (userEmail == null)
            {
                return "Email was not sent";
            }

            _message.From.Add(new MailboxAddress("Elevel Notification", _emailConfiguration.Email));
            _message.To.Add(MailboxAddress.Parse(userEmail));
            _message.Subject = subject;
            _message.Body = new TextPart("plain")
            {
                Text = body
            };

            try
            {
                _smtpClient.Connect("smtp.gmail.com", 465, true);
                _smtpClient.Authenticate(_emailConfiguration.Email,
                    _emailConfiguration.Password);
                _smtpClient.Send(_message);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                _smtpClient.Disconnect(true);
                _smtpClient.Dispose();
            }
            return "Email was sent successfully";
        }

        public string UsersEmailNotification(List<Guid> receiverIds, string subject, string body)
        {
            var receiverEmails = _userManager.Users.Where(x => receiverIds.Contains(x.Id)).Select(x => x.Email).ToList();
            if (receiverEmails == null)
            {
                return "Email was not sent";
            }

            _message.From.Add(new MailboxAddress("Elevel Notification", _emailConfiguration.Email));
            _message.Body = new TextPart("plain")
            {
                Text = body
            };
            _message.Subject = subject;


            foreach (var receiverEmail in receiverEmails)
            {
                _message.To.Add(MailboxAddress.Parse(receiverEmail));

                try
                {
                    _smtpClient.Send(_message);
                }
                catch (Exception ex)
                {
                    _smtpClient.Disconnect(true);
                    _smtpClient.Dispose();
                    return ex.Message;
                }
            }

            return "Email was sent successfully";
        }

        public string MissedDeadlineEmailNotification(List<Test> tests, string subject, string body)
        {
            var UsersAndHrs = tests.GroupBy(x => x.HrId, (key, value) => new
            {
                HrId = key,
                UserIds = value.Select(x => x.UserId)
            });

            _message.From.Add(new MailboxAddress("Elevel Notification", _emailConfiguration.Email));

            _message.Subject = subject;


            foreach (var pair in UsersAndHrs)
            {
                var hrEmail = _userManager.Users.FirstOrDefault(x => x.Id == pair.HrId).Email;
                var Users = _userManager.Users.Where(x => pair.UserIds.Contains(x.Id));

                if (hrEmail == null)
                {
                    return "Email was not sent";
                }
                if (Users == null)
                {
                    return "Email was not sent";
                }

                _message.To.Add(MailboxAddress.Parse(hrEmail));
                foreach (var user in Users)
                {
                    _message.Body = new TextPart("plain")
                    {
                        Text = body
                        .Replace("{FirstName}", user.FirstName, StringComparison.CurrentCultureIgnoreCase)
                        .Replace("{LastName}", user.LastName, StringComparison.CurrentCultureIgnoreCase)
                        .Replace("{Email}", user.Email, StringComparison.CurrentCultureIgnoreCase)
                    };
                    try
                    {
                        _smtpClient.Send(_message);
                    }
                    catch (Exception ex)
                    {
                        _smtpClient.Disconnect(true);
                        _smtpClient.Dispose();
                        return ex.Message;
                    }
                }
            }
            return "Email was sent successfully";
        }

        ~MailService()
        {
            _smtpClient.Dispose();
        }
    }
}
