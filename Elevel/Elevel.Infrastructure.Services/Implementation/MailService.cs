using Elevel.Application.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;

namespace Elevel.Infrastructure.Services.Implementation
{
    public class MailService : IMailService
    {
        private MimeMessage _message;
        private SmtpClient _smtpClient;
        private string adminEmail = "elevelexadel@gmail.com";
        private string adminPassword = "admin4elevel";

        public MailService()
        {
            _message = new MimeMessage();
            _smtpClient = new SmtpClient();
        }

        public string SendMessage(string receiverEmail, string subject, string body)
        {
            _message.From.Add(new MailboxAddress("Elevel Notification", adminEmail));
            _message.To.Add(MailboxAddress.Parse(receiverEmail));
            _message.Subject = subject;
            _message.Body = new TextPart("plain")
            {
                Text = body
            };

            try
            {
                _smtpClient.Connect("smtp.gmail.com", 465, true);
                _smtpClient.Authenticate(adminEmail, adminPassword);
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

        public string UsersEmailNotification(List<string> receiverEmails, string subject, string body)
        {
            try
            {
                _smtpClient.Connect("smtp.gmail.com", 465, true);
                _smtpClient.Authenticate(adminEmail, adminPassword);
            }
            catch (Exception ex)
            {
                _smtpClient.Disconnect(true);
                _smtpClient.Dispose();
                return ex.Message;
            }

            _message.From.Add(new MailboxAddress("Elevel Notification", adminEmail));
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

            _smtpClient.Disconnect(true);
            _smtpClient.Dispose();

            return "Email was sent successfully";
        }
    }
}
