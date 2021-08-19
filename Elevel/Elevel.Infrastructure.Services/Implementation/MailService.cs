using Elevel.Application.Infrastructure;
using Elevel.Application.Infrastructure.Configurations;
using Elevel.Application.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;

namespace Elevel.Infrastructure.Services.Implementation
{
    public class MailService : IMailService
    {
        private MimeMessage _message;
        private SmtpClient _smtpClient;
        private readonly EmailConfigurations _emailConfiguration;

        public MailService(IOptions<EmailConfigurations> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
            _message = new MimeMessage();
        }

        public void NotifyUser(string Useremail, string subject, string body)
        {
            var emailForm = new List<EmailFormConfiguration>();

            var email = new EmailFormConfiguration();
            email.ReceiverEmails.Add(MailboxAddress.Parse(Useremail));
            email.Subject = subject;
            email.Body = body;

            emailForm.Add(email);

            UsersEmailNotification(emailForm);
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
            try
            {
                using (StreamReader str = new(Path.Combine(Constants.EMAIL_PATH, Constants.EMAIL_TEMPLATE)))
                {
                    var html = str.ReadToEnd();
                    str.Close();
                    return html;
                }
            }
            catch (Exception)
            {
                throw new NotFoundException($"File: {Constants.EMAIL_TEMPLATE}");
            }
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
