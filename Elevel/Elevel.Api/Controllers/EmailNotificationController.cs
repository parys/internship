using Elevel.Application.Infrastructure;
using Elevel.Application.Interfaces;
using Elevel.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elevel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailNotificationController : ControllerBase
    {

        private readonly IMailService mailService;

        public EmailNotificationController(IMailService mailService)
        {
            this.mailService = mailService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("sendEmailTemplate")]
        public async Task<IActionResult> SendWelcomeMail([FromForm] MailSource source)
        {
            try
            {
                await mailService.SendEmailTemplateAsynk(source);
                return Ok();
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
