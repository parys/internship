

using AutoMapper;
using Elevel.Infrastructure.Services.Implementation;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class EmailNotificationController : ControllerBase
    {
        private MimeMessage _message;
        private SmtpClient _smtpClient;

        public EmailNotificationController()
        {
            _message = new MimeMessage();
            _smtpClient = new SmtpClient();

        }

        [HttpPost("send")]
        public IActionResult SendMail([FromBody]string email, string subject, string body)
        {
            var message = new MailService();
            return Ok(message.SendMessage(email, subject, body));
        }

        //[HttpPost("sendEmailTemplate")]
        //public async Task<IActionResult> SendWelcomeMail([FromForm] MailSource source)
        //{
        //    try
        //    {
        //        await mailService.SendEmailTemplateAsynk(source);
        //        return Ok();
        //    }
        //    catch(Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}
