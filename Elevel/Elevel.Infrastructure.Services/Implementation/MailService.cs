using Elevel.Application.Infrastructure;
using Elevel.Application.Infrastructure.Configurations;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Elevel.Infrastructure.Services.Implementation
{
    public class MailService : IMailService
    {
        private MimeMessage _message;
        private SmtpClient _smtpClient;
        private readonly EmailConfigurations _emailConfiguration;
        private readonly UserManager<User> _userManager;

        public MailService(UserManager<User> userManager, IOptions<EmailConfigurations> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
            _userManager = userManager;
            _message = new MimeMessage();
        }

        public string SendMessage(Guid receiverId, string subject, string body)
        {
            Connect();
            var userEmail = _userManager.Users.FirstOrDefault(x => x.Id == receiverId).Email;
            if (userEmail == null)
            {
                return "Email was not sent";
            }

            string html = GetHtml();

            html = html.Replace("[Content]", body);

            _message.From.Add(new MailboxAddress("Elevel Notification", _emailConfiguration.Email));
            _message.To.Add(MailboxAddress.Parse(userEmail));
            _message.Subject = subject;
            _message.Body = new TextPart("html") 
            {
                Text = html
            };

            try
            {
                _smtpClient.Send(_message);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                Disconnect();
            }
            return "Email was sent successfully";
        }

        public string UsersEmailNotification(List<EmailFormConfiguration> emails)
        {
            Connect();

            _message.From.Add(new MailboxAddress("Elevel Notification", _emailConfiguration.Email));

            string html = GetHtml();

            foreach (var email in emails)
            {
                if (_message.To.Count > 0)
                {
                    _message.To.Clear();
                }

                html = html.Replace("[Content]", email.Body);

                _message.To.AddRange(email.ReceiverEmails);
                _message.Subject = email.Subject;
                _message.Body = new TextPart("html")
                {
                    Text = html
                };

                try
                {
                    _smtpClient.Send(_message);
                }
                catch (Exception ex)
                {
                    Disconnect();
                    return ex.Message;
                }
            }

            Disconnect();

            return "Email was sent successfully";
        }

        private string GetHtml()
        {
            StreamReader str;
            try
            {
                str = new(Path.Combine(Constants.EMAIL_PATH, Constants.EMAIL_TEMPLATE));
            }
            catch (Exception)
            {
                throw new NotFoundException($"File: {Constants.EMAIL_TEMPLATE}");
            }
            var html = str.ReadToEnd();
            str.Close();
            return html;
        }

        private void Connect()
        {
            _smtpClient = new SmtpClient();
            try
            {
                _smtpClient.Connect("smtp.gmail.com", 465, true);
                _smtpClient.Authenticate(_emailConfiguration.Email,
                    _emailConfiguration.Password);
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        private void Disconnect()
        {
            _smtpClient.Disconnect(true);
            _smtpClient.Dispose();
        }
        
        ~MailService()
        {
            _smtpClient.Dispose();
        }
    }
}
