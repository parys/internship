using Elevel.Application.Interfaces;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Elevel.Infrastructure.Services.Jobs
{
    public class EmailSender : IEmailSender
    {
		public Task SendEmailAsync(string email, string subject, string message)
		{
			var from = "rogovandrey8@gmail.com";
			var pass = "";
			SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
			client.DeliveryMethod = SmtpDeliveryMethod.Network;
			client.UseDefaultCredentials = false;
			client.Credentials = new System.Net.NetworkCredential(from, pass);
			client.EnableSsl = true;
			var mail = new MailMessage(from, email);
			mail.Subject = subject;
			mail.Body = message;
			mail.IsBodyHtml = true;
			return client.SendMailAsync(mail);
		}
	}
}
