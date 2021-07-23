using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Application.Infrastructure
{
    public class EmailNotification
    {
            public async Task SendEmailAsync(string email, string subject, string message)
            {
                var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Elevel", "eleve@elevel.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

                using (var client = new SmtpClient())
                {
                     await client.ConnectAsync("smpt.gmail.com", 465, false);
                     await client.AuthenticateAsync("evgeniykowpak@gmail.com", "password");
                     await client.SendAsync(emailMessage);
                     await client.DisconnectAsync(true);
                    
                }   
            }
    }
}
