using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
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
        private string adminEmail = "elevelexadel@gmail.com";
        private string adminPassword = "admin4elevel";

        public MailService()
        {
            _message = new MimeMessage();
            _smtpClient = new SmtpClient();
        }

        public string SendMessage(UserManager<User> userManager, Guid receiverId, string subject, string body)
        {
            var userEmail = userManager.Users.FirstOrDefault(x => x.Id == receiverId).Email;
            if(userEmail == null)
            {
                return "Email was not sent";
            }

            _message.From.Add(new MailboxAddress("Elevel Notification", adminEmail));
            _message.To.Add(MailboxAddress.Parse(userEmail));
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

        public string UsersEmailNotification(UserManager<User> userManager, List<Guid> receiverIds, string subject, string body)
        {
            var receiverEmails = userManager.Users.Where(x => receiverIds.Contains(x.Id)).Select(x => x.Email).ToList();
            if (receiverEmails == null)
            {
                return "Email was not sent";
            }

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
